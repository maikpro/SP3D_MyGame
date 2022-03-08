using UnityEngine.SceneManagement;

// Source: Codemonkey - Scene Manager in Unity (Unity Tutorial) https://www.youtube.com/watch?v=3I5d2rUJ0pE

/**
 * Diese Klasse ist für das Laden der Szenen verantwortlich.
 */
public static class Loader
{
    // Alle Level in dem Spiel
    public enum Scene
    {
        Level_1,
        Level_2
    }
    
    // Lädt die übergebene Szene erneut.
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    // Lädt die aktuelle Szene erneut
    public static void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
