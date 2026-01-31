using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class FoodManager : MonoBehaviour
{
    //[SerializeField] List<GameObject> foodItems;
    private GameObject SelectedFood;
    private bool haveSelected = false;

    [Header("Cooking Element")]
    [SerializeField] FoodWaitTimer FoodCounter;
    private bool isCooking = false;
    [NonSerialized] public SereveState CurrentState;

    [Header("Hover Animation")]
    private GameObject holderObj;
    [SerializeField] LeanTweenType easeType = LeanTweenType.easeInOutSine;
    [SerializeField] float scaleSize = 1.1f;
    [SerializeField] float animDuration = 0.3f;
    //[SerializeField] Image outline;
    private Vector3 intialScale;
    private Vector3 intialPos;

    public enum SereveState
    {
        notReady,
        Ready
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentState = SereveState.notReady;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFoodFinish();
    }

    public void SelectObject(GameObject item)
    {
        if (isCooking) return;
        if (SelectedFood != null) SelectedFood.transform.GetChild(0).gameObject.SetActive(false);

        if (SelectedFood == item)
        {
            UnSelectObject();
        }
        else if (SelectedFood != item)
        {
            SelectedFood = item;
            haveSelected = true;
        }

        if (SelectedFood != null) SelectedFood.transform.GetChild(0).gameObject.SetActive(haveSelected);
        //Debug.Log("Selected Food: " + SelectedFood);
        //Debug.Log("Have Selected?: " + haveSelected);
    }

    private void UnSelectObject()
    {
        if (SelectedFood == null) return;
        SelectedFood.transform.GetChild(0).gameObject.SetActive(false);
        haveSelected = false;
        SelectedFood = null;
    }

    #region Sereve and Cook Food
    public void CookItem()
    {
        if (haveSelected)
        {
            FoodCounter.StartTimer();
            isCooking = true;
        }
    }

    public void CancelCooking()
    {
        UnSelectObject();
        if(isCooking)
        {
            FoodCounter.CancleTimer();
            isCooking = false;
        }
    }

    public void CheckFoodFinish()
    {
        if (isCooking)
        {
            if (FoodCounter.FinishCook())
            {
                isCooking = false;
                CurrentState = SereveState.Ready;
                UnSelectObject();
            }
        }
    }
    #endregion

    #region Hover Animation
    public void Hover(GameObject obj)
    {
        if (holderObj != obj)
        {
            if(holderObj != null)
            {
                // Reset previous object
                LeanTween.cancel(holderObj);
                holderObj.transform.localPosition = intialPos;
                holderObj.transform.localScale = intialScale;
            }

            holderObj = obj;
            intialPos = obj.transform.localPosition;
            intialScale = obj.transform.localScale;
            LeanTween.scale(obj, obj.transform.localScale * scaleSize, animDuration).setEase(easeType);
        }
    }

    public void UnHover(GameObject obj)
    {
        if (holderObj == obj)
        {
            LeanTween.cancel(holderObj);
            obj.transform.localPosition = intialPos;
            obj.transform.localScale = intialScale;
            holderObj = null;
        }
    }
    #endregion
}
