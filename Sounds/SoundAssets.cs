using System;
using UnityEngine;

/** Source:
 * CodeMonkey - Simple Sound Manager (Unity Tutorial): https://www.youtube.com/watch?v=QL29aTa7J5Q
 * CodeMonkey - Quick Tip: Referencing Assets through Code | Unity Tutorial: https://www.youtube.com/watch?v=7GcEW6uwO8E 
 * */

/**
 * SoundAssets wird für die Nutzung der Soundeffekte verwendet.
 * Hierbei können über den Unity-Editor alle Sound-Effekte in einem Array übergeben werden.
 * Die Klasse regelt die Instanziierung der SoundAssets. So können die Assets über den Soundmanager aufgerufen werden.
 */

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
