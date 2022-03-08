
using UnityEngine;
using UnityEngine.UI;

/**
 * UICenterText ist für die Textanzeige in der Mitte des Bildschirms zuständig.
 * Momentan werden Game Over, +1 Bonus Up! und Checkpoint reached angezeigt.
 */
public class UICenterText : MonoBehaviour
{
    private GameLogic gameLogic;
    private Text displayText;
    
    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find(GlobalNamingHandler.GAMEOBJECT_GAMELOGIC).GetComponent<GameLogic>();
        
        // After Player is dead show text
        this.gameLogic.OnDeadShowCenterText += GameLogicOnDeadShowCenterText; // Wenn der Spieler stirbt => Game Over
        this.gameLogic.OnLevelUpCenterText += GameLogicOnLevelUpCenterText; // Wenn der Spieler ein Lebenspunkt erhält => +1 Bonus Up!
        this.gameLogic.OnCheckPointReachedText += GameLogicOnCheckPointReachedText; // Wenn der Spieler den Checkpoint zerstört => Checkpoint reached.

        this.displayText = GetComponent<Text>();
        this.displayText.enabled = false;
        
    }

    private void GameLogicOnCheckPointReachedText()
    {
        SetCenterText("Checkpoint reached", Color.green, 5f);
    }

    private void GameLogicOnLevelUpCenterText()
    {
        SetCenterText("+1 Bonus Up!", Color.green, 3f);
    }

    private void GameLogicOnDeadShowCenterText()
    {
        SetCenterText("Game Over!", Color.red, 5f);
    }
    
    private void SetCenterText(string text, Color color, float delayToClear)
    {
        this.displayText.enabled = true;
        displayText.text = text;
        displayText.color = color;
        Invoke("ClearText", delayToClear);
    }

    private void ClearText()
    {
        this.displayText.enabled = false;
        this.displayText.text = "";
    }
}
