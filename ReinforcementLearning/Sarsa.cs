using System;

namespace ReinforcementLearning
{
  public class Sarsa : IReinforcementLearning
  {
    private readonly double[][] _q;

    public double LearningRate { get; set; }
    public double DiscountFactor { get; set; }
    public IExplorationPolicy ExplorationPolicy { get; set; }
    public int CurrentState { get; private set; }
    public int SelectedAction { get; private set; }
    public int StateCount { get; }
    public int ActionCount { get; }

    public Sarsa(int stateCount, int actionCount, IExplorationPolicy explorationPolicy, double learningRate = 0.1,
      double discountFactor = 0.9, bool initializeRandom = false)
    {
      StateCount = stateCount;
      ActionCount = actionCount;
      ExplorationPolicy = explorationPolicy;
      LearningRate = learningRate;
      DiscountFactor = discountFactor;

      _q = new double[stateCount][];
      for (var i = 0; i < stateCount; i++) {
        _q[i] = new double[actionCount];
      }

      if (initializeRandom) {
        var random = new Random();
        for (var i = 0; i < stateCount; i++) {
          for (var j = 0; j < actionCount; j++) {
            _q[i][j] = random.NextDouble();
          }
        }
      }
    }

    public void Begin(int state)
    {
      CurrentState = state;
      SelectedAction = ExplorationPolicy.SelectAction(_q[CurrentState]);
    }

    public void Step(double reward, int nextState)
    {
      var nextAction = ExplorationPolicy.SelectAction(_q[nextState]);

      var target = reward + DiscountFactor * _q[nextState][nextAction];
      var delta = target - _q[CurrentState][SelectedAction];
      _q[CurrentState][SelectedAction] += LearningRate * delta;

      CurrentState = nextState;
      SelectedAction = ExplorationPolicy.SelectAction(_q[CurrentState]);
    }
  }
}