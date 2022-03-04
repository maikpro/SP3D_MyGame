using UnityEngine;

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
        this.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        
        this.gameLogic.OnChangeShieldStatus += GameLogicOnChangeShieldStatus;

        SetHasShieldUI();
    }

    private void GameLogicOnChangeShieldStatus()
    {
        SetHasShieldUI();
    }
}
