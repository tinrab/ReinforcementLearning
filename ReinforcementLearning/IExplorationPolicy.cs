namespace ReinforcementLearning
{
  public interface IExplorationPolicy
  {
    int SelectAction(double[] estimates);
  }
}
