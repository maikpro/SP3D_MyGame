using System;
using UnityEngine;

public class GoalReached : MonoBehaviour
{
    public event Action OnGoalReached;
    private bool isGoalReached;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isGoalReached)
        {
            Debug.Log("Player has reached Goal!");
            this.isGoalReached = true;
            OnGoalReached?.Invoke();
        }
    }
}
