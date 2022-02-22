using System;
using UnityEngine;

namespace Camera.Player.CommandPattern
{
    public class AttackCommand : ICommand
    {
        public const string attackParameterName = "isAttacking";

        private Animator animator;

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
            this.animator.SetBool(attackParameterName, true);
            
        }
    }
}