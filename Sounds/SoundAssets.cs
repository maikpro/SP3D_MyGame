using System;
using UnityEngine;

/** Source:
 * CodeMonkey - Simple Sound Manager (Unity Tutorial): https://www.youtube.com/watch?v=QL29aTa7J5Q
 * CodeMonkey - Quick Tip: Referencing Assets through Code | Unity Tutorial: https://www.youtube.com/watch?v=7GcEW6uwO8E 
 * */
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

    public SoundAudioClip[] soundAudioClips;

    [Serializable]
    public class SoundAudioClip         
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
