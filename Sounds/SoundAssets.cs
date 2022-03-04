using UnityEngine;

// Source: CodeMonkey: https://www.youtube.com/watch?v=QL29aTa7J5Q
public class SoundAssets : MonoBehaviour
{
    private static SoundAssets _instance;

    public static SoundAssets instance
    {
        get
        {
            if (_instance == null) _instance = Instantiate(Resources.Load<SoundAssets>("SoundAssets"));
            return _instance;
        }
    }

    public AudioClip explosion;
}
