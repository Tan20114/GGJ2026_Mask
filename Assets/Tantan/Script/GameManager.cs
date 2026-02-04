using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public enum Product
{
    None,
    Chicken,
    Beef,
    Lamb,
    Illegal
}

public class GameManager : Singleton<GameManager>
{
    SceneLoader sl => FindAnyObjectByType<SceneLoader>();
    public CusWaitTimer customerTimer => FindAnyObjectByType<CusWaitTimer>();
    public FoodWaitTimer foodTimer => FindAnyObjectByType<FoodWaitTimer>();
    FoodManager fm => FindAnyObjectByType<FoodManager>();
    CharacterSpawner spawner => FindAnyObjectByType<CharacterSpawner>();
    CamMover mover => FindAnyObjectByType<CamMover>();

    public int winCount = 0;
    public int money = 0;
    [SerializeField] int bargainMoney = 15000;
    [Range(0,3)][SerializeField] int lifePoint = 3;
    public bool isPoliced = false;
    public bool isGameOver = false;

    public Product currentProduct = Product.None;
    public Product productToSell = Product.None;


    public int LifePoint
    {
        get => lifePoint;
        set => lifePoint = Mathf.Clamp(value,0,3);
    }

    bool isTimerStart = false;

    // Update is called once per frame
    void Update()
    {
        //customerTimer.DebugPlay();
        if (!spawner.currentCharacter) return;

        DifficultyCurve();
        GameOver();

        if (spawner.currentCharacter.currentState == CharacterState.Ordering && !isTimerStart)
        {
            isTimerStart = true;
            customerTimer.StartTimer();
            int ranVal = Random.Range(0,4);

            if (currentProduct != Product.None) return;

            currentProduct = ranVal switch
            {
                0 => Product.Chicken,
                1 => Product.Beef,
                2 => Product.Lamb,
                _ => Product.Chicken
            };
            
            if (spawner.currentCharacter.role == CharacterRole.Gang)
                currentProduct = Product.Illegal;

            spawner.currentCharacter.SentenceSelector(currentProduct);
        }

        if (customerTimer.IsTimeOut() && isTimerStart)
            EndServing(false);

        if (fm.SereveFood())
            Serve();
    }

    public void EndServing(bool isSuccess)
    {
        if (spawner.currentCharacter.currentState != CharacterState.Ordering) return;

        mover.MoveToDefaultCam();

        customerTimer.CurrentState = CusWaitTimer.CusWaitState.finished;
        spawner.currentCharacter.currentState = CharacterState.MovingOut;
        if (isSuccess)
        {
            winCount++;

            if(spawner.currentCharacter.role == CharacterRole.Normie || spawner.currentCharacter.role == CharacterRole.Police)
                money += 1000;
            else if (spawner.currentCharacter.role == CharacterRole.Gang)
                money += 2500;

            customerTimer.Served();
        }
        else
        {
            Debug.Log("Customer left due to timeout");
            lifePoint--;
        }

        isTimerStart = false;
        fm.CancelCooking();
        productToSell = Product.None;
        currentProduct = Product.None;
    }

    public void MissOrder(bool isIllegal)
    {
        mover.MoveToDefaultCam();

        customerTimer.CurrentState = CusWaitTimer.CusWaitState.finished;
        customerTimer.CancleTimer();
        spawner.currentCharacter.currentState = CharacterState.MovingOut;
        if (spawner.currentCharacter.role == CharacterRole.Police && isIllegal)
        {

            if (money >= bargainMoney)
            {
                Debug.Log("Bribed the Police! Money Decreased.");
                money -= bargainMoney;
            }
            else
            {
                Debug.Log("Caught by Police! Game Over!");
                lifePoint = 0;
            }

        }
        else
        {
            Debug.Log("Missed Order! Life Point Decreased.");
            lifePoint--;
        }

        isTimerStart = false;
        fm.CancelCooking();
        productToSell = Product.None;
        currentProduct = Product.None;
    }

    void DifficultyCurve()
    {
        if (winCount % 10 == 0 && winCount != 0)
        {
            customerTimer.SetTimer(25 - (int)(winCount / 10));
        }
    }

    void GameOver()
    {
        if (lifePoint <= 0)
            isGameOver = true;
    }

    void Serve()
    {
        if (productToSell == currentProduct)
            EndServing(true);
        else if (productToSell != currentProduct)
        {
            if(productToSell == Product.Illegal)
                MissOrder(true);
            else
                MissOrder(false);
        }
    }

    public void CancelCooking() => productToSell = Product.None;
}
