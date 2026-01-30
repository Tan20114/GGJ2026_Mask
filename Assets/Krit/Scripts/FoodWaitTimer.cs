using UnityEngine;
using UnityEngine.UI;

public class FoodWaitTimer : MonoBehaviour
{
    [SerializeField] float waitDuration = 5f;
    private Image img;
    private bool db = false;
    private Vector3 intialScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.reset();
        img = GetComponent<Image>();
        intialScale = img.rectTransform.localScale;
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

    public void StartTimer()
    {
        img.fillAmount = 1f;
        //img.color = Color.green;

        LeanTween.cancel(img.gameObject);

        LeanTween.value(
            img.gameObject,
            1f,
            0f,
            waitDuration
        )
        .setEase(LeanTweenType.linear);
    }
}
