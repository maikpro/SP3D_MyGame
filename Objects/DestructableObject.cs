using System;
using DefaultNamespace.Controller.Boy;
using UnityEngine;
using UnityEngine.Serialization;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] 
    private GameObject brokenObject;

    [SerializeField] private float dissolveTime;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<BoyController>().Player;
            //Debug.Log("Player collided with box");
            
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
    }
}
