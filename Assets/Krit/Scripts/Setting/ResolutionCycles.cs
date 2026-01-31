using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionCycles : HoverChecker
{
    Vector2Int[] resolutions =
    {
        new Vector2Int(1024, 768),
        new Vector2Int(1280, 1024),
        new Vector2Int(1280, 720),
        new Vector2Int(1366, 800),
        new Vector2Int(1366, 768),
        new Vector2Int(1440, 900),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440)
    };

    int index = 0;

    protected override void Start()
    {
        base.Start();
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
        index = (index + 1) % resolutions.Length;
        Apply();
    }

    public void Prev()
    {
        index--;
        if (index < 0) index = resolutions.Length - 1;
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
