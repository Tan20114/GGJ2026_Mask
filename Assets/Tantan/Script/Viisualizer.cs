using UnityEngine;
using UnityEngine.UI;

public class Viisualizer : MonoBehaviour
{
    [SerializeField] Image cookingSpriteRenderer;
    [SerializeField] Sprite[] cookingSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CookingVisual();
    }

    void CookingVisual()
    {
        if (GameManager.Instance.foodTimer == null) return;

        cookingSpriteRenderer.sprite = cookingSprites[!(GameManager.Instance.foodTimer.CurrentState == FoodWaitTimer.State.cooking) ? 0:1];
    }
}
