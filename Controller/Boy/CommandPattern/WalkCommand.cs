using Camera.Player.CommandPattern;
using UnityEngine;

/**
 * WalkCommand wird ausgeführt wenn der Spieler die Tasten AWSD tätigt, dementsprechend bewegt sich der Spieler in die Richtung
 * in die die Kamera zeigt.
 */
public class WalkCommand : ICommand
{
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
        this.animator.SetBool(GlobalNamingHandler.WALK_PARAMETER_NAME, true); //Animation beim Gehen
        
        // Boxing Animation
        this.animator.SetBool(GlobalNamingHandler.ATTACK_PARAMETER_NAME, false);
        
        //Dance Animation
        this.animator.SetBool(GlobalNamingHandler.DANCE_PARAMETER_NAME, false);
        
        if (this.rigidbody.velocity.magnitude < this.maxAcceleration)
        {
            //this.rigidbody.AddForce(this.moveDirection, ForceMode.Impulse);
            this.rigidbody.velocity = this.moveDirection * maxAcceleration;
        }
    }
}
