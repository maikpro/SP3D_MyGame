using Camera.Player.CommandPattern;
using DefaultNamespace;
using DefaultNamespace.Controller.Enemies;
using UnityEngine;
using UnityEngine.AI;

/**
 * Der EnemyController ist für die Verwaltung der Gegner verantwortlich,
 * damit werden die nötigen Einstellungen konfiguriert und das passende Verhalten aufgerufen.
 */

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
            // State-Machine
            // Solange der Gegner nicht tot ist rufe sein Verhalten auf.
             this.state= this.enemy.Behaviour(); 
        }
        
        //Debug.Log(state.ToString());
        
        GroundChecker.IsGrounded(this.capsuleCollider);
        
        this.enemyRespawner.AfterFall(transform.position.y, this.enemyRespawner.StartPosition.y - 20);

        // ANIMATION FÜR ENEMY
        if (enemy.Life.IsDead)
        {
            state = State.DEAD;
        }
        
        // Wenn der Spieler in der Sichtweite ist (sightRange) verfolge ihn
        if (state == State.CHASING)
        {
            SoundManager.PlaySound(SoundManager.Sound.enemyHey, transform.position);
            this.animator.SetBool(GlobalNamingHandler.RUNNING_PARAMETER_NAME, true);
            this.animator.SetBool(GlobalNamingHandler.PATROLLING_PARAMETER_NAME, false);  
            this.animator.SetBool(GlobalNamingHandler.ATTACK_PARAMETER_NAME, false);
        }  
        // Wenn der Spieler in Angriffsnähe ist, greife ihn an
        else if (state == State.ATTACK)
        {
            this.animator.SetBool(GlobalNamingHandler.ATTACK_PARAMETER_NAME, true);
            this.animator.SetBool(GlobalNamingHandler.PATROLLING_PARAMETER_NAME, false); 
            this.animator.SetBool(GlobalNamingHandler.RUNNING_PARAMETER_NAME, false);
            this.attackCommand.Execute();
        }
        // Wenn der Spieler nicht in Angriffsnähe oder Sichtweite ist Patrolliere
        else if (state == State.PATROLLING)
        {
            this.animator.SetBool(GlobalNamingHandler.PATROLLING_PARAMETER_NAME, true); 
            this.animator.SetBool(GlobalNamingHandler.RUNNING_PARAMETER_NAME, false);
            this.animator.SetBool(GlobalNamingHandler.ATTACK_PARAMETER_NAME, false);
        }
        // Wenn der Spieler den Gegner geschlagen hat, stribt der Gegner
        else if (state == State.DEAD)
        {
            SoundManager.PlaySound(SoundManager.Sound.enemyUh, transform.position);
            this.animator.SetBool(GlobalNamingHandler.HIT_PARAMETER_NAME, true);
            Invoke("FallToGround", 1f);
            this.enemy.ClearNavMeshDestination();
            Destroy(gameObject,3f); // Zerstöre den Gegner nach 2 Sekunden
        }
    }

    // Set Collider to 0.1 so enemy, falls to the ground
    private void FallToGround()
    {
        // Fall to the ground
        this.capsuleCollider.height = 0.1f;

        this.enemyRigidbody.isKinematic = false;
    }
    
    // Fürs Debugging der Spheren Angriffs- und Sichtweite anzeigen.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, this.sightRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, this.attackRange);
    }
}
