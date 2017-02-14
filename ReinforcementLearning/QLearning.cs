using System;

namespace ReinforcementLearning
{
  public class QLearning
  {
    private readonly double[][] _q;

    public int StateCount { get; private set; }
    public int ActionCount { get; }

    public double LearningRate { get; set; }
    public double DiscountFactor { get; set; }
    public IExplorationPolicy ExplorationPolicy { get; set; }

    public QLearning(int stateCount,
      int actionCount,
      IExplorationPolicy explorationPolicy,
      double learningRate = 0.1,
      double discountFactor = 0.9,
      bool initializeRandom = false)
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

    /// <summary>
    ///   Perform learning step.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="action">Performed action.</param>
    /// <param name="reward">Gained reward.</param>
    /// <param name="nextState">Entered state after performing action.</param>
    public void Learn(int previousState, int action, double reward, int nextState)
    {
      var nextReward = _q[nextState][0];

      for (var i = 1; i < ActionCount; i++) {
        if (_q[nextState][i] > nextReward) {
          nextReward = _q[nextState][i];
        }
      }

      _q[previousState][action] = _q[previousState][action] * (1.0 - LearningRate) +
                                  LearningRate * (reward + DiscountFactor * nextReward);
    }

    /// <summary>
    ///   Select action in current state.
    /// </summary>
    /// <param name="state">Current state.</param>
    /// <returns>Selected action.</returns>
    public int SelectAction(int state)
    {
      return ExplorationPolicy.SelectAction(_q[state]);
    }
  }
}