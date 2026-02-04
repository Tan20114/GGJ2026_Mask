using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

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
    [Header("Reference")]
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] Dialog dialogData;

    [Header("Status")]
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float stepHeight = 0.08f;
    [SerializeField] float stepFrequency = 6f;

    float stepTime;
    float baseY;

    public CharacterState currentState = CharacterState.MovingIn;
    [SerializeField] string speech;
    bool isTalking = false;
    GameObject dialogueBox;
    public CharacterRole role = CharacterRole.Normie;
    public GameObject mask;
    public GameObject badge;

    Transform dialoguePoint => transform.GetChild(2);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseY = rb.position.y;
    }

    private void Update()
    {
        if (transform.position.x <= 0 && currentState != CharacterState.MovingOut)
            currentState = CharacterState.Ordering;

        if (currentState == CharacterState.MovingOut && dialogueBox != null)
            Destroy(dialogueBox);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Talk();
    }

    void Move()
    {
        if (currentState != CharacterState.MovingIn && currentState != CharacterState.MovingOut) return;
    
        stepTime += Time.fixedDeltaTime;
    
        float wave = Mathf.Sin(stepTime * stepFrequency);
        wave = wave * Mathf.Abs(wave); 
    
        float y = baseY + wave * stepHeight;
        float x = rb.position.x - walkSpeed * Time.fixedDeltaTime;
    
        rb.MovePosition(new Vector2(x, y));
    }

    void Talk()
    {
        if (currentState == CharacterState.Ordering && !isTalking)
        {
            isTalking = true;

            dialogueBox = Instantiate(dialoguePrefab, dialoguePoint.position, Quaternion.identity, this.transform);

            TextMeshPro textField = dialogueBox.transform.GetChild(0).GetComponent<TextMeshPro>();

            textField.text = speech;
        }
    }

    string StringifyFood(Product product)
    {
        Debug.Log("Replaced");

        string foodString = "";

        switch (product)
        {
            case Product.Chicken:
                foodString = "chicken sharwama";
                break;
            case Product.Beef:
                foodString = "beef sharwama";
                break;
            case Product.Lamb:
                foodString = "lamb sharwama";
                break;
            case Product.Illegal:
                int ranVal = Random.Range(0, 3);
                switch (ranVal)
                {
                    case 0:
                        foodString = "chicken sharwama";
                        break;
                    case 1:
                        foodString = "beef sharwama";
                        break;
                    case 2:
                        foodString = "lamb sharwama";
                        break;
                }
                break;
        }

        return foodString;
    }

    public void SentenceSelector(Product product)
    {
        switch (role)
        {
            case CharacterRole.Normie:
                {
                    if (mask.gameObject.CompareTag("NormM"))
                    {
                        bool isNormalSpeech = Random.Range(0, 2) == 1;

                        speech = isNormalSpeech
                            ? dialogData.speech[Random.Range(0, dialogData.speech.Length)]
                            : dialogData.abnormal[Random.Range(7, dialogData.abnormal.Length)];

                    }
                    else
                        speech = dialogData.speech[Random.Range(0, dialogData.speech.Length)];

                    break;
                }
            case CharacterRole.Gang:
                {
                    speech = dialogData.abnormal[Random.Range(0, dialogData.abnormal.Length)];

                    break;
                }
            case CharacterRole.Police:
                {
                    int ranVal = Random.Range(0, 2);

                    if (mask.gameObject.CompareTag("GangM"))
                        speech = dialogData.speech[Random.Range(0, dialogData.speech.Length)];
                    else
                    {
                        switch (ranVal)
                        {
                            case 0:
                                speech = dialogData.speech[Random.Range(0, dialogData.speech.Length)];
                                break;
                            default:
                                speech = dialogData.abnormal[Random.Range(7, dialogData.abnormal.Length)];
                                break;
                        }
                    }
                    break;
                }
        }

        string food = StringifyFood(product);

        if (speech.Contains("{0}"))
        {
            speech = string.Format(speech, food);
        }
    }
}
