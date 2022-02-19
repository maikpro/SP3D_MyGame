using UnityEngine;

namespace Camera.Player.CommandPattern
{
    public class IdleCommand : ICommand
    {
        private Animator animator;

        public IdleCommand(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute()
        {
            Idle();
        }

        private void Idle()
        {
            animator.SetBool(WalkCommand.walkParameterName, false);
        }
    }
}