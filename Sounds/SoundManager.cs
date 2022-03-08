
using System.Collections.Generic;
using UnityEngine;

// Source: CodeMonkey - Simple Sound Manager (Unity Tutorial): https://www.youtube.com/watch?v=QL29aTa7J5Q

/**
 * Der Soundmanager regelt die Soundeffekte er wird ein Typ übergeben und zum passenden Typ wird der richtige Sound dann abgespielt.
 */
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
        enemyUh,
        enemyHey,
        
        // Explosion
        Explosion,
        ExplosionAlarm,
        
        //Wood Hit
        WoodHit,
        
        //Dance at Goal
        Dance,
    }

    // Dictionary soll die Zeit regeln, wann der gleiche Sound erneut abgespielt werden kann.
    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize() //Instanziierung und startwerte der Sounds
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.ExplosionAlarm] = 0f;
        soundTimerDictionary[Sound.Explosion] = 0f;
        soundTimerDictionary[Sound.boyAttack] = 0f;
        soundTimerDictionary[Sound.Dance] = 0f;
        soundTimerDictionary[Sound.WoodHit] = 0f;
        soundTimerDictionary[Sound.enemyHey] = 0f;
        soundTimerDictionary[Sound.enemyUh] = 0f;
    }
    
    /**
     * CanPlaySound prüft, ob der jeweilige Sound abgespielt werden darf.
     * Dafür wird das Dictionary mit einem Delay genutzt. Das Delay gibt an, wann der Sound erneut abgespielt werden kann.
     * Hier wird der jeweilige Typ des entsprechenden Sound aufgerufen.
     *
     * Sound.Explosion => gibt den Explosions-Sound zurück.
     * 
     */
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
            
            case Sound.Dance:
                return DelayPlaySound(sound, 15f);
            
            case Sound.WoodHit:
                return DelayPlaySound(sound, 1f);
            
            case Sound.enemyHey:
                return DelayPlaySound(sound, 7f);
            
            case Sound.enemyUh:
                return DelayPlaySound(sound, 3f);
        }
    }

    /**
     * DelayPlaySound gibt die Verzögerung an, wann der Sound als nächstes abgespielt werden kann.
     */
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
    
    // Hier wird ein Sound-Gameobject erstellt und zurückgegeben.
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

    /**
     * Gibt den passenden Sound zum übergebenen Typ zurück.
     * Dabei werden alle im Unity-Editor eingefügten Sounds im Prefab "SoundAssets" (im Resource-Ordner) durchlaufen.
     * Der gesuchte Sound wird dann zurückgegeben.
     */
    public static AudioClip GetAudioClip(Sound sound)
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
