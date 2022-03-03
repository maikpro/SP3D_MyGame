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

    [Space] [Header("Attack")] 
    [SerializeField]
    private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask attackLayers;
    
    // Privat
    private Animator animator;
    private Vector3 moveDirection;
    private Rigidbody boyRigidbody;
    private CapsuleCollider capsuleCollider;
    private Player player;
    private bool grounded;
    
    // Steuerung
    private float xDirection;
    private float yDirection;

    // Camera
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    // Command Pattern
    private ICommand command;

    public Player Player
    {
        get => player;
        set => player = value;
    }

    public Rigidbody BoyRigidbody
    {
        get => boyRigidbody; // Für Knockback beim Enemy Hit
    }

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.boyRigidbody = GetComponent<Rigidbody>();

        this.capsuleCollider = GetComponent<CapsuleCollider>();
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        this.player = this.gameLogic.Player;
    }

    // Update is called once per frame
    void Update()
    {
        this.grounded = GroundChecker.IsGrounded(this.capsuleCollider);
        GroundedAnimation();
        
        CameraPosition();
        InputHandler();

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
        if (Input.GetKeyDown(KeyCode.Space) && this.grounded)
        {
            this.command = new JumpCommand(this.boyRigidbody, this.animator, this.JumpSpeed);
        }
        
        // BOXING / FIGHTING
        // Input.GetKey(KeyCode.Q
        if (Input.GetKey(KeyCode.Mouse1))
        {
            this.command = new AttackCommand(this.animator, this.attackPoint, this.attackRange, this.attackLayers);
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

    void GroundedAnimation()
    {
        if (this.grounded)
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
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.attackPoint.position, this.attackRange);
    }
}