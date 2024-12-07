using UnityEngine.UI;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    public Slider volume_slider;
    AudioSource audioSource;
    void Start(){
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        audioSource.volume = volume_slider.value;
    }
}
