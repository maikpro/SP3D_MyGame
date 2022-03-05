using System;
using DefaultNamespace.Controller.Enemies;
using DefaultNamespace.Util;
using UnityEngine;

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
            this.countdown.Run();
            
            // Change Material every Sec
            if (!this.isMaterialChanged && (this.countdown.CurrentTime % 2 == 0))
            {
                this.material.color = this.touchedColor;
                this.isMaterialChanged = true;
                
                SoundManager.PlaySound(SoundManager.Sound.ExplosionAlarm, transform.position);
            }
            else
            {
                this.material.color = this.defaultMaterial.color;
                this.isMaterialChanged = false;
            }
        }

        if (this.countdown.CurrentTime == 0)
        {
            Explode();
            SoundManager.PlaySound(SoundManager.Sound.Explosion, transform.position);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // wenn der Player die Box berührt geht der Countdown für die Explosion los
        if (other.gameObject.CompareTag("Player"))
        {
            this.isTouched = true;
        }
        
        // wenn eine ExplosionBox neben einer anderen ExplosionBox steht, die explodiert dann explodiere auch:
        if (other.gameObject.CompareTag("ExplodingBox"))
        {
            Explode();
        }
    }

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
                
        Destroy(brokenPieces, this.dissolveTime);
        Destroy(effect, 1f);
        

        this.isExploding = true;
    }

    private void Damage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, this.hitRadius, this.hitLayers);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<EnemyController>().Enemy;
                enemy.TakesDamage(1);
                
                Debug.Log(collider.name + " left " + enemy.Life.Counter);
            }
                
            else if (collider.CompareTag("Player"))
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
