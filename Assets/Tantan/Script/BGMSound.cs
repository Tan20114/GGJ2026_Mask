using UnityEngine;

public class BGMSound : MonoBehaviour
{
    AudioSource audioSource => GetComponent<AudioSource>();

    private void Update()
    {
        audioSource.volume = MasterVolume.Volume / 100;
    }
}
