using UnityEngine;
using UnityEngine.UI;

public class UIGems : MonoBehaviour
{
    private GameLogic gameLogic;
    private string gemsCollectedText;

    public void SetGemsCollectedText(int gemsCollectedText)
    {
        GetComponent<Text>().text = "x" + gemsCollectedText;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        
        // Start GemsCollected
        SetGemsCollectedText(this.gameLogic.GemsCollected);
        
        // After Gem Collected
        this.gameLogic.OnCollect += GameLogicOnCollect;
    }

    private void GameLogicOnCollect()
    {
        SetGemsCollectedText(this.gameLogic.GemsCollected);
    }
}
