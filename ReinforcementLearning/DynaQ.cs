using System;
using System.Collections.Generic;
using System.Linq;

namespace ReinforcementLearning
{
  public class DynaQ : IReinforcementLearning
  {
    private readonly int[][] _finalStates;
    private readonly double[][] _q;
    private readonly Random _random;
    private readonly double[][] _rewards;
    private readonly Dictionary<int, HashSet<int>> _visited;

    public int N { get; }
    public double LearningRate { get; set; }
    public double DiscountFactor { get; set; }
    public IExplorationPolicy ExplorationPolicy { get; set; }

    public DynaQ(int stateCount, int actionCount, IExplorationPolicy explorationPolicy, int n = 5,
      double learningRate = 0.1, double discountFactor = 0.9, bool initializeRandom = false)
    {
      StateCount = stateCount;
      ActionCount = actionCount;
      N = n;
      ExplorationPolicy = explorationPolicy;
      LearningRate = learningRate;
      DiscountFactor = discountFactor;

      _visited = new Dictionary<int, HashSet<int>>();
      _random = new Random();
      _q = new double[stateCount][];
      _finalStates = new int[stateCount][];
      _rewards = new double[stateCount][];
      for (var i = 0; i < stateCount; i++) {
        _q[i] = new double[actionCount];
        _finalStates[i] = new int[actionCount];
        _rewards[i] = new double[actionCount];
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

    public int CurrentState { get; private set; }
    public int SelectedAction { get; private set; }
    public int StateCount { get; }
    public int ActionCount { get; }

    public void Begin(int state)
    {
      CurrentState = state;
      SelectedAction = ExplorationPolicy.SelectAction(_q[CurrentState]);
    }

    public void Step(double reward, int nextState)
    {
      if (!_visited.ContainsKey(CurrentState)) {
        var actions = new HashSet<int>();
        actions.Add(SelectedAction);
        _visited[CurrentState] = actions;
      }

      UpdateQ(reward, nextState);
      Plan();

      CurrentState = nextState;
      SelectedAction = ExplorationPolicy.SelectAction(_q[CurrentState]);
    }

    private void UpdateQ(double reward, int nextState)
    {
      var bestNext = _q[nextState][0];

      for (var i = 1; i < ActionCount; i++) {
        if (_q[nextState][i] > bestNext) {
          bestNext = _q[nextState][i];
        }
      }

      var target = reward + DiscountFactor * bestNext;
      var delta = target - _q[CurrentState][SelectedAction];
      _q[CurrentState][SelectedAction] += LearningRate * delta;

      _finalStates[CurrentState][SelectedAction] = nextState;
      _rewards[CurrentState][SelectedAction] = reward;
    }

    private void Plan()
    {
      for (var i = 0; i < N; i++) {
        var nextState = _visited.Keys.ToArray()[_random.Next(0, _visited.Keys.Count)];

        if (_visited.ContainsKey(nextState)) {
          var nextAction = _visited[nextState].ToArray()[_random.Next(0, _visited[nextState].Count)];
          var finalState = _finalStates[nextState][nextAction];
          var reward = _rewards[nextState][nextAction];

          UpdateQ(reward, finalState);
        }
      }
    }
  }
}