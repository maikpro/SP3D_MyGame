using UnityEngine;

public class WalkCommand : ICommand
{
    public const string walkParameterName = "isWalking";
    
    private Vector3 moveDirection;
    private CharacterController characterController;
    private Transform transform;
    private Animator animator;
    private Vector3 velocity;
    private float rotateSpeed;

    public WalkCommand(Vector3 moveDirection, CharacterController characterController, Transform transform, Animator animator, Vector3 velocity, float rotateSpeed)
    {
        this.moveDirection = moveDirection;
        this.characterController = characterController;
        this.transform = transform;
        this.animator = animator;
        this.velocity = velocity;
        this.rotateSpeed = rotateSpeed;
    }

    public void Execute()
    {
        Walk();
    }

    private void Walk()
    {
        // Wenn sich der Character bewegt, soll Charakter in Richtung der Bewegung schauen
        Quaternion toRotation = Quaternion.LookRotation(this.moveDirection, Vector3.up); //Richtung in die Character schaut
                                                                              
        // RotateTowards von A nach B und Winkel (Rotationsgeschwindigkeit!)
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, toRotation, this.rotateSpeed * Time.deltaTime);
        animator.SetBool(walkParameterName, true); //Animation beim Gehen
        
        
        this.characterController.Move(this.velocity * Time.deltaTime);
    }
}
