using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarationScript : MonoBehaviour
{
    public AudioClip [] narrations;
    public Slider speech_volume;
    // [Range(0f, 1f)] float volume;
    
    private AudioSource audioSource;
    private int count = 0;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(){
        audioSource.volume = speech_volume.value;
        audioSource.clip = narrations[count];
        audioSource.Play();
    }
}
