
using UnityEngine;

// Source: CodeMonkey: https://www.youtube.com/watch?v=QL29aTa7J5Q

public static class SoundManager
{
    public static void PlaySound()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(SoundAssets.instance.explosion);
    }
}
