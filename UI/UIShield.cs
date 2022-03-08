using UnityEngine;

/**
 * UIShield zeigt an, ob der Spieler einen Schild hat oben rechts.
 * Der Schild schützt den Spieler vor dem nächsten Angriff des Gegeners.
 */
public class UIShield : MonoBehaviour
{
    private GameLogic gameLogic;
    private string shieldText;
    private bool hasShield;

    private void SetHasShieldUI()
    {
        this.hasShield = this.gameLogic.Player.HasShield;
        gameObject.SetActive(hasShield);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find(GlobalNamingHandler.GAMEOBJECT_GAMELOGIC).GetComponent<GameLogic>();
        
        this.gameLogic.OnChangeShieldStatus += GameLogicOnChangeShieldStatus; // Wenn das Event aufgerufen wird, wird die Anzeige des Schilds geändert.

        SetHasShieldUI();
    }

    private void GameLogicOnChangeShieldStatus()
    {
        SetHasShieldUI();
    }
}
