using System;

namespace ReinforcementLearning
{
  public class Sarsa : IReinforcementLearning
  {
    private readonly double[][] _q;

    public double LearningRate { get; set; }
    public double DiscountFactor { get; set; }
    public IExplorationPolicy ExplorationPolicy { get; set; }

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

    public int StateCount { get; }
    public int ActionCount { get; }

    public void Learn(int previousState, int action, double reward, int nextState)
    {
      var nextAction = SelectAction(nextState);

      var target = reward + DiscountFactor * _q[nextState][nextAction];
      var delta =  target - _q[previousState][action];
      _q[previousState][action] += LearningRate * delta;
    }

    public int SelectAction(int state)
    {
      return ExplorationPolicy.SelectAction(_q[state]);
    }
  }
}