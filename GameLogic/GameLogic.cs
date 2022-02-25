using System;
using DefaultNamespace;
using DefaultNamespace.Controller.Boy;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // EDITOR VARIABLES
    [Header("Player Settings")]
    [SerializeField]
    private int playerLifeCounter;

    [SerializeField] private int gemsCollected;
    
    //PRIVATE FIELDS
    private BoyController boyController;
    private Respawn boyRespawner;
    private Player player;
    
    // EVENTS
    public event Action OnLifeUpdate; // Action for Respawn after Falling 
    public event Action OnCollect; // Action for Collection of Gems

    
    // TODO RESPAWNER EVENT!!
    // TODO OnDead Animation
    // TODO Fight with Enemy
    // TODO Checkpoints
    
    
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
        this.player = new Player(new Life(this.playerLifeCounter, false), false);
        this.boyRespawner = new Respawn(this.boyController.gameObject, transform.position);
        this.boyRespawner.SetStartPosition(new Vector3(219.809998f,0f,2.41199994f));
        
        
        
        // Spawn Enemies
    }

    // Update is called once per frame
    void Update()
    {
        // Respawn after Falling down
        if (this.boyRespawner.afterFall(this.boyController.gameObject.transform.position.y, -20))
        {
            //minus life
            this.player.TakesDamage(1);
            OnLifeUpdate?.Invoke();
        }

        
        
        // Check Player Lives with Events
        

    }

    private void BonusLife()
    {
        this.player.LifeBonus(1);
        OnLifeUpdate?.Invoke();
        this.GemsCollected = 0;
    }
}
