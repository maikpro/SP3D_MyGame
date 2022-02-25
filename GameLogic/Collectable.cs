using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private GameLogic gameLogic;

    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int counter = this.gameLogic.GemsCollected;
            counter++;
            this.gameLogic.GemsCollected = counter;
            gameObject.SetActive(false);
        }
    }
}
