using TMPro;
using UnityEngine;
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
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440),
        new Vector2Int(3840, 2160)
    };

    private int index = 0;
    private int maxIndex;
    private Vector2Int masRes;

    protected override void Start()
    {
        base.Start();
        UpdateText();
        masRes = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
        for (int i = 0; i < resolutions.Length - 1; i++)
        {
            if (resolutions[i].x == Screen.currentResolution.width && resolutions[i].y == Screen.currentResolution.height)
            {
                maxIndex = i;
                index = i;
                break;
            }
        }
        //Debug.Log("Resolution" + masRes);
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
        Screen.SetResolution(r.x, r.y, Screen.fullScreen);
        UpdateText();
    }

    void UpdateText()
    {
        TMPtext.text = "Resolution : " + resolutions[index].x + " x " + resolutions[index].y;
    }

    
}
