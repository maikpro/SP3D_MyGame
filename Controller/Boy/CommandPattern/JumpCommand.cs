using UnityEngine;

/**
 * JumpCommand wird ausgeführt wenn der Spieler die Leertaste tätigt
 */
public class JumpCommand : ICommand
{
    private Rigidbody rigidbody;
    private Animator animator;
    private float jumpSpeed;

    public JumpCommand(Rigidbody rigidbody, Animator animator, float jumpSpeed)
    {
        this.rigidbody = rigidbody;
        this.animator = animator;
        this.jumpSpeed = jumpSpeed;
    }

    public void Execute()
    {
        Jump();
    }

    private void Jump()
    {
        this.rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        this.animator.SetBool(GlobalNamingHandler.JUMP_PARAMETER_NAME, true);
        
        // Boxing Animation
        this.animator.SetBool(GlobalNamingHandler.ATTACK_PARAMETER_NAME, false);
    }
}
