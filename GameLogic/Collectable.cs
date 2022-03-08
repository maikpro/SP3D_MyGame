using UnityEngine;

/**
 * Collectable wird für das Sammeln von Diamanten, Herzen, Schildern genutzt.
 * Je nach eingesammelten Gegenstand vom Spieler wird die zugehörige Methode aufgerufen.
 * Nach einsammeln des Gegenstandes verschwindet dieser.
 */
public class Collectable : MonoBehaviour
{
    private GameLogic gameLogic;

    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find(GlobalNamingHandler.GAMEOBJECT_GAMELOGIC).GetComponent<GameLogic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GlobalNamingHandler.TAG_PLAYER))
        {
            // Wenn Gems eingesammelt werden, erhöhe die Anzahl der Diamanten
            if (gameObject.CompareTag(GlobalNamingHandler.TAG_GEMS))
            {
                int counter = this.gameLogic.GemsCollected;
                counter++;
                this.gameLogic.GemsCollected = counter;
            }
            // Wenn ein Schild eingesammelt wird (gelber Edelstein), dann bekommt der Spieler ein Schild, der ihn vor einem Angriff schützt.
            if (gameObject.CompareTag(GlobalNamingHandler.TAG_SHIELD))
            {
                this.gameLogic.AddShield();
            }

            // Wenn der Spieler ein Herz einsammelt, bekommt er ein zusätzliches Leben.
            if (gameObject.CompareTag(GlobalNamingHandler.TAG_HEART))
            {
                this.gameLogic.BonusLife();
            }
            
            // Sammelt der Spieler einen Gegenstand ein, verschwindet dieser.
            gameObject.SetActive(false);
        }
    }
}
