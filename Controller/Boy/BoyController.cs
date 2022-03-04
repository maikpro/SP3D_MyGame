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

    [Header("Damage")] 
    [SerializeField] 
    private Material boySkin;
    [SerializeField]
    private Material damageMaterial;
    [SerializeField]
    private Material defaultMaterial;
    
    // Privat
    private Animator animator;
    private Vector3 moveDirection;
    private Rigidbody boyRigidbody;
    private CapsuleCollider capsuleCollider;
    private Player player;
    private bool grounded;
    private float colliderHeight; 
    
    // Damage 
    private bool isHit;
    private Renderer skinnedMeshRenderer;
   
    
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

    public bool IsHit
    {
        get => isHit;
        set => isHit = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.boyRigidbody = GetComponent<Rigidbody>();

        this.capsuleCollider = GetComponent<CapsuleCollider>();
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        this.player = this.gameLogic.Player;

        this.colliderHeight = this.capsuleCollider.height;
    }

    // Update is called once per frame
    void Update()
    {
        this.grounded = GroundChecker.IsGrounded(this.capsuleCollider);
        Animation();
        
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
        // Input.GetKey(KeyCode.Mouse1)
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

    void Animation()
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

        if (this.isHit)
        {
            // Player Hit by enemy
            if (!this.player.HasShield)
            {
                this.animator.SetBool("isHit", true);
                Invoke("FallToGround", 1f);
                this.boyRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            
            Debug.Log("isHit!");
            this.gameLogic.PlayerHitByEnemy();
        }
        else
        {
            this.animator.SetBool("isHit", false);
            this.boySkin.color = defaultMaterial.color;
        }
        
        if (this.gameLogic.IsInvoking("Respawn"))
        {
            this.boySkin.color = damageMaterial.color;
            this.capsuleCollider.height = this.colliderHeight;
        }
    }
    
    // Set Collider to 0.1 so enemy falls to the ground
    private void FallToGround()
    {
        // Fall to the ground
        this.capsuleCollider.height = 0.1f;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.attackPoint.position, this.attackRange);
    }
}