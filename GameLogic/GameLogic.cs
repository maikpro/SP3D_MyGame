using System;
using DefaultNamespace;
using DefaultNamespace.Controller.Boy;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // EDITOR VARIABLES
    [Header("Player Settings")]
    [SerializeField]
    [Range(1,99)]
    private int playerLifeCounter;

    [SerializeField] 
    [Range(0,99)]
    private int gemsCollected;

    [SerializeField] 
    private bool playerHasShield;
    
    //PRIVATE FIELDS
    private BoyController boyController;
    private Respawn boyRespawner;
    private Player player;
    
    private GoalReached goalReached;
    
    // Objects
    private DestructableObject[] checkPoints;
    
    // EVENTS
    public event Action OnLifeUpdate; // Action for Respawn after Falling 
    public event Action OnCollect; // Action for Collection of Gems

    public event Action OnChangeShieldStatus; // Action for Collection of Shields // Only 1 Shield possible cause of bool usage!!!
    public event Action OnDeadShowCenterText; // Show UI Game Over Text on Event
    public event Action OnLevelUpCenterText; // Show UI Level Up!

    public event Action OnCheckPointReachedText;

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
            else
            {
                // Play basic sound 
                GameObject soundObject = SoundManager.PlaySound(SoundManager.Sound.GemCollected, this.boyController.transform.position);
                Destroy(soundObject, 1f);
            }
        }
    }

    void Awake()
    {
        this.boyController = GameObject.Find("Boy").GetComponent<BoyController>();
        this.player = new Player(new Life(this.playerLifeCounter, false), this.playerHasShield, false);
        this.boyRespawner = new Respawn(this.boyController.gameObject, transform.position);
        //this.boyRespawner.SetStartPosition(new Vector3(219.809998f,0f,2.41199994f));

        SoundManager.Initialize();
    }

    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        //this.destructableObject = GameObject.Find("DestructableObject").GetComponent<DestructableObject>();
        this.checkPoints = FindObjectsOfType<DestructableObject>();
        this.boyRespawner.SetStartPositionAtBegin(this.boyController.transform.position);
        
        this.goalReached = GameObject.Find("GoalPlatform").GetComponent<GoalReached>();
        this.goalReached.OnGoalReached += GoalReachedOnGoalReached;
        
        // Subscribe to Event OnCheckPointReached from DestructableObject
        //this.destructableObject.OnCheckPointReached += DestructableObjectOnCheckPointReached;
        foreach (var checkpoint in this.checkPoints)
        {
            checkpoint.OnCheckPointReached += DestructableObjectOnCheckPointReached;
        }
        
        DontDestroyOnLoad(gameObject); // Dont Destroy GameLogic
        DontDestroyOnLoad(GameObject.Find("UI"));
    }

    /**
     * Load Level 2 when Goal is reached after a Dance! :D
     */
    private void GoalReachedOnGoalReached()
    {
        this.boyController.PlayerDance();
        Invoke("LoadingAfterDelay",5f);
        // DoDance
        
    }

    private void LoadingAfterDelay()
    {
        Loader.Load(Loader.Scene.Level_2);
        Vector3 nextLevelSpawnPoint = new Vector3(112.658646f,0.0249999762f,6.83869505f);
        this.boyRespawner.NextLevelSpawnPoint(nextLevelSpawnPoint);
    }

    private void DestructableObjectOnCheckPointReached()
    {
        OnCheckPointReachedText?.Invoke();
        
        // Set new StartPoint aka Checkpoint!
        this.boyRespawner.SetCheckPointPosition(this.boyController.gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Respawn after Falling down
        if (this.boyRespawner.AfterFall(this.boyController.transform.position.y, this.boyRespawner.StartPosition.y - 20))
        {
            Debug.Log("Respawn After Fall!");
            MinusLife();
        }

    }

    private void Respawn()
    {
        this.boyRespawner.Execute();
        this.boyController.BoyRigidbody.constraints = RigidbodyConstraints.None;
        this.boyController.BoyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void PlayerHit()
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
        
        if (this.player.Life.CheckIfDead())
        {
            OnDeadShowCenterText?.Invoke(); //Zeige an dass Spieler gestorben ist.
            Invoke("PlayerDead", 5f);
        }
        
        OnLifeUpdate?.Invoke();
    }

    /*
     * When Player dies
     * respawn him at the startposition
     * and give him startlives back!
     */
    private void PlayerDead()
    {
        this.boyRespawner.BackToStartPosition();
        this.player = new Player(new Life(this.playerLifeCounter, false), this.playerHasShield, false);
        OnLifeUpdate?.Invoke(); // Update UI nochmal
        
        // Restart Level
        //Application.LoadLevel(Application.loadedLevel);
        //Loader.Load(Loader.Scene.Level_1);
        Loader.ReloadCurrentLevel();
    }

    public void BonusLife()
    {
        this.player.LifeBonus(1);
        OnLifeUpdate?.Invoke();
        
        if(this.GemsCollected > 99) this.GemsCollected = 0;
        
        OnLevelUpCenterText?.Invoke();
        
        // Level Up Sound
        SoundManager.PlaySound(SoundManager.Sound.LevelUp, this.boyController.transform.position);
    }

    public void AddShield()
    {
        this.player.HasShield = true;
        OnChangeShieldStatus?.Invoke();
        
        // Shield Sound
        SoundManager.PlaySound(SoundManager.Sound.ShieldCollected, this.boyController.transform.position);
    }

    public void RemoveShield()
    {
        this.player.HasShield = false;
        OnChangeShieldStatus?.Invoke();
    }
}
