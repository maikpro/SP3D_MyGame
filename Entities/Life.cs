using UnityEngine;

/**
 * Diese Klasse wird für den Zustand der Leben genutzt.
 * Sie wird vom Spieler und Gegner verwendet.
 */
public class Life
{
    private int counter;
    private bool isDead;

    public Life(int counter, bool isDead)
    {
        this.counter = counter;
        this.isDead = isDead;
    }

    /**
     * Lebenspunkte
     */
    public int Counter
    {
        get => counter;
        set => counter = value;
    }

    /**
     * Für die Prüfung, ob der Spieler/Gegner tot sind.
     */
    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }

    /**
     * Ein Lebenspunkte abziehen, minimum sind 0 Lebenpunkte, -1 ist nicht möglich!!
     */
    public void Minus(int damage)
    {
        if (!IsDead && Counter > 0)
        {
            Counter -= damage;
        }
        CheckIfDead(); //Prüfe ob der Spieler/Gegner tot sind?
    }
    
    /**
     * Lebenspunkte erhöhen
     */
    public void Plus(int bonus)
    {
        Counter += bonus;
    }

    /**
     * Für die Prüfung, ob der Spieler/Gegner tot sind.
     */
    public bool CheckIfDead()
    {
        if (Counter <= 0 && !IsDead)
        {
            Debug.Log("Dead!");
            this.IsDead = true;
        }

        return this.IsDead;
    }
        
}
