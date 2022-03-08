using UnityEngine;

namespace Camera.Player.CommandPattern
{
    /**
     * Idle wird aufgerufen, wenn der Spieler keine anderen Commandos eingibt, wie Laufen oder Springen
     */
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
            
            // Animationen
            this.animator.SetBool(GlobalNamingHandler.WALK_PARAMETER_NAME, false);
            this.animator.SetBool(GlobalNamingHandler.ATTACK_PARAMETER_NAME, false);
            this.animator.SetBool(GlobalNamingHandler.DANCE_PARAMETER_NAME, false);
        }
    }
}