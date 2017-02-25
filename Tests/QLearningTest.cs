using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning;

namespace Tests
{
  [TestClass]
  public class QLearningTest
  {
    [TestMethod]
    public void QLearningGridPathTest()
    {
      var random = new Random(1337);
      var gridSize = 16;
      var grid = new int[gridSize, gridSize];
      var ql = new QLearning(gridSize * gridSize, 4, new EpsilonGreedyExplorationPolicy(0.0), 0.3, 0.8);
      var pathLength = 20;
      var maxReward = 0.0;

      for (var x = 0; x < gridSize; x++) {
        for (var y = 0; y < gridSize; y++) {
          grid[x, y] = random.Next(0, 100);
        }
      }

      // train
      for (var i = 0; i < 100; i++) {
        var x = random.Next(0, gridSize);
        var y = random.Next(0, gridSize);
        var currentReward = 0.0;

        ql.Begin(x + y * gridSize);

        for (var j = 0; j < pathLength; j++) {
          switch ((Action)ql.SelectedAction) {
            case Action.UP:
              y++;
              break;
            case Action.DOWN:
              y--;
              break;
            case Action.LEFT:
              x--;
              break;
            case Action.RIGHT:
              x++;
              break;
          }

          var r = 0;
          if (x < 0 || x >= gridSize || y < 0 || y >= gridSize) {
            x = x < 0 ? 0 : x >= gridSize ? gridSize - 1 : x;
            y = y < 0 ? 0 : y >= gridSize ? gridSize - 1 : y;
          } else {
            r = grid[x, y];
          }

          currentReward += r;

          var nextState = x + y * gridSize;
          ql.Step(r, nextState);
        }

        if (currentReward > maxReward) {
          maxReward = currentReward;
        }
      }

      Assert.AreEqual(896, maxReward);
    }

    private enum Action
    {
      UP = 0,
      DOWN = 1,
      LEFT = 2,
      RIGHT = 3
    }
  }
}