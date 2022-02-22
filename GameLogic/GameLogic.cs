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
    
    //PRIVATE FIELDS
    private BoyController boyController;
    private Respawn boyRespawner;
    private Player player;
    
    // EVENTS
    public event Action OnFall;

    
    // TODO RESPAWNER EVENT!!
    // TODO OnDead Animation
    // TODO Fight with Enemy
    // TODO Checkpoints
    
    
    public Player Player
    {
        get => player;
        set => player = value;
    }
    
    void Awake()
    {
        // Spawn Boy with 5 Lives
        this.boyController = GameObject.Find("Boy").GetComponent<BoyController>();
        this.player = new Player(new Life(this.playerLifeCounter, false), false);
        this.boyRespawner = new Respawn(this.boyController.gameObject, transform.position);
        this.boyRespawner.SetStartPosition(new Vector3(219.809998f,0f,2.41199994f));
        
        // Spawn Enemies
    }

    // Update is called once per frame
    void Update()
    {
        //Test
        //Debug.Log(this.boyController.text);

        if (this.boyRespawner.afterFall(this.boyController.gameObject.transform.position.y, -20))
        {
            //minus life
            this.player.TakesDamage(1);
            OnFall?.Invoke();
        }
        
        // Check Player Lives with Events
        

    }
}
