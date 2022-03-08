using System;
using DefaultNamespace;
using DefaultNamespace.Controller.Boy;
using UnityEngine;

/**
 * GameLogic ist für die Verwaltung der Spiellogik verantwortlich
 * von hier aus werden die Leben des Spielers verwaltet, die Camera zugewiesen und das Laden der Scenen aufgerufen
 */

public class GameLogic : MonoBehaviour
{
    [Header("Camera Settings")] 
    [SerializeField]
    private GameObject camera;
    
    // EDITOR VARIABLES
    [Header("Player Settings")] 
    [SerializeField]
    private GameObject startPoint;
    
    [SerializeField]
    private GameObject boy;
    
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
    private GameObject boyInstance;

    private GoalReached goalReached;
    
    // Objects
    private DestructableObject[] checkPoints;
    
    /**
     * EVENTS werden subscribed und werden aufgerufen, wenn ein bestimmtes Ereignis auftritt.
     * Damit kann die UI von der Gamelogik getrennt werden und der Code bleibt übersichtlicher.
     */
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
            OnCollect?.Invoke(); // Event wird aufgerufen wenn ein Diamant eingesammelt wird
            
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
        /**
         * Beim allerersten Laden des Spiels wird der Spieler (boy) instanziiert.
         * Die Kamera wird dem Spieler und der Spieler der Kamera zugewiesen, damit die Funktionen,
         * der 3rd Person Kamera funktioniert.
         */
        
        if (GameObject.Find(GlobalNamingHandler.boyName) == null)
        {
            // ON FIRST GAMESTART CREATE ALL INSTANCES!
            this.boyInstance = Instantiate(this.boy, this.startPoint.transform.position, this.startPoint.transform.rotation);
            Debug.Log(boyInstance.name);
            GlobalNamingHandler.boyName = this.boyInstance.name;
        
            // Camera References on Gamestart!
            this.boyController = boyInstance.GetComponent<BoyController>();
            SoundManager.Initialize();
        }
        else
        {
            // AFTER DEAD REUSE REFERENCES!
            this.boyController = GameObject.Find(GlobalNamingHandler.boyName).GetComponent<BoyController>();
            this.camera.GetComponent<MainCameraGameObject>().target = this.boyController.transform;
        }
        
        this.camera.GetComponent<MainCameraGameObject>().target = this.boyController.transform;
        this.boyController.Camera = this.camera.transform;
        
        this.player = new Player(new Life(this.playerLifeCounter, false), this.playerHasShield, false);
        this.boyRespawner = new Respawn(this.boyController.gameObject, transform.position);
        
        // Dont Destroy these GameObjects on reload
        DontDestroyOnLoad(gameObject); // Dont Destroy GameLogic
        DontDestroyOnLoad(GameObject.Find(GlobalNamingHandler.GAMEOBJECT_UI));
    }

    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.checkPoints = FindObjectsOfType<DestructableObject>();
        this.boyRespawner.SetStartPositionAtBegin(this.boyController.transform.position);
        
        this.goalReached = GameObject.Find(GlobalNamingHandler.GAMEOBJECT_GOALPLATTFORM).GetComponent<GoalReached>();
        this.goalReached.OnGoalReached += GoalReachedOnGoalReached; // Subscribe to GoalReached Event in GoalReached Script
        
        // Subscribe to Event OnCheckPointReached from DestructableObject
        //this.destructableObject.OnCheckPointReached += DestructableObjectOnCheckPointReached;
        foreach (var checkpoint in this.checkPoints)
        {
            checkpoint.OnCheckPointReached += DestructableObjectOnCheckPointReached;
        }
    }

    /**
     * Lade Level 2 und wenn das Ziel erreicht ist soll der Spieler tanzen
     */
    private void GoalReachedOnGoalReached()
    {
        //SoundManager.PlaySound(SoundManager.Sound.Dance, transform.position);
        AudioSource audioSource = GameObject.Find("Background_Music").GetComponent<AudioSource>(); 
        audioSource.clip = SoundManager.GetAudioClip(SoundManager.Sound.Dance);
        audioSource.Play();
        this.boyController.PlayerDance();
        Invoke("LoadingAfterDelay",5f);
        // DoDance
        
    }

    /**
     * Die Szene von Level_2 wird durch Invoke nach 5 Sekunden geladen.
     * und der Spieler wird an einem bestimmten Punkt gespawnt.
     */
    private void LoadingAfterDelay()
    {
        DontDestroyOnLoad(this.camera);
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
            if (this.player.HasShield)
            {
                RemoveShield();
            }
            
            MinusLife();
        }

    }

    /**
     * Der Spieler wird an dem jeweiligen Checkpoint, wenn dieser aktiviert worden ist zurück gespawnt
     * oder an den Start wenn kein Checkpoint aktiviert wurde
     */
    private void Respawn()
    {
        this.boyRespawner.Execute();
    }

    /**
     * PlayerHit wird aufgerufen, wenn der Spieler vom Gegener getroffen wird
     */
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

    /**
     * Zieht dem Spieler ein Leben ab
     * nachdem er runterfällt oder er von einem Gegner geschlagen wird.
     */
    public void MinusLife()
    {
        // Lebensabzug
        this.player.TakesDamage(1);
        
        // Prüft ob der Spieler nach Lebensabzug noch genug Leben hat (0 == Dead)
        if (this.player.Life.CheckIfDead())
        {
            OnDeadShowCenterText?.Invoke(); //Zeige an dass Spieler gestorben ist.
            Invoke("PlayerDead", 5f);
        }
        
        OnLifeUpdate?.Invoke();
    }

    /*
     * Wenn der Spieler stirbt
     * respawne ihn an der startposition
     * und resette alle Leben
     */
    private void PlayerDead()
    {
        this.boyRespawner.BackToStartPosition();
        this.player = new Player(new Life(this.playerLifeCounter, false), this.playerHasShield, false);
        OnLifeUpdate?.Invoke(); // Update UI nochmal

        // Ladet die aktuelle Szene nochmal neu
        Loader.ReloadCurrentLevel();
    }

    /**
     * Wenn der Spieler 100 Diamanten eingesammelt hat oder ein Herz dann erscheint
     * im UI ein +1 Bonus Life Text und seine Leben steigen an.
     */
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
        
        // Wemm der Spieler ein Schild eingesammelt hat, dann zeige das Schild oben rechts an.
        OnChangeShieldStatus?.Invoke();
        
        // Schild-Sound abspielen
        SoundManager.PlaySound(SoundManager.Sound.ShieldCollected, this.boyController.transform.position);
    }

    public void RemoveShield()
    {
        this.player.HasShield = false;
        OnChangeShieldStatus?.Invoke();
    }
}
