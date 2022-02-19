using System;
using Camera.Player.CommandPattern;
using DefaultNamespace;
using UnityEngine;

public class BoyController : MonoBehaviour
{
    // Zuweisung per Unity Editor
    public Transform Camera;
    public float Acceleration;
    public float RotateSpeed;
    public float JumpSpeed;

    // Privat
    private Animator animator;
    private Vector3 moveDirection;
    private CharacterController characterController;
    private float yGravity;

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
        this.characterController = GetComponent<CharacterController>();
        this.boyRespawner = new Respawn(gameObject, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        this.isGrounded = this.characterController.isGrounded;
        
        CameraPosition();
        InputHandler();
        UpdateGrounded();
        
        this.boyRespawner.afterFall(transform.position.y, -20);

        // Geh-Richtung
        this.moveDirection = (this.cameraForward * this.yDirection + this.cameraRight * this.xDirection);
        float magnitude = Mathf.Clamp01(this.moveDirection.magnitude) * this.Acceleration;
        this.moveDirection.Normalize();

        // Schwerkraft
        this.yGravity += Physics.gravity.y * Time.deltaTime; 
        Vector3 velocity = this.moveDirection * magnitude;
        velocity.y = this.yGravity;

        // GEHEN
        if (moveDirection != Vector3.zero)
        {
            this.command = new WalkCommand(this.moveDirection, this.characterController, this.transform, this.animator, velocity, this.RotateSpeed);
        }  else
        {
            this.command = new IdleCommand(this.animator); 
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && this.isGrounded)
        {
            Debug.Log("dewedgwe");
            this.command = new JumpCommand(this.characterController, this.animator, this.JumpSpeed, velocity);
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

    private void UpdateGrounded()
    {
        this.isGrounded = this.characterController.isGrounded;

       // Debug.Log(this.isGrounded);
        
        if (this.isGrounded)
        {
            this.animator.SetBool("isJumping", false);
            this.animator.SetBool("isFalling", false);
            this.animator.SetBool("Grounded", true);
        }
        else
        {
            this.animator.SetBool("isFalling", true);
            this.animator.SetBool("Grounded", false);
        }
    }
}
