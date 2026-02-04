using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class MasterVolume : HoverChecker
{
    ButtonSound bs => FindAnyObjectByType<ButtonSound>();

    private static byte _volume = 100;
    public static byte Volume {
        get => _volume;
        set => _volume = (byte)Mathf.Clamp(value, 0, 100);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            Volume = (byte)PlayerPrefs.GetInt("MasterVolume");
        }
        else
        {
            Volume = 100;
            PlayerPrefs.SetInt("MasterVolume", Volume);
            PlayerPrefs.Save();
        }

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (IPI)
        {
            if (Input.GetMouseButtonDown(0))
            {
                IncVolume();
            }
            if (Input.GetMouseButtonDown(1))
            {
                DesVolume();
                bs.PlayButtonSound();
            }
        }
    }

    private void UpdateText()
    {
        TMPtext.text = "master volume : " + Volume + "%";
    }

    private void IncVolume()
    {
        if (Volume == 100)
        {
            Volume = 0;
        }
        else
        {
            Volume += 5;
        }
        SaveToPlayerPrefs();
        UpdateText();
    }

    private void DesVolume()
    {
        if (Volume == 0)
        {
            Volume = 100;
        }
        else
        {
            Volume -= 5;
        }
        SaveToPlayerPrefs();
        UpdateText();
    }

    private void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt("MasterVolume", Volume);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveToPlayerPrefs();
    }
}
