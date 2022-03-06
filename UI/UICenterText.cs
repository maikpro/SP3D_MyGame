
using UnityEngine;
using UnityEngine.UI;

public class UICenterText : MonoBehaviour
{
    private GameLogic gameLogic;
    private Text displayText;
    
    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        
        // After Player is dead show text
        this.gameLogic.OnDeadShowCenterText += GameLogicOnDeadShowCenterText;
        this.gameLogic.OnLevelUpCenterText += GameLogicOnLevelUpCenterText;
        this.gameLogic.OnCheckPointReachedText += GameLogicOnCheckPointReachedText;
        
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
