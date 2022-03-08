
using UnityEngine;

/**
 * Das DanceCommando wird beim erreichen der Zielplattform abgespielt oder wenn der Spieler die Taste "o" drückt.
 * Es ist nicht für den Spielverlauf relevant ist aber ein nice-to-have :D
 */
public class DanceCommand : ICommand
{
    private Animator animator;

    public DanceCommand(Animator animator)
    {
        this.animator = animator;
    }

    public void Execute()
    {
        Dance();
    }

    private void Dance()
    {
        Debug.Log("DAAAAANCE!");
        this.animator.SetBool(GlobalNamingHandler.DANCE_PARAMETER_NAME, true);
    }
}
