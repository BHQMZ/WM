using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private void Awake()
    {
        GameObject = gameObject;
    }

    private static GameObject GameObject;

    public static void MusicVolume(float volume)
    {
        if (GameObject == null)
        {
            Debug.LogError("…˘“Ùπ‹¿Ì∆˜Œ¥π“‘ÿ");
            return;
        }

        AudioSource source = GameObject.GetComponent<AudioSource>();
        if (source != null)
        {
            source.volume = volume;
        }
    }

    public static void PlayMusic(bool isOpen)
    {
        if (GameObject == null)
        {
            Debug.LogError("…˘“Ùπ‹¿Ì∆˜Œ¥π“‘ÿ");
            return;
        }

        AudioSource source = GameObject.GetComponent<AudioSource>();
        if (source != null)
        {
            if (isOpen)
            {
                source.Play();
            }
            else
            {
                source.Stop();
            }
        }
    }

    //public static bool IsPlayMusic()
    //{
    //    if (GameObject == null)
    //    {
    //        Debug.LogError("…˘“Ùπ‹¿Ì∆˜Œ¥π“‘ÿ");
    //        return false;
    //    }

    //    AudioSource source = GameObject.GetComponent<AudioSource>();
    //    if (source != null)
    //    {
    //        source.    
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
