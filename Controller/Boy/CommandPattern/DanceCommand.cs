
using UnityEngine;

public class DanceCommand : ICommand
{
    public const string danceParameterName = "isDancing";
    
    private Animator animator;

    public DanceCommand(Animator animator)
    {
        this.animator = animator;
    }

    public void Execute()
    {
        Dance();
    }

    private void Dance()
    {
        Debug.Log("DAAAAANCE!");
        this.animator.SetBool(danceParameterName, true);
    }
}
