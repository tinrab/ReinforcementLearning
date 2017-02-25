namespace ReinforcementLearning
{
  public interface IReinforcementLearning
  {
    int StateCount { get; }
    int ActionCount { get; }
    int CurrentState { get; }
    int SelectedAction { get; }

    /// <summary>
    /// Begin learning process.
    /// </summary>
    /// <param name="state">Initial state.</param>
    void Begin(int state);

    /// <summary>
    ///   Perform learning step.
    /// </summary>
    /// <param name="reward">Gained reward.</param>
    /// <param name="nextState">Entered state after performing action.</param>
    void Step(double reward, int nextState);
  }
}