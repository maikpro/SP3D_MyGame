using UnityEngine.SceneManagement;

// Source: Codemonkey - Scene Manager in Unity (Unity Tutorial) https://www.youtube.com/watch?v=3I5d2rUJ0pE

public static class Loader
{
    public enum Scene
    {
        Level_1,
        Level_2
    }
    
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
