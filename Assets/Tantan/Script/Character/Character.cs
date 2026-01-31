using UnityEngine;

public enum CharacterState
{
    MovingIn,
    MovingOut,
    Ordering
}

public enum CharacterRole
{
    Normie,
    Gang,
    Police
}

public class Character : MonoBehaviour
{
    Rigidbody2D rb => GetComponent<Rigidbody2D>();

    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float stepHeight = 0.08f;
    [SerializeField] float stepFrequency = 6f;

    float stepTime;
    float baseY;

    [SerializeField] CharacterState currentState = CharacterState.MovingIn;
    public CharacterRole role = CharacterRole.Normie;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseY = rb.position.y;
    }

    private void Update()
    {
        if (transform.position.x <= 0 && currentState != CharacterState.MovingOut)
            currentState = CharacterState.Ordering;

        if (Input.GetKeyDown(KeyCode.O) && currentState == CharacterState.Ordering)
        {
            currentState = CharacterState.MovingOut;
            GameManager.Instance.winCount++;
        }

        if (Input.GetKeyDown(KeyCode.P))
            currentState = CharacterState.MovingOut;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveIn();
        MoveOut();
    }

    void MoveIn()
    {
        if (currentState != CharacterState.MovingIn) return;
    
        stepTime += Time.fixedDeltaTime;
    
        float wave = Mathf.Sin(stepTime * stepFrequency);
        wave = wave * Mathf.Abs(wave); 
    
        float y = baseY + wave * stepHeight;
        float x = rb.position.x - walkSpeed * Time.fixedDeltaTime;
    
        rb.MovePosition(new Vector2(x, y));
    }

    void MoveOut()
    {
        if (currentState != CharacterState.MovingOut) return;

        stepTime += Time.fixedDeltaTime;

        float wave = Mathf.Sin(stepTime * stepFrequency);
        wave = wave * Mathf.Abs(wave);

        float y = baseY + wave * stepHeight;
        float x = rb.position.x - walkSpeed * Time.fixedDeltaTime;

        rb.MovePosition(new Vector2(x, y));
    }
}
