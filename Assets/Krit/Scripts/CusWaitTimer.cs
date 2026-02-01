using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CusWaitTimer : MonoBehaviour
{
    [SerializeField] bool debugMode = false;
    [SerializeField] float waitDuration = 5f;
    [SerializeField] GameObject boarder;
    private Image image;
    private bool db = false;
    private bool isShinking = false;
    private bool isFillbarRunOut = false;
    //private bool isActive = false;
    private Vector3 intialScale, intialBoarderScale;

    [NonSerialized] public CusWaitState CurrentState;

    [Header("Particles")]
    [SerializeField] private ParticleSystem finishParticle1;
    [SerializeField] private ParticleSystem finishParticle2;

    public enum CusWaitState
    {
        idle,
        loading,
        finished,
        timeout
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.reset();
        image = GetComponent<Image>();
        intialScale = image.rectTransform.localScale;
        intialBoarderScale = boarder.transform.localScale;
        CurrentState = CusWaitState.idle;
        toggleActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode) DebugPlay();

        //Debug.Log(CurrentState);
    }

    void FixedUpdate()
    {
    }
    #region Timer
    [Header("Timer")]
    [SerializeField] float targetScale = 0.6f;
    private float scaleOffset = 0.05f;
    public void StartTimer()
    {
        if(CurrentState == CusWaitState.loading) return;

        CurrentState = CusWaitState.loading;
        toggleActive(true);

        // Kill previous tweens (VERY important)
        LeanTween.cancel(image.gameObject);
        LeanTween.cancel(boarder.gameObject);

        // Reset scale first
        image.rectTransform.localScale = intialScale;
        image.color = Color.green;

        boarder.transform.localScale = intialBoarderScale;
        boarder.GetComponent<Image>().color = new Color(1, 1, 1, 1);


        LeanTween.scale(
            image.gameObject,
            new Vector3(0, intialScale.y, intialScale.z),
            waitDuration
        )
        .setEase(LeanTweenType.linear)
        .setIgnoreTimeScale(true)
        .setOnUpdate((Vector3 scale) =>
        {
            UpdateColor(scale.x);
            if (!isShinking && scale.x <= targetScale)
            {
                isShinking = true;
                ShrinksBoarder();
            }
            if (scale.x <= scaleOffset && !isFillbarRunOut)
            {
                OnFillbarRunOut();
                //Debug.Log("Force Bar End");
            }
        });
    }

    private void ShrinksBoarder()
    {

        LeanTween.cancel(boarder);

        var time = targetScale * waitDuration;

        LeanTween.scale(
            boarder,
            new Vector3(0, intialBoarderScale.y, intialBoarderScale.z),
            time + 0.1f
        )
        .setEase(LeanTweenType.easeInQuart)
        .setOnComplete(() =>
        {
            LeanTween.cancel(boarder);
            toggleActive(false);
        });
    }

    private void ShrinksBoarder(float duration)
    {

        LeanTween.cancel(boarder);

        var time = targetScale * waitDuration;

        LeanTween.scale(
            boarder,
            new Vector3(0, intialBoarderScale.y, intialBoarderScale.z),
            duration
        )
        .setEase(LeanTweenType.easeInQuart)
        .setOnComplete(() =>
        {
            LeanTween.cancel(boarder);
            toggleActive(false);
        });
    }

    private void OnFillbarRunOut()
    {
        isFillbarRunOut = true;
        UpdateColor(0f);
        LeanTween.cancel(image.gameObject);
        CurrentState = CusWaitState.timeout;
        image.transform.localScale = new Vector3(0, intialScale.y, intialScale.z);
        //Debug.Log("Time out");
    }

    public void ReduceTimer(float value)
    {
        waitDuration -= value;
    }
    public void SetTimer(float value)
    {
        waitDuration = value;
    }
    public bool IsTimeOut()
    {
        if (CurrentState != CusWaitState.timeout) return false;
        CurrentState = CusWaitState.idle;
        return true;
    }
    public void Served()
    {
        if(CurrentState != CusWaitState.finished) return;
        LeanTween.cancel(image.gameObject);
        LeanTween.cancel(boarder);

        image.color = start;

        LeanTween.scale(
            image.gameObject,
            new Vector3(intialScale.x, intialScale.y, intialScale.z),
            0.25f
        )
        .setEase(LeanTweenType.linear)
        .setOnComplete(() =>
        {
            PlayFinishParticle();
            LeanTween.cancel(image.gameObject);

            LeanTween.scale(
            image.gameObject,
            new Vector3(intialScale.x, 0, intialScale.z),
            0.25f
            )
            .setEase(LeanTweenType.linear)
            .setOnComplete(() =>
            {
                CurrentState = CusWaitState.idle;
                LeanTween.cancel(image.gameObject);
                toggleActive(false);
            });
        });

        LeanTween.scale(
        boarder,
        new Vector3(intialBoarderScale.x, intialBoarderScale.y, intialBoarderScale.z),
        0.25f
        )
        .setEase(LeanTweenType.linear)
        .setOnComplete(() =>
        {
            LeanTween.cancel(boarder);
            LeanTween.scale(
            boarder,
            new Vector3(intialBoarderScale.x, 0, intialBoarderScale.z),
            0.25f
            )
            .setEase(LeanTweenType.linear)
            .setOnComplete(() =>
            {
                LeanTween.cancel(boarder);
            });
        });
    }

    private float cancleDuration = 0.2f;
    public void CancleTimer()
    {
        if (CurrentState == CusWaitState.idle) return;
        CurrentState = CusWaitState.idle;
        LeanTween.cancel(image.gameObject);
        LeanTween.cancel(boarder);
        toggleActive(true);
        image.color = end;

        isShinking = true;
        ShrinksBoarder(cancleDuration + 0.1f);

        LeanTween.scale(
            image.gameObject,
            new Vector3(0, intialBoarderScale.x, intialBoarderScale.z),
            cancleDuration
            )
            .setEase(LeanTweenType.linear)
            .setOnUpdate((Vector3 scale) =>
            {
                if (scale.x <= scaleOffset && !isFillbarRunOut)
                {
                    OnFillbarRunOut();
                    //Debug.Log("Force Bar End");
                }
            });
    }

    #endregion

    #region Color
    [Header("Color Settings")]
    [SerializeField] Color start = Color.green;
    [SerializeField] Color mid = Color.yellow;
    [SerializeField] Color end = Color.red;
    private void UpdateColor(float scaleX)
    {
        // scaleX goes from 1 
        float t = 1f - scaleX; // 0 

        

        if (t < 0.5f)
        {
            // Green
            image.color = Color.Lerp(start, mid, t * 2f);
        }
        else
        {
            // Yellow
            image.color = Color.Lerp(mid, end, (t - 0.5f) * 2f);
        }
    }
    #endregion

    private void toggleActive(bool value)
    {
        //img.gameObject.SetActive(value);
        //boarder.SetActive(value);
        //Debug.Log("toggled : " + value);
        if (!value)
        {
            image.color = image.color * 0f;
            boarder.GetComponent<Image>().color = Color.white * 0f;
            //isShinking = false;
        }
        else
        {
            isShinking = false;
            isFillbarRunOut = false;
            image.fillAmount = 0f;
            
        }
    }

    void PlayFinishParticle()
    {
        if (finishParticle1 == null || finishParticle2 == null) return;

        finishParticle1.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        finishParticle1.Play();
        finishParticle2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        finishParticle2.Play();
    }
    
    public void DebugPlay()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (db) return;

            db = true;
            StartTimer();
            //Debug.Log("Pressed");
            db = false;
        }

        if (CurrentState == CusWaitState.loading)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                CurrentState = CusWaitState.finished;
                Served();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CancleTimer();
            }
        }
    }
}
