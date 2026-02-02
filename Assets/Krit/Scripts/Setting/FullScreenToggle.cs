using System;
using TMPro;
using UnityEngine;

public class FullScreenToggle : HoverChecker
{
    private bool isFullscreen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        LeanTween.reset();
        if(PlayerPrefs.HasKey("isFullScreen"))
        {
            isFullscreen = PlayerPrefs.GetInt("isFullScreen") == 1 ? true : false;
        }
        else
        {
            isFullscreen = true;
            SaveToPlayerPrefs();
        }
        Screen.fullScreen = isFullscreen;

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (IPI)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                ToggleFullScreen();
            }
        }
    }

    void UpdateText()
    {
        var text = isFullscreen ? "on" : "off";
        TMPtext.text = "FullScreen : " + text;
    }

    public void ToggleFullScreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        SaveToPlayerPrefs();
        UpdateText();
    }

    private void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt("isFullScreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveToPlayerPrefs();
    }
}
