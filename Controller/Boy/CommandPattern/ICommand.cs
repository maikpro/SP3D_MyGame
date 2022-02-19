/**
 * Command Pattern:
 * wird für die unterschiedlichen Aktionen des Charakters verwendet
 * Je nach Eingabe des Spieler wird ein entsprechendes Kommando ausgeführt.
 * Quelle: R. Nystrom, Game Programming Patterns,
 * https://gameprogrammingpatterns.com/command.html
 */
public interface ICommand 
{
    void Execute();
}
