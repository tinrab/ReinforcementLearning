using System;

namespace ReinforcementLearning
{
  public class EpsilonGreedyExplorationPolicy : IExplorationPolicy
  {
    private readonly Random _random;
    public double Epsilon { get; set; }

    public EpsilonGreedyExplorationPolicy(double epsilon = 0.1)
    {
      Epsilon = epsilon;
      _random = new Random();
    }

    public int SelectAction(double[] estimates)
    {
      var maxReward = estimates[0];
      var action = 0;

      for (var i = 1; i < estimates.Length; i++) {
        if (estimates[i] > maxReward) {
          maxReward = estimates[i];
          action = i;
        }
      }

      if (_random.NextDouble() < Epsilon) {
        var a = _random.Next(estimates.Length - 1);

        if (a >= action) {
          a++;
        }

        return a;
      }

      return action;
    }
  }
}