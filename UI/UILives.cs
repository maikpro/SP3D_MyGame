using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILives : MonoBehaviour
{
    private GameLogic gameLogic;
    private string livesText;

    public void SetLivesText(int livesText)
    {
        GetComponent<Text>().text = livesText.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT GAMELOGIC MUST BE SET BEFORE!!!
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        
        //Start Lives
        Debug.Log(this.gameLogic.Player);
        SetLivesText(this.gameLogic.Player.LifeCounter());
        
        //After Falling
        
        gameLogic.OnFall += GameLogicOnFall;
    }

    private void GameLogicOnFall()
    {
        // Nachdem der Boy von der Platform fällt aktualisiere die Lebensanzeige
        int currentLives = this.gameLogic.Player.Life.Counter;
        SetLivesText(currentLives);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
