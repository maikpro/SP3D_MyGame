using UnityEngine;

namespace Camera.Player.CommandPattern
{
    public class IdleCommand : ICommand
    {
        private Rigidbody rigidbody;
        private Animator animator;

        public IdleCommand(Rigidbody rigidbody, Animator animator)
        {
            this.rigidbody = rigidbody;
            this.animator = animator;
        }

        public void Execute()
        {
            Idle();
        }

        private void Idle()
        {
            this.rigidbody.AddForce(Vector3.zero, ForceMode.Acceleration);
            this.animator.SetBool(WalkCommand.walkParameterName, false);
            this.animator.SetBool(BoxingCommand.boxingParameterName, false);
        }
    }
}