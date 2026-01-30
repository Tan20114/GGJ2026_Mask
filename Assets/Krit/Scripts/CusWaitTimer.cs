using UnityEngine;
using UnityEngine.UI;

public class CusWaitTimer : MonoBehaviour
{
    [SerializeField] float WaitDuration = 5f;
    private float timer = 0f;
    private Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        Debug.Log(image.rectTransform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetImageSize(image.rectTransform.localScale.x - 0.1f, image.rectTransform.localScale.y);
        }
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= WaitDuration)
        {
            SetImageSize(1f, 1f);
            timer = 0f;
        }
    }

    private void SetImageSize(float width, float height)
    {
        image.rectTransform.localScale = new Vector2(width, height);
        Debug.Log(image.rectTransform.localScale);
    }

    public void ReduceTimer()
    {
        WaitDuration -= 1f;
    }
}
