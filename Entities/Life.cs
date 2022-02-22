using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life
{
    private int counter;
    private bool isDead;

    public Life(int counter, bool isDead)
    {
        this.counter = counter;
        this.isDead = isDead;
    }

    public int Counter
    {
        get => counter;
        set => counter = value;
    }

    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }

    public void Minus(int damage)
    {
        if (!IsDead && Counter > 0)
        {
            Counter -= damage;
        }
        CheckIfDead();
    }
    
    public void Plus(int bonus)
    {
        Counter += bonus;
    }

    public void CheckIfDead()
    {
        if (Counter <= 0 && !IsDead)
        {
            Debug.Log("Dead!");
            IsDead = true;
        }
    }
        
}
