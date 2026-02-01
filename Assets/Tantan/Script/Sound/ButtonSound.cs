using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] AudioClip button;

    public void PlayButtonSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(button);
    }
}
