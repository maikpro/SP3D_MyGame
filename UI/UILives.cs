using UnityEngine;
using UnityEngine.UI;

public class UILives : MonoBehaviour
{
    private GameLogic gameLogic;
    private string livesText;

    public void SetLivesText(int livesText)
    {
        GetComponent<Text>().text = "x" + livesText.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();

        //Start Lives
        SetLivesText(this.gameLogic.Player.LifeCounter());

        //After Falling

        this.gameLogic.OnLifeUpdate += GameLogicOnLifeUpdate;
    }

    private void GameLogicOnLifeUpdate()
    {
        // aktualisiere die Lebensanzeige
        int currentLives = this.gameLogic.Player.Life.Counter;
        SetLivesText(currentLives);
    }
}
