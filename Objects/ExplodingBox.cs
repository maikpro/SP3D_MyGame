using System;
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
        
        SoundManager.PlaySound();
        
        foreach (Transform child in brokenPieces.transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(1f, transform.position, 0.5f);
            }
        }
                
        Destroy(brokenPieces, this.dissolveTime);
        Destroy(effect, 1f);

        this.isExploding = true;
    }
}
