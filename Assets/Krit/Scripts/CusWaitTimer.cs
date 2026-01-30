using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CusWaitTimer : MonoBehaviour
{
    [SerializeField] float waitDuration = 5f;
    [SerializeField] GameObject boarder;
    private Image image;
    private bool db = false;
    private bool isActive = false;
    private Vector3 intialScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.reset();
        image = GetComponent<Image>();
        intialScale = image.rectTransform.localScale;
        toggleActive(false);
        //Debug.Log(image.rectTransform.localScale);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (db) return;

            db = true;
            StartTimer();
            Debug.Log("Pressed");
            db = false;
        }
    }

    void FixedUpdate()
    {
    }

    public void StartTimer()
    {
        if(isActive) return;

        toggleActive(true);

        // Reset scale first
        image.rectTransform.localScale = intialScale;
        image.color = Color.green;

        // Kill previous tweens (VERY important)
        LeanTween.cancel(image.gameObject);

        LeanTween.scale(
            image.gameObject,
            new Vector3(0, intialScale.y, intialScale.z),
            waitDuration
        )
        .setEase(LeanTweenType.linear)
        .setOnUpdate((Vector3 scale) =>
        {
            UpdateColor(scale.x);
        })
        .setOnComplete(() =>
        {
            UpdateColor(0f);
            LeanTween.cancel(image.gameObject);
            toggleActive(false);
        });
    }

    public void ReduceTimer(byte value)
    {
        waitDuration -= value;
    }

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

    private void toggleActive(bool value)
    {
        //img.gameObject.SetActive(value);
        //boarder.SetActive(value);
        if (!value)
        {
            image.color = image.color * 0f;
            boarder.GetComponent<Image>().color = Color.white * 0f;
            isActive = false;
        }
        else
        {
            image.fillAmount = 0f;
            isActive = true;
            boarder.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
}
