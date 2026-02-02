using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ResolutionCycles : HoverChecker
{
    Vector2Int[] resolutions =
    {
        new Vector2Int(640, 360),
        new Vector2Int(854, 480),
        new Vector2Int(960, 540),
        new Vector2Int(1024, 576),
        new Vector2Int(1280, 720),
        new Vector2Int(1360, 768),
        new Vector2Int(1366, 768),
        new Vector2Int(1440, 900),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440)
    };

    private int index = 0;
    private int maxIndex;
    private Vector2Int maxRes;

    protected override void Start()
    {
        base.Start();
        maxRes = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
        //maxRes = new Vector2Int(699, 256);

        if(PlayerPrefs.HasKey("ResolutionIndex") && PlayerPrefs.HasKey("MaxResolutionIndex"))
        {
            index = PlayerPrefs.GetInt("ResolutionIndex");
            maxIndex = PlayerPrefs.GetInt("MaxResolutionIndex");
            //Apply();
        }
        else
        {
            CheckForResolution();
        }
        //Debug.Log("Resolution" + maxRes);
        //Debug.Log("Max Index : " + maxIndex + "/ index :" + index);
        UpdateText();
    }

    void Update()
    {
        //Debug.Log(hoverChecker.isPointerIn);
        if (IPI)
        {
            //Debug.Log("IN");
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Left");
                Next();
            }
            if (Input.GetMouseButtonDown(1))
            {
                //Debug.Log("Right");
                Prev();
            }
        }
    }

    public void Next()
    {
        index++;
        if (index > maxIndex) index = 0;
        Apply();
    }

    public void Prev()
    {
        index--;
        if (index < 0) index = maxIndex;
        Apply();
    }

    void Apply()
    {
        var r = resolutions[index];
        SaveToPlayerPrefs();
        UpdateText();
    }

    void UpdateText()
    {
        TMPtext.text = "Resolution : " + resolutions[index].x + " x " + resolutions[index].y;
    }

    private void CheckForResolution()
    {
        for (int i = 0; i < resolutions.Length - 1; i++)
        {
            if (resolutions[i].x == maxRes.x && resolutions[i].y == maxRes.y)
            {
                maxIndex = i;
                index = i;
                break;
            }
        }
        if (resolutions[index].x != maxRes.x || resolutions[index].y != maxRes.y)
        {
            CheckOutOfBoundRes();
        }
    }

    private void CheckOutOfBoundRes()
    {
        Debug.Log("Player Don't have resolution in preset");
        for (int i = resolutions.Length - 1; i >= 0; i--)
        {
            Debug.Log($"Checking index {i}: {resolutions[i].x}x{resolutions[i].y}");
            if (maxRes.x >= resolutions[i].x &&
                maxRes.y >= resolutions[i].y)   
            {
                maxIndex = i;
                index = i;
                SaveToPlayerPrefs();
                break;
            }
        }
    }

    private void SaveToPlayerPrefs()
    {
        if (PlayerPrefs.GetInt("MaxResolutionIndex") != maxIndex)
        {
            PlayerPrefs.SetInt("MaxResolutionIndex", maxIndex);
        }
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveToPlayerPrefs();
    }
}
