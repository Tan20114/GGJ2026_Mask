using System;
using System.Xml;
using TMPro;
using UnityEngine;

public class HoverChecker : MonoBehaviour
{
    private bool isPointerIn;
    public bool IPI => isPointerIn;
    [SerializeField] protected TextMeshProUGUI TMPtext;

    private Vector3 intialScale;
    private Color intialColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        intialScale = TMPtext.transform.localScale;
        intialColor = TMPtext.color;
        //Debug.Log(TMPtext.transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        //CheckMouseClick();
    }

    #region Pointer Events
    public void OnPointerEnter()
    {
        isPointerIn = true;
        UpScale();
        ChangeColor();
    }

    public void OnPointerExit()
    {
        isPointerIn = false;
        DownScale();
        ResetColor();
    }

    public void CheckMouseClick()
    {
        if (!isPointerIn) return;
        if (Input.GetMouseButtonDown(0)) Debug.Log("Pressed left click.");
        if (Input.GetMouseButtonDown(1)) Debug.Log("Pressed right click.");
        if (Input.GetMouseButtonDown(2)) Debug.Log("Pressed middle click.");
    }
    #endregion

    #region Hover Animation
    [Header("LeanTween Settings")]
    [SerializeField] float scale = 2;
    [SerializeField] float duration = 0.5f;
    [SerializeField] LeanTweenType easeType = LeanTweenType.linear;
    private void UpScale()
    {
        
        //TMPtext.fontSize += scale;
        LeanTween.scale(TMPtext.gameObject, new Vector3(scale, scale), duration).setEase(easeType);  
    }
    private void DownScale()
    {
        LeanTween.cancel(TMPtext.gameObject);
        TMPtext.transform.localScale = intialScale;
        //Debug.Log(intialSize);
    }
    #endregion

    #region Color Change
    private void ChangeColor()
    {
        TMPtext.color = Color.yellow;
    }

    private void ResetColor()
    {
        TMPtext.color = intialColor;
    }
    #endregion
}

