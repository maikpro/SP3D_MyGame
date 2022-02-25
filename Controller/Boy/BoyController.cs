using System;
using Camera.Player.CommandPattern;
using DefaultNamespace.Controller.Boy;
using UnityEngine;

public class BoyController : MonoBehaviour
{
    private GameLogic gameLogic;
    
    //EVENTS
    public event Action<bool, int> OnActionEvent;
    
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

    private Player player;

    public string text;


    // Steuerung
    private float xDirection;
    private float yDirection;

    // Camera
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    // Command Pattern
    private ICommand command;
    
    // Respawn 
    //private Respawn boyRespawner;
    
    // Is PLayer Grounded?
    private bool isGrounded;

    public Player Player
    {
        get => player;
        set => player = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        //this.boyRespawner = new Respawn(gameObject, transform.position);
        this.boyRigidbody = GetComponent<Rigidbody>();
        //this.player = new Player(new Life(5, false), false);
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        this.player = this.gameLogic.Player;
        
        this.text = "Hello from BoyController: Player Lives: " + this.player.Life.Counter;
    }

    // Update is called once per frame
    void Update()
    {
        //OnActionEvent?.Invoke(true, 213);
        CameraPosition();
        InputHandler();

        //this.boyRespawner.afterFall(transform.position.y, -20);

        // Geh-Richtung
        this.moveDirection = (this.cameraForward * this.yDirection + this.cameraRight * this.xDirection);
        
        // GEHEN
        if (moveDirection != Vector3.zero)
        {
            this.command = new WalkCommand(this.moveDirection, this.boyRigidbody, this.transform, this.animator, this.RotateSpeed, this.MaxAcceleration);
        }  else
        {
            this.command = new IdleCommand(this.boyRigidbody, this.animator);
            this.player.IsAttacking = false;
        }
        
        // JUMP
        if (Input.GetKeyDown(KeyCode.Space) && this.isGrounded)
        {
            this.command = new JumpCommand(this.boyRigidbody, this.animator, this.JumpSpeed);
        }
        
        // BOXING / FIGHTING
        if (Input.GetKey(KeyCode.Q))
        {
            this.command = new AttackCommand(this.animator);
            this.player.IsAttacking = true;
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
        Debug.Log(gameObject.name + " collided with " + other.gameObject.name);
        
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
