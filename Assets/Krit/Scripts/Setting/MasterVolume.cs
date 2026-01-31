using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class MasterVolume : HoverChecker
{
    private byte _volume = 100;
    public byte Volume {
        get => _volume;
        set => _volume = (byte)Mathf.Clamp(value, 0, 100);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
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
            }
        }
    }

    private void UpdateText()
    {
        TMPtext.text = "master volume : " + Volume + "%";
    }

    private void IncVolume()
    {
        Volume += 5;
        UpdateText();
    }

    private void DesVolume()
    {
        Volume -= 5;
        UpdateText();
    }
}
