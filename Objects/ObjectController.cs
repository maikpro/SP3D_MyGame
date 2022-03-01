using System;
using DefaultNamespace.Controller.Boy;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField]
    private bool breakable;

    [SerializeField] 
    private GameObject brokenBox;



    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && this.breakable)
        {
            Player player = other.gameObject.GetComponent<BoyController>().Player;
            //Debug.Log("Player collided with box");
            
            if (player.IsAttacking)
            {
                Destroy(this.gameObject);
                GameObject brokenPieces = Instantiate(this.brokenBox, transform.position, transform.rotation) as GameObject;

                foreach (Transform child in brokenPieces.transform)
                {
                    if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
                    {
                        childRigidbody.AddExplosionForce(1f, transform.position, 0.5f);
                    }
                }
                
                Destroy(brokenPieces, 2f);
            }
        }
    }
}
