using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PostItPopup : MonoBehaviour
{
    [SerializeField] GameObject postit;
    [SerializeField] GameObject BG;
    [SerializeField] Outline outline;
    [SerializeField] GameObject text;

    private bool isActive = false;

    [SerializeField] Volume globalVolume;
    private DepthOfField dof;

    [SerializeField] float foucsDistance = 1f;

    [Header("Animation")]
    [SerializeField] Transform targetTrans;
    [SerializeField] LeanTweenType easeType = LeanTweenType.linear;
    [SerializeField] float scaleSize = 15f;
    [SerializeField] float animDuration = 1f;
    private Vector3 intialPos;
    private Vector3 intialScale;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.reset();
        intialPos = postit.transform.localPosition;
        intialScale = postit.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenAndClose()
    {
        isActive = !isActive;

        postit.SetActive(!postit.activeSelf);
        BG.SetActive(!BG.activeSelf);
        text.SetActive(!text.activeSelf);
        PopAnimation();
    }
    
    public void ToggleBackgroundUI()
    {
        
        if (globalVolume.profile.TryGet(out dof))
        {
            dof.active = isActive;
            dof.focusDistance.value = foucsDistance;
        }
    }

    public void PopAnimation()
    {
        if (isActive)
        {
            LeanTween.move(postit, targetTrans, animDuration).setEase(easeType);
            LeanTween.scale(postit, postit.transform.localScale * scaleSize, animDuration).setEase(easeType);
        } else
        {
            LeanTween.cancel(postit);
            postit.transform.localPosition = intialPos;
            postit.transform.localScale = intialScale;
        }
    }

    public void ToggleOutline(bool state)
    {
        outline.enabled = state;
    }
}
