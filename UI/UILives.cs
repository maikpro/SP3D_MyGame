using UnityEngine;
using UnityEngine.UI;

/**
 * UILives ist für die Lebenspunkte oben rechts vernantwortlich
 */
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
        this.gameLogic = GameObject.Find(GlobalNamingHandler.GAMEOBJECT_GAMELOGIC).GetComponent<GameLogic>();

        //Start Lives
        SetLivesText(this.gameLogic.Player.LifeCounter());

        this.gameLogic.OnLifeUpdate += GameLogicOnLifeUpdate; // Ändern sich die Lebenspunkte wird das Event in GameLogic aufgerufen und die Lebensanzeige wird aktualisiert
        
    }

    private void GameLogicOnLifeUpdate()
    {
        // aktualisiere die Lebensanzeige
        int currentLives = this.gameLogic.Player.Life.Counter;
        SetLivesText(currentLives);
    }
}
