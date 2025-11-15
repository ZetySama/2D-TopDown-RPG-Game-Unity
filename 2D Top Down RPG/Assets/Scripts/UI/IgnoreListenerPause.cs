using UnityEngine;

// Bu script'in olduðu objeye bir AudioSource bileþeni eklemeyi zorunlu kýlar
[RequireComponent(typeof(AudioSource))]
public class IgnoreListenerPause : MonoBehaviour
{
    // Script çalýþýr çalýþmaz (Oyun baþlar baþlamaz)
    void Awake()
    {
        // 1. Bu objenin üzerindeki AudioSource bileþenini bul
        AudioSource myAudioSource = GetComponent<AudioSource>();

        // 2. O kutucuðun kod karþýlýðý olan "ignoreListenerPause" özelliðini 'true' yap
        myAudioSource.ignoreListenerPause = true;
    }
}