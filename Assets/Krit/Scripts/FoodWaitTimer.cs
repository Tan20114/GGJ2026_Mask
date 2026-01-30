using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;

public class FoodWaitTimer : MonoBehaviour
{
    [SerializeField] GameObject boarder;
    [SerializeField] float waitDuration = 5f;
    private Image img;
    private bool db = false;
    private bool isActive = false;
    private Vector3 intialPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.reset();
        img = GetComponent<Image>();
        intialPos = img.rectTransform.localPosition;

        toggleActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DebugPlay();
    }

    private void FixedUpdate()
    {
        CountDownEndTimer();
    }

    public void StartTimer()
    {
        if (isActive) return;

        toggleActive(true);
        hasJumped = false;

        LeanTween.cancel(img.gameObject);

        LeanTween.value(
            img.gameObject,
            0f,
            1f,
            waitDuration
        )
        .setEase(LeanTweenType.linear)
        .setOnUpdate((float value) =>
        {
            img.fillAmount = value;
            UpdateColor(value);
        })
        .setOnComplete(() =>
        {
            img.fillAmount = 1f;
            UpdateColor(1f);
            LeanTween.cancel(img.gameObject);

            if (!hasJumped)
            {
                hasJumped = true;
                PlayJump();
            }
        });
    }

    #region Color

    [SerializeField] Color start = Color.green;
    [SerializeField] Color end = Color.red;
    private void UpdateColor(float fill)
    {
        float t = fill;

        img.color = Color.Lerp(start, end, t);
    }
    #endregion

    #region Jumping

    private bool hasJumped = false;
    [SerializeField] float jumpHeight = 20f;
    [SerializeField] float jumpDuration = 0.25f;
    private void PlayJump()
    {
        LeanTween.moveLocalY(
            gameObject,
            intialPos.y + jumpHeight,
            jumpDuration
        )
        .setEase(LeanTweenType.easeOutQuad)
        .setOnComplete(() =>
        {
            LeanTween.moveLocalY(
                gameObject,
                intialPos.y,
                jumpDuration
            ).setEase(LeanTweenType.easeInQuad)
            .setOnComplete(() =>
            {
                img.rectTransform.localPosition = intialPos;
                isCountingDown = true;
            });

        });
    }
    #endregion

    private float timer = 0f;
    private bool isCountingDown = false;
    [SerializeField] float DisappearWaitingTime = 0.1f;
    private void CountDownEndTimer()
    {
        if (!isCountingDown) return;

        timer += Time.fixedDeltaTime;
        if (timer >= DisappearWaitingTime)
        {
            toggleActive(false);
            isCountingDown = false;
            timer = 0f;
        }
    }

    private void toggleActive(bool value)
    {
        //img.gameObject.SetActive(value);
        //boarder.SetActive(value);
        if (!value)
        {
            img.color = img.color * 0f;
            boarder.GetComponent<Image>().color = Color.white * 0f;
            isActive = false;
        } else
        {
            img.fillAmount = 0f;
            isActive = true;
            boarder.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    private void DebugPlay()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (db) return;

            db = true;
            StartTimer();
            Debug.Log("Pressed");
            db = false;
        }
    }
}
