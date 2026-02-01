using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //  DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip)
    {
        AudioSource audioSource = Instantiate(soundFXObject);
        audioSource.volume = MasterVolume.Volume / 100;
        audioSource.clip = audioClip;

        audioSource.Play();
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);

    }
}