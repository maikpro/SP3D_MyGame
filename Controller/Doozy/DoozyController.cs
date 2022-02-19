using System;
using Camera.Player.CommandPattern;
using DefaultNamespace;
using UnityEngine;

public class DoozyController : MonoBehaviour
{
    // Zuweisung per Unity Editor
    public float MaxAcceleration;
    [SerializeField]
    public float RotateSpeed;

    // Privat
    private Animator animator;
    private Vector3 moveDirection;
    private Rigidbody doozyRigidbody;

    // Steuerung
    private float xDirection;
    private float yDirection;

    // Command Pattern
    private ICommand command;
    
    // Respawn 
    private Respawn doozyRespawner;
    
    // Is PLayer Grounded?
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.doozyRespawner = new Respawn(gameObject, transform.position);
        this.doozyRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Zum Testen
        InputHandler();

        this.doozyRespawner.afterFall(transform.position.y, -20);

        // Geh-Richtung
        this.moveDirection = (new Vector3(this.xDirection,  0, this.yDirection));
        
        // GEHEN
        if (moveDirection != Vector3.zero)
        {
            this.command = new WalkCommand(this.moveDirection, this.doozyRigidbody, this.transform, this.animator, this.RotateSpeed, this.MaxAcceleration);
        }  else
        {
            this.command = new IdleCommand(this.doozyRigidbody, this.animator); 
        }

        // BOXING / FIGHTING
        if (Input.GetMouseButton(0))
        {
            this.command = new AttackCommand(this.animator);
        }
        
        this.command.Execute();
    }

    void InputHandler()
    {
        // Keyboard-Eingabe A W S D
        this.xDirection = Input.GetAxis("Horizontal");
        this.yDirection = Input.GetAxis("Vertical");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            this.isGrounded = true;
            this.animator.SetBool("isJumping", false);
            this.animator.SetBool("isFalling", false);
            this.animator.SetBool("Grounded", true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            this.isGrounded = false;
            this.animator.SetBool("isFalling", true);
            this.animator.SetBool("Grounded", false);
        }
    }
}
