
public class StateMachine
{
    public State CurrentStae;
   
    public void Initialize(State startingState)
    {
        CurrentStae = startingState;
        startingState.Enter();
    }
    public void ChangeState(State newState)
    {
        CurrentStae.Exit();

        CurrentStae = newState;
        newState.Enter();
    }
}
