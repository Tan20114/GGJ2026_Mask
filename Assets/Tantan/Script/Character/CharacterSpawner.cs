using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] GameObject[] charList;
    public GameObject[] maskList;
    public GameObject policeBadge;
    public GameObject gangBadge;

    [Header("Reference")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform endPoint;
    Transform maskPoint;
    Transform badgePoint;

    [Header("Parameter")]
    public Character currentCharacter;
    [SerializeField] float NormiePity = 40;
    [SerializeField] float PolicePity = 20;
    [SerializeField] float GangPity = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharacter == null)
        {
            NewCustomer();
        }

        if (currentCharacter.transform.position.x <= endPoint.position.x)
        {
            Destroy(currentCharacter.gameObject);
            currentCharacter = null;
        }
    }

    Character SpawnCharacter()
    {
        int RanVal = Random.Range(0, charList.Length * 10);
        int characterIndex = RanVal % 8;

        GameObject charToSpawn = charList[characterIndex];
        SpriteRenderer charSR = charToSpawn.GetComponent<SpriteRenderer>();

        float offsetY = charSR.bounds.size.y / 2;

        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y + offsetY, spawnPoint.position.z);

        GameObject spawnedChar = Instantiate(charToSpawn, spawnPos, Quaternion.identity);

        return spawnedChar.GetComponent<Character>();
    }

    void RandomRole(GameObject spawned)
    {
        float totalpity = NormiePity + PolicePity + GangPity;

        float rand = Random.Range(0, totalpity);
        Debug.Log(rand);
        maskPoint = spawned.transform.GetChild(0);
        badgePoint = spawned.transform.GetChild(1);

        currentCharacter.mask = Instantiate(maskList[Random.Range(0, maskList.Length)], maskPoint.transform.position, Quaternion.identity, spawned.transform);
        if (rand <= NormiePity)
        {
            currentCharacter.role = CharacterRole.Normie;

            if (currentCharacter.mask.CompareTag("PoliceM"))
                currentCharacter.badge = Instantiate(gangBadge, badgePoint.position, Quaternion.identity, spawned.transform);

        }
        else if (rand <= NormiePity + GangPity)
        {
            currentCharacter.role = CharacterRole.Gang;

            if (currentCharacter.mask.CompareTag("GangM"))
            {
                int ranVal = Random.Range(0, 3);

                switch (ranVal)
                {
                    case 0:
                        currentCharacter.badge = Instantiate(gangBadge, badgePoint.position, Quaternion.identity, spawned.transform);
                        break;
                    case 1:
                        currentCharacter.badge = Instantiate(policeBadge, badgePoint.position, Quaternion.identity, spawned.transform);
                        break;
                    default:
                        break;
                }
            }
            else
                currentCharacter.badge = Instantiate(gangBadge, badgePoint.position, Quaternion.identity, spawned.transform);
        }
        else
        {
            currentCharacter.role = CharacterRole.Police;

            if (currentCharacter.mask.gameObject.CompareTag("PoliceM"))
            {
                int ranVal = Random.Range(0, 2);

                switch (ranVal)
                {
                    case 0:
                        currentCharacter.badge = Instantiate(policeBadge, badgePoint.position, Quaternion.identity, spawned.transform);
                        break;
                    default:
                        break;
                }
            }
            else
                currentCharacter.badge = Instantiate(policeBadge, badgePoint.position, Quaternion.identity, spawned.transform);
        }
    }
    void NewCustomer()
    {
        currentCharacter = SpawnCharacter();
        RandomRole(currentCharacter.gameObject);
    }
}
