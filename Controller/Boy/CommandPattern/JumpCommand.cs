using UnityEngine;

public class JumpCommand : ICommand
{
    public const string jumpParameterName = "isJumping";

    private CharacterController characterController;
    private Animator animator;
    private float jumpSpeed;
    private Vector3 velocity;

    public JumpCommand(CharacterController characterController, Animator animator, float jumpSpeed, Vector3 velocity)
    {
        this.characterController = characterController;
        this.animator = animator;
        this.jumpSpeed = jumpSpeed;
        this.velocity = velocity;
    }

    public void Execute()
    {
        Jump();
    }

    private void Jump()
    {
        Debug.Log("JUMP");
        this.velocity.y = jumpSpeed;
        this.animator.SetBool(jumpParameterName, true);
        this.characterController.Move(this.velocity * Time.deltaTime);
    }
}
