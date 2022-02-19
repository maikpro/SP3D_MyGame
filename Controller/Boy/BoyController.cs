using System;
using Camera.Player.CommandPattern;
using DefaultNamespace;
using UnityEngine;

public class BoyController : MonoBehaviour
{
    // Zuweisung per Unity Editor
    [SerializeField]
    private Transform Camera;
    [Space]
    [SerializeField]
    public float MaxAcceleration;
    [SerializeField]
    public float RotateSpeed;
    [SerializeField]
    [Space]
    public float JumpSpeed;

    // Privat
    private Animator animator;
    private Vector3 moveDirection;
    private Rigidbody boyRigidbody;

    // Steuerung
    private float xDirection;
    private float yDirection;

    // Camera
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    // Command Pattern
    private ICommand command;
    
    // Respawn 
    private Respawn boyRespawner;
    
    // Is PLayer Grounded?
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.boyRespawner = new Respawn(gameObject, transform.position);
        this.boyRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraPosition();
        InputHandler();

        this.boyRespawner.afterFall(transform.position.y, -20);

        // Geh-Richtung
        this.moveDirection = (this.cameraForward * this.yDirection + this.cameraRight * this.xDirection);
        
        // GEHEN
        if (moveDirection != Vector3.zero)
        {
            this.command = new WalkCommand(this.moveDirection, this.boyRigidbody, this.transform, this.animator, this.RotateSpeed, this.MaxAcceleration);
        }  else
        {
            this.command = new IdleCommand(this.boyRigidbody, this.animator); 
        }
        
        // JUMP
        if (Input.GetKeyDown(KeyCode.Space) && this.isGrounded)
        {
            this.command = new JumpCommand(this.boyRigidbody, this.animator, this.JumpSpeed);
        }
        
        // BOXING / FIGHTING
        if (Input.GetMouseButton(0))
        {
            this.command = new BoxingCommand(this.animator);
        }
        
        this.command.Execute();
    }

    void InputHandler()
    {
        // Keyboard-Eingabe A W S D
        this.xDirection = Input.GetAxis("Horizontal");
        this.yDirection = Input.GetAxis("Vertical");
    }

    void CameraPosition()
    {
        // Im Kamera-Raum bewegen
        this.cameraForward = Camera.forward;
        this.cameraRight = Camera.right;

        // Y-Werte sollen unberührt bleiben! Character soll sich nur in X/Z-Ebene bewegen.
        this.cameraForward.y = 0;
        this.cameraRight.y = 0;

        this.cameraForward = this.cameraForward.normalized;
        this.cameraRight = this.cameraRight.normalized;
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
