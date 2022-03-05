
using System.Collections.Generic;
using UnityEngine;

// Source: CodeMonkey - Simple Sound Manager (Unity Tutorial): https://www.youtube.com/watch?v=QL29aTa7J5Q

public static class SoundManager
{
    public enum Sound
    {
        // Collected
        LevelUp,
        GemCollected,
        ShieldCollected,
        
        // Boy
        boyAttack,
        
        // Enemy
        enemyAttack,
        
        // Explosion
        Explosion,
        ExplosionAlarm,
    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.ExplosionAlarm] = 0f;
        soundTimerDictionary[Sound.Explosion] = 0f;
        soundTimerDictionary[Sound.boyAttack] = 0f;
    }
    
    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            
            
            case Sound.boyAttack:
                return DelayPlaySound(sound, 1f);
                

            case Sound.ExplosionAlarm:
                return DelayPlaySound(sound, 1f);
                
            
            case Sound.Explosion:
                return DelayPlaySound(sound, 2f);
                
        }

        return true;
    }

    private static bool DelayPlaySound(Sound sound, float delay)
    {
        if (soundTimerDictionary.ContainsKey(sound))
        {
            float lastTimePlayed = soundTimerDictionary[sound];
            //float delay = 1f;
            if (lastTimePlayed + delay < Time.time)
            {
                soundTimerDictionary[sound] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    
    public static GameObject PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.01f;
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
            return soundGameObject;
        }

        return null;
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.instance.soundAudioClips)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
