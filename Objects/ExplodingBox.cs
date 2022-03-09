using DefaultNamespace.Controller.Enemies;
using DefaultNamespace.Util;
using UnityEngine;

/**
 * ExplodingBox ist für die Objekte, die explodieren sollen, sobald der Spieler den Gegenstand berührt.
 */
public class ExplodingBox : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float timeToExplode;
    [SerializeField] private float dissolveTime;

    [Header("Object")]
    [SerializeField] 
    private GameObject brokenObject;
    [SerializeField]
    private float hitRadius;
    [SerializeField]
    private LayerMask hitLayers;
    
    [Header("Effect")]
    [SerializeField] 
    private GameObject explosionEffect;

    [Header("Material")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Color touchedColor;
    
    private bool isTouched;
    private Countdown countdown;

    private Material material;
    private bool isMaterialChanged;

    
    private bool isExploding;

    public bool IsExploding
    {
        get => isExploding;
        set => isExploding = value;
    }

    void Start()
    {
        this.isTouched = false;
        this.isMaterialChanged = false;
        this.isExploding = false;
        this.countdown = new Countdown(this.timeToExplode);

        this.material = GetComponent<MeshRenderer>().material;
        
        //this.material.color = Color.magenta;
    }

    void Update()
    {
        if (isTouched)
        {
            this.countdown.Run(); // Startet den Countdown der Bombe, wenn der Spieler die Box berührt.
            
            // Change Material every Sec
            if (!this.isMaterialChanged && (this.countdown.CurrentTime % 2 == 0))
            {
                this.material.color = this.touchedColor; // Ändert die Farbe der Box alle 2 Sekunden
                this.isMaterialChanged = true;
                
                SoundManager.PlaySound(SoundManager.Sound.ExplosionAlarm, transform.position); // Spielt einen Alarm-Sound ab, wenn der Spieler die Box berührt hat
            }
            else
            {
                this.material.color = this.defaultMaterial.color; // Standardfarbe zum Hin und Her wechseln der Farbe
                this.isMaterialChanged = false;
            }
        }

        if (this.countdown.CurrentTime == 0)
        {
            Explode();
            SoundManager.PlaySound(SoundManager.Sound.Explosion, transform.position); // Explosions-Sound
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // wenn der Player die Box berührt geht der Countdown für die Explosion los
        if (other.gameObject.CompareTag(GlobalNamingHandler.TAG_PLAYER))
        {
            this.isTouched = true;
        }
        
        // wenn eine ExplosionBox neben einer anderen ExplosionBox steht, die explodiert dann explodiere auch:
        if (other.gameObject.CompareTag(GlobalNamingHandler.TAG_EXPLODING_BOX))
        {
            Explode();
        }
    }

    /**
     * Wenn die Box explodiert, wird diese Methode aufgerufen, dabei wird das gleiche Prinzip wie bei DestructableObject genutzt.
     * Eine zerstörte Box wird instanziiert und von der Box ersetzt, dann fliegen die Teile in die Luft.
     * Hierbei wird ein Explosionseffect angezeigt.
     */
    
    /**
     * Wie in DestructableObject.cs
     * Für die Zerstörung des Objekt wurden die Videos als Quelle verwendet:
     * Code Monkey - Awesome Easy DESTRUCTION in Unity! (Add SECRETS!), https://www.youtube.com/watch?v=tPWMZ4Ic7PA
     * Code Monkey - SLICE objects, CUT doors or BREAK them inside Unity!, https://www.youtube.com/watch?v=InpKZloVk0w
     */
    private void Explode()
    {
        Destroy(this.gameObject);
        GameObject brokenPieces = Instantiate(this.brokenObject, transform.position, transform.rotation) as GameObject;

        GameObject effect = Instantiate(this.explosionEffect, transform.position + (Vector3.up*0.5f), transform.rotation);
        
        foreach (Transform child in brokenPieces.transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(1f, transform.position, 0.5f);
            }
        }
        
        // When Player/Enemy is in exploding range damage him
        Damage();
                
        Destroy(brokenPieces, this.dissolveTime); //Löse BoxStücke auf
        Destroy(effect, 1f); // Löse Explsionseffect auf
        

        this.isExploding = true;
    }

    /**
     * Wenn der Spieler oder ein Gegner in der Nähe der Explosion sind, dann füge ihnen einen Schaden zu.
     */
    private void Damage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, this.hitRadius, this.hitLayers);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag(GlobalNamingHandler.TAG_ENEMY))
            {
                Enemy enemy = collider.GetComponent<EnemyController>().Enemy;
                enemy.TakesDamage(1);
                
                Debug.Log(collider.name + " left " + enemy.Life.Counter);
            }
                
            else if (collider.CompareTag(GlobalNamingHandler.TAG_PLAYER))
            {
                BoyController boyController = collider.GetComponent<BoyController>();
                boyController.IsHit = true;
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, this.hitRadius);
    }
}
