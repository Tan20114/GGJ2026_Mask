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
    CusWaitTimer customerTimer => FindAnyObjectByType<CusWaitTimer>();
    FoodWaitTimer foodTimer => FindAnyObjectByType<FoodWaitTimer>();
    CharacterSpawner spawner => FindAnyObjectByType<CharacterSpawner>();
    CamMover mover => FindAnyObjectByType<CamMover>();

    public int winCount = 0;
    public int money = 0;
    [SerializeField] int bargainMoney = 15000;
    [Range(0,3)][SerializeField] int lifePoint = 3;

    public Product currentProduct;
    public Product productToSell;

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

        if (spawner.currentCharacter.currentState == CharacterState.Ordering && !isTimerStart)
        {
            isTimerStart = true;
            customerTimer.StartTimer();
            int ranVal = Random.Range(0,4);

            currentProduct = ranVal switch
            {
                0 => Product.Chicken,
                1 => Product.Beef,
                2 => Product.Lamb,
                _ => Product.Chicken
            };
            
            if (spawner.currentCharacter.role == CharacterRole.Gang)
                currentProduct = Product.Illegal;
        }

        if (customerTimer.IsTimeOut() && isTimerStart)
            EndServing(false);

        if (foodTimer.FinishCook())
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
    }

    public void MissOrder(bool isIllegal)
    {
        if (spawner.currentCharacter.role == CharacterRole.Police && isIllegal)
        {
            if(money >= bargainMoney)
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
