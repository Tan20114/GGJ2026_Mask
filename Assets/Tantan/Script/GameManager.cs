using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    CusWaitTimer customerTimer => FindAnyObjectByType<CusWaitTimer>();
    CharacterSpawner spawner => FindAnyObjectByType<CharacterSpawner>();

    public int winCount = 0;
    public int money = 0;
    [SerializeField] int bargainMoney = 15000;
    [Range(0,3)][SerializeField] int lifePoint = 3;
    public int LifePoint
    {
        get => lifePoint;
        set => lifePoint = Mathf.Clamp(value,0,3);
    }

    bool isTimerStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //customerTimer.DebugPlay();

        if (spawner.currentCharacter.currentState == CharacterState.Ordering && !isTimerStart)
        {
            isTimerStart = true;
            customerTimer.StartTimer();
        }

        if (Input.GetKeyDown(KeyCode.O))
            EndServing(true);

        if (customerTimer.IsTimeOut() && isTimerStart)
            EndServing(false);
    }

    public void EndServing(bool isSuccess)
    {
        if (spawner.currentCharacter.currentState != CharacterState.Ordering) return;

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
    }
}
