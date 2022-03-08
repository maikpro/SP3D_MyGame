using UnityEngine;
using UnityEngine.UI;

/**
 * UIGems ist für die Anzeige der Diamantenanzahl oben links zuständig.
 */
public class UIGems : MonoBehaviour
{
    private GameLogic gameLogic;
    private string gemsCollectedText;

    private void SetGemsCollectedText(int gemsCollectedText)
    {
        GetComponent<Text>().text = "x" + gemsCollectedText;
    }

    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find(GlobalNamingHandler.GAMEOBJECT_GAMELOGIC).GetComponent<GameLogic>();
        
        // Start GemsCollected
        SetGemsCollectedText(this.gameLogic.GemsCollected);
        
        // After Gem Collected
        this.gameLogic.OnCollect += GameLogicOnCollect; // Beim Einsammel eines Diamants, erhöht sich die Anzahl der Diamantenanzeige.
    }

    private void GameLogicOnCollect()
    {
        SetGemsCollectedText(this.gameLogic.GemsCollected);
    }
}
