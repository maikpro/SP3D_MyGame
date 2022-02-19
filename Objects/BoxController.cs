using System;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private Rigidbody boxRigidbody;
    void Start()
    {
        this.boxRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(gameObject.name + " collided with " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player"))
        {
            //TODO: Push Box into Direction
            this.boxRigidbody.AddForce(Vector3.forward * 25.0f, ForceMode.Impulse);
        }
    }
}
