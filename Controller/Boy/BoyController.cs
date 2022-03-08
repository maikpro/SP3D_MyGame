using Camera.Player.CommandPattern;
using UnityEngine;

/**
 * BoyController ist für die Steuerung des Spielers verantwortlich von hier aus werden die passenden Animationen aufgerufen 
 */

public class BoyController : MonoBehaviour
{
    private GameLogic gameLogic;

    // Zuweisung per Unity Editor
    public Transform Camera;
    
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
    [Range(0,10)]
    [SerializeField] private float attackCooldown;

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
    private bool grounded;
    private float colliderHeight;
    private bool isPlayerDancing;
    
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
        this.gameLogic = GameObject.Find(GlobalNamingHandler.GAMEOBJECT_GAMELOGIC).GetComponent<GameLogic>();

        this.colliderHeight = this.capsuleCollider.height;

        this.isPlayerDancing = false;
        
        DontDestroyOnLoad(gameObject);
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
        if (moveDirection != Vector3.zero && !this.gameLogic.Player.Life.CheckIfDead())
        {
            this.command = new WalkCommand(this.moveDirection, this.boyRigidbody, this.transform, this.animator, this.RotateSpeed, this.MaxAcceleration);
        }  else if(!isPlayerDancing)
        {
            this.command = new IdleCommand(this.boyRigidbody, this.animator);
            this.gameLogic.Player.IsAttacking = false;
        }
    
        // JUMP
        if (Input.GetKeyDown(KeyCode.Space) && this.grounded)
        {
            this.command = new JumpCommand(this.boyRigidbody, this.animator, this.JumpSpeed);
        }
    
        // Dance
        if (this.isPlayerDancing || Input.GetKey(KeyCode.O))
        {
            this.isPlayerDancing = true;
            this.command = new DanceCommand(this.animator);
            Invoke("PlayerStopDance", 5f);
        }
        
        if (Input.GetMouseButton(1))
        {
            this.command = new AttackCommand(this.animator, this.attackPoint, this.attackRange, this.attackLayers);
            this.gameLogic.Player.IsAttacking = true;
            GameObject soundGameObject = SoundManager.PlaySound(SoundManager.Sound.boyAttack, this.attackPoint.position);
            Destroy(soundGameObject, 1f);
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
            this.animator.SetBool(GlobalNamingHandler.JUMP_PARAMETER_NAME, false);
            this.animator.SetBool(GlobalNamingHandler.FALL_PARAMETER_NAME, false);
            this.animator.SetBool(GlobalNamingHandler.GROUNDED_PARAMETER_NAME, true);
        }
        else 
        {
            this.animator.SetBool(GlobalNamingHandler.FALL_PARAMETER_NAME, true);
            this.animator.SetBool(GlobalNamingHandler.GROUNDED_PARAMETER_NAME, false);
        }

        if (this.isHit && !this.gameLogic.Player.Life.IsDead)
        {
            // Player Hit by enemy
            if (!this.gameLogic.Player.HasShield)
            {
                this.animator.SetBool(GlobalNamingHandler.HIT_PARAMETER_NAME, true);
                Invoke("FallToGround", 1f);
            }
            this.gameLogic.PlayerHit();
        }
        else if(!this.gameLogic.Player.Life.CheckIfDead())
        {
            this.animator.SetBool(GlobalNamingHandler.HIT_PARAMETER_NAME, false);
            this.boySkin.color = defaultMaterial.color;
        }
        
        if (this.gameLogic.IsInvoking("Respawn"))
        {
            this.boySkin.color = damageMaterial.color;
            this.capsuleCollider.height = this.colliderHeight;
        }
        
        if (this.gameLogic.Player.Life.CheckIfDead())
        {
            this.animator.SetBool(GlobalNamingHandler.HIT_PARAMETER_NAME, true);
        }
    }
    
    // Set Collider to 0.1 so enemy falls to the ground
    private void FallToGround()
    {
        // Fall to the ground
        this.capsuleCollider.height = 0.1f;
    }

    public void PlayerDance()
    {
        this.isPlayerDancing = true;
        Invoke("PlayerStopDance", 4f);
    }

    public void PlayerStopDance()
    {
        this.isPlayerDancing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.attackPoint.position, this.attackRange);
    }
}