using System;
using DefaultNamespace;
using DefaultNamespace.Controller.Boy;
using UnityEngine;
using Object = System.Object;

public class GameLogic : MonoBehaviour
{
    // EDITOR VARIABLES
    [Header("Player Settings")]
    [SerializeField]
    [Range(1,99)]
    private int playerLifeCounter;

    [SerializeField] 
    [Range(1,99)]
    private int gemsCollected;

    [SerializeField] 
    private bool playerHasShield;
    
    //PRIVATE FIELDS
    private BoyController boyController;
    private Respawn boyRespawner;
    private Player player;
    
    // Objects
    private DestructableObject[] checkPoints;
    
    // EVENTS
    public event Action OnLifeUpdate; // Action for Respawn after Falling 
    public event Action OnCollect; // Action for Collection of Gems

    public event Action OnChangeShieldStatus; // Action for Collection of Shields // Only 1 Shield possible cause of bool usage!!!
    
    public Player Player
    {
        get => player;
        set => player = value;
    }

    public int GemsCollected
    {
        get => gemsCollected;
        set
        {
            gemsCollected = value;
            
            // Collected Gem
            OnCollect?.Invoke();
            
            // If Player has collected 100 Gems give him an extra life and reset gemscounter = 0
            if (this.GemsCollected >= 100)
            {
                BonusLife();
            }
        }
    }

    void Awake()
    {
        this.boyController = GameObject.Find("Boy").GetComponent<BoyController>();
        this.player = new Player(new Life(this.playerLifeCounter, false), this.playerHasShield, false);
        this.boyRespawner = new Respawn(this.boyController.gameObject, transform.position);
        this.boyRespawner.SetStartPosition(new Vector3(219.809998f,0f,2.41199994f));
    }

    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        //this.destructableObject = GameObject.Find("DestructableObject").GetComponent<DestructableObject>();
        this.checkPoints = FindObjectsOfType<DestructableObject>();
        
        
        // Subscribe to Event OnCheckPointReached from DestructableObject
        //this.destructableObject.OnCheckPointReached += DestructableObjectOnCheckPointReached;
        foreach (var checkpoint in this.checkPoints)
        {
            checkpoint.OnCheckPointReached += DestructableObjectOnCheckPointReached;
        }
    }

    private void DestructableObjectOnCheckPointReached()
    {
        // Set new StartPoint aka Checkpoint!
        this.boyRespawner.SetStartPosition(this.boyController.gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Respawn after Falling down
        if (this.boyRespawner.afterFall(this.boyController.gameObject.transform.position.y, -20))
        {
            MinusLife();
        }

    }

    private void Respawn()
    {
        this.boyRespawner.Execute();
        this.boyController.BoyRigidbody.constraints = RigidbodyConstraints.None;
        this.boyController.BoyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void PlayerHitByEnemy()
    {
        MinusLife();
        this.boyController.IsHit = false;
        
        // Disable Shield after Attack
        if (this.Player.HasShield)
        {
            RemoveShield();
        }
        else
        {
            Invoke("Respawn", 1.5f);
        }
    }

    public void MinusLife()
    {
        //minus life
        this.player.TakesDamage(1);
        OnLifeUpdate?.Invoke();
    }
    
    private void BonusLife()
    {
        this.player.LifeBonus(1);
        OnLifeUpdate?.Invoke();
        this.GemsCollected = 0;
    }

    public void AddShield()
    {
        this.player.HasShield = true;
        OnChangeShieldStatus?.Invoke();
    }

    public void RemoveShield()
    {
        this.player.HasShield = false;
        OnChangeShieldStatus?.Invoke();
    }
}
