using Camera.Player.CommandPattern;
using UnityEngine;

public class WalkCommand : ICommand
{
    public const string walkParameterName = "isRunning";
    
    private Vector3 moveDirection;
    private Rigidbody rigidbody;
    private Transform transform;
    private Animator animator;
    private float rotateSpeed;
    private float maxAcceleration;

    public WalkCommand(Vector3 moveDirection, Rigidbody rigidbody, Transform transform, Animator animator, float rotateSpeed, float maxAcceleration)
    {
        this.moveDirection = moveDirection;
        this.rigidbody = rigidbody;
        this.transform = transform;
        this.animator = animator;
        this.rotateSpeed = rotateSpeed;
        this.maxAcceleration = maxAcceleration;
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
        
        // Walk Animation
        this.animator.SetBool(walkParameterName, true); //Animation beim Gehen
        
        // Boxing Animation
        this.animator.SetBool(AttackCommand.attackParameterName, false);
        
        if (this.rigidbody.velocity.magnitude < this.maxAcceleration)
        {
            //this.rigidbody.AddForce(this.moveDirection, ForceMode.Impulse);
            this.rigidbody.velocity = this.moveDirection * maxAcceleration;
        }
    }
}
