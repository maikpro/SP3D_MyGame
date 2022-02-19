using UnityEngine;

namespace Camera.Player.CommandPattern
{
    public class AttackCommand : ICommand
    {
        public const string attackParameterName = "isAttacking";

        private Animator animator;
        private float strength;

        public AttackCommand(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute()
        {
            Attacking();
        }

        private void Attacking()
        {
            Debug.Log("Attack!");
            this.animator.SetBool(attackParameterName, true);
        }
    }
}