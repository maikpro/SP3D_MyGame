using System;
using UnityEngine;

/**
 * GoalReached ist ein Script, das an die Zielplattform geknüpft ist,
 * wenn der Spieler diesen erreicht, hat er das Level geschafft und das nächste Level wird geladen.
 */
public class GoalReached : MonoBehaviour
{
    public event Action OnGoalReached;
    private bool isGoalReached;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(GlobalNamingHandler.TAG_PLAYER) && !isGoalReached)
        {
            Debug.Log("Player has reached Goal!");
            this.isGoalReached = true;
            OnGoalReached?.Invoke(); // Das Event OnGoalReached wird hier aufgerufen und in der GameLogi ausgeführt.
        }
    }
}
