using System;
using UnityEngine;

/**
 * DestructableObject ist an die Objekte geknüpft, die vom Spieler zerstört werden können.
 * Wenn der Spieler das Objekt angreift, wird ein gebrochenes Objekt instanziiert.
 */
public class DestructableObject : MonoBehaviour
{
    [SerializeField] 
    private GameObject brokenObject; // Das Prefab des Zerstörten Objekts

    [SerializeField] private float dissolveTime; // Auflösungszeit

    [Header("Checkpoint")] 
    [SerializeField]
    private bool isCheckpoint;
    
    public event Action OnCheckPointReached; // Wenn das Objekt zerstört wird und ein Checkpoint ist, dann wird der Respawnpunkt neu gesetzt.

    public void OnAttackDestroy()
    {
        Destroy(this.gameObject);
        GameObject brokenPieces = Instantiate(this.brokenObject, transform.position, transform.rotation) as GameObject;

        SoundManager.PlaySound(SoundManager.Sound.WoodHit, brokenPieces.transform.position);
        
        // Only set CheckPoint if destroyed Object is a checkpoint!
        if (this.isCheckpoint)
        {
            OnCheckPointReached?.Invoke(); // Event wird aufgerufen, wenn Checkpoint zerstört wird.
            Debug.Log("Checkpoint set!");
        }
        
        // Alle Teile des Zestörten Objekts werden durchlaufen und erhalten eine Kraft damit die Teile wegfliegen.
        foreach (Transform child in brokenPieces.transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(1f, transform.position, 0.5f);
            }
        }
                
        Destroy(brokenPieces, this.dissolveTime); // Zum Schluss lösen sich die Zerstörten Teile auf.
    }
}
