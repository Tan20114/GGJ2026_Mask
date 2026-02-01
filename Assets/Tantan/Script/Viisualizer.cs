using TMPro;
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

    [SerializeField] TextMeshProUGUI servedCountTxt;

    [SerializeField] Sprite[] endTemplate;
    [SerializeField] Image endImage;
    [SerializeField] GameObject endScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        postItFirst.OpenAndClose();
    }

    // Update is called once per frame
    void Update()
    {
        CountServed();
        CookingVisual();
        LifeVisual();
        ShowEnd();
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

    void CountServed() => servedCountTxt.text = GameManager.Instance.winCount.ToString();

    public void StartGame() => spawner.isStart = true;
    public void EndGame() => spawner.isStart = false;

    void ShowEnd()
    {
        if (!GameManager.Instance.isGameOver) return;
        if (endScreen.activeSelf) return;

        EndGame();

        endImage.sprite = endTemplate[GameManager.Instance.isPoliced ? 1 : 0];
        endScreen.SetActive(true);
    }
}
