using UnityEngine;

public class MenuSound : MonoBehaviour
{
    public AudioClip buttonClickSound;

    public void PlayButtonClickSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonClickSound);
    }
}
