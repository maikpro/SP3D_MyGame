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
            // Wenn Gems eingesammelt werden
            if (gameObject.CompareTag("Gems"))
            {
                int counter = this.gameLogic.GemsCollected;
                counter++;
                this.gameLogic.GemsCollected = counter;
            }

            if (gameObject.CompareTag("Shield"))
            {
                this.gameLogic.AddShield();
            }

            if (gameObject.CompareTag("Heart"))
            {
                this.gameLogic.BonusLife();
            }
            
            gameObject.SetActive(false);
        }
    }
}
