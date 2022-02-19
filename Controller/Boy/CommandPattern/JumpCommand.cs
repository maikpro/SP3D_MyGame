using UnityEngine;

public class JumpCommand : ICommand
{
    public const string jumpParameterName = "isJumping";

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
        this.animator.SetBool(jumpParameterName, true);
    }
}
