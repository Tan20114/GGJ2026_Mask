using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Viisualizer : MonoBehaviour
{
    CharacterSpawner spawner => FindAnyObjectByType<CharacterSpawner>();

    [SerializeField] Image cookingSpriteRenderer;
    [SerializeField] Sprite[] cookingSprites;

    [SerializeField] PostItPopup postItFirst;

    [SerializeField] Animator[] hearts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        postItFirst.OpenAndClose();
    }

    // Update is called once per frame
    void Update()
    {
        CookingVisual();
        LifeVisual();
    }

    void CookingVisual()
    {
        if (GameManager.Instance.foodTimer == null) return;

        cookingSpriteRenderer.sprite = cookingSprites[!(GameManager.Instance.foodTimer.CurrentState == FoodWaitTimer.State.cooking) ? 0:1];
    }

    void LifeVisual()
    {
        int alert = 3 - GameManager.Instance.LifePoint;

        if (alert > 0)
        {
            hearts[alert - 1].SetBool("isBeep", true);
        }
        else
        {
            foreach (var heart in hearts)
            {
                heart.SetBool("isBeep", false);
            }
        }
    }

    public void StartGame() => spawner.isStart = true;
}
