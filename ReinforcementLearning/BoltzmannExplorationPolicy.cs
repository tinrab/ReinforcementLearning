using System;

namespace ReinforcementLearning
{
  public class BoltzmannExplorationPolicy : IExplorationPolicy
  {
    private Random _random;
    public double Temperature { get; set; }

    public BoltzmannExplorationPolicy(double temperature = 1.0)
    {
      Temperature = temperature;
    }

    public int SelectAction(double[] estimates)
    {
      var probabilities = new double[estimates.Length];
      var probabilitiesSum = 0.0;

      for (var i = 0; i < estimates.Length; i++) {
        var p = Math.Exp(estimates[i] / Temperature);
        probabilities[i] = p;
        probabilitiesSum += p;
      }

      if (double.IsInfinity(probabilitiesSum) || probabilitiesSum == 0.0) {
        var maxReward = estimates[0];
        var action = 0;

        for (var i = 1; i < estimates.Length; i++) {
          if (estimates[i] > maxReward) {
            maxReward = estimates[i];
            action = i;
          }
        }

        return action;
      }

      var r = _random.NextDouble();
      var sum = 0.0;

      for (var i = 0; i < estimates.Length; i++) {
        sum += probabilities[i] / sum;
        if (r <= sum) {
          return i;
        }
      }

      return estimates.Length - 1;
    }
  }
}