using UnityEngine;

namespace Camera.Player.CommandPattern
{
    public class BoxingCommand : ICommand
    {
        public const string boxingParameterName = "isBoxing";

        private Animator animator;
        private float strength;

        public BoxingCommand(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute()
        {
            Boxing();
        }

        private void Boxing()
        {
            Debug.Log("BOX BOX BOX!");
            this.animator.SetBool(boxingParameterName, true);
        }
    }
}