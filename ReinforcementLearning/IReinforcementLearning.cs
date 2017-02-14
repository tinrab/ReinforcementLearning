namespace ReinforcementLearning
{
  public interface IReinforcementLearning
  {
    int StateCount { get; }
    int ActionCount { get; }

    /// <summary>
    ///   Perform learning step.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="action">Performed action.</param>
    /// <param name="reward">Gained reward.</param>
    /// <param name="nextState">Entered state after performing action.</param>
    void Learn(int previousState, int action, double reward, int nextState);

    /// <summary>
    ///   Select action in current state.
    /// </summary>
    /// <param name="state">Current state.</param>
    /// <returns>Selected action.</returns>
    int SelectAction(int state);
  }
}