using Camera.Player.CommandPattern;
using DefaultNamespace;
using DefaultNamespace.Controller.Enemies;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Privat
    private Animator animator;
    private Vector3 moveDirection;
    private CapsuleCollider capsuleCollider;
    private State state;
    private Enemy enemy;
    private AttackCommand attackCommand;

    // Zuweisung per Unity Editor
    [Header("Enemy Lifes")]
    [SerializeField]
    [Range(1,3)]
    private int enemyLifeCounter;
    
    [Header("Enemy Behaviour")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float walkPointRange;

    [Header("Attack Player")]
    [SerializeField]
    private Transform attackPoint;
    [SerializeField] private LayerMask attackLayers;
    
    // Respawn falls Enemy fällt
    private Respawn enemyRespawner;
    private Rigidbody enemyRigidbody;
    
    // Is PLayer Grounded?
    private bool grounded;

    public Enemy Enemy
    {
        get => enemy;
    }

    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.enemyRespawner = new Respawn(gameObject, transform.position);
        this.capsuleCollider = GetComponent<CapsuleCollider>();
        this.enemyRigidbody = GetComponent<Rigidbody>();
        
        // Enemy 
        this.enemy = new Enemy(new Life(this.enemyLifeCounter, false), GetComponent<NavMeshAgent>(), transform, this.sightRange, this.attackRange, this.walkPointRange);
        this.attackCommand = new AttackCommand(this.attackPoint, this.attackRange, this.attackLayers, true);
    }
    
    void Update()
    {
        if (!enemy.Life.IsDead)
        {
             this.state= this.enemy.Behaviour(); 
        }
        
        //Debug.Log(state.ToString());
        
        GroundChecker.IsGrounded(this.capsuleCollider);
        
        this.enemyRespawner.afterFall(transform.position.y, -20);

        // ANIMATION FÜR ENEMY
        if (enemy.Life.IsDead)
        {
            state = State.DEAD;
        }
        
        if (state == State.CHASING)
        {
            this.animator.SetBool("isRunning", true);
            this.animator.SetBool("isPatrolling", false);  
            this.animator.SetBool("isAttacking", false);
        }  
        else if (state == State.ATTACK)
        {
            this.animator.SetBool("isAttacking", true);
            this.animator.SetBool("isPatrolling", false); 
            this.animator.SetBool("isRunning", false);
            this.attackCommand.Execute();
        }
        else if (state == State.PATROLLING)
        {
            this.animator.SetBool("isPatrolling", true); 
            this.animator.SetBool("isRunning", false);
            this.animator.SetBool("isAttacking", false);
        }
        else if (state == State.DEAD)
        {
            this.animator.SetBool("isHit", true);
            Invoke("FallToGround", 1f);
            this.enemy.ClearNavMeshDestination();
            Destroy(gameObject,3f); // Zerstöre den Gegner nach 2 Sekunden
        }
    }

    // Set Collider to 0.1 so enemy falls to the ground
    private void FallToGround()
    {
        // Fall to the ground
        this.capsuleCollider.height = 0.1f;

        this.enemyRigidbody.isKinematic = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, this.sightRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, this.attackRange);
    }
}
