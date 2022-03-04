using System;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] 
    private GameObject brokenObject;

    [SerializeField] private float dissolveTime;

    [Header("Checkpoint")] 
    [SerializeField]
    private bool isCheckpoint;
    
    public event Action OnCheckPointReached;

    public void OnAttackDestroy()
    {
        Destroy(this.gameObject);
        GameObject brokenPieces = Instantiate(this.brokenObject, transform.position, transform.rotation) as GameObject;
        
        // Only set CheckPoint if destroyed Object is a checkpoint!
        if (this.isCheckpoint)
        {
            OnCheckPointReached?.Invoke();
            Debug.Log("Checkpoint set!");
        }
        
        foreach (Transform child in brokenPieces.transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(1f, transform.position, 0.5f);
            }
        }
                
        Destroy(brokenPieces, this.dissolveTime);
    }

    /*private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<BoyController>().Player;
            
            if (player.IsAttacking)
            {
                Destroy(this.gameObject);
                GameObject brokenPieces = Instantiate(this.brokenObject, transform.position, transform.rotation) as GameObject;

                foreach (Transform child in brokenPieces.transform)
                {
                    if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
                    {
                        childRigidbody.AddExplosionForce(1f, transform.position, 0.5f);
                    }
                }
                
                Destroy(brokenPieces, this.dissolveTime);
            }
        }
    }*/
}
