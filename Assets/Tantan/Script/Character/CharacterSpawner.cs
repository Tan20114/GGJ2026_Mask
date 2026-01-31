using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] GameObject[] charList;
    [SerializeField] GameObject[] maskList;
    [SerializeField] GameObject policeBadge;

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
        if(currentCharacter == null)
        {
            NewCustomer();
        }

        if(currentCharacter.transform.position.x <= endPoint.position.x)
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
        maskPoint = spawned.transform.GetChild(0);
        badgePoint = spawned.transform.GetChild(1);

        if (rand <= NormiePity)
        {
            currentCharacter.role = CharacterRole.Normie;

            Instantiate(maskList[Random.Range(0, maskList.Length - 1)], maskPoint.transform.position, Quaternion.identity, spawned.transform);
        }
        else if (rand <= NormiePity + GangPity)
        {
            currentCharacter.role = CharacterRole.Gang;

            Instantiate(maskList[2], maskPoint.transform.position, Quaternion.identity, spawned.transform);
        }
        else
        {
            Instantiate(policeBadge, badgePoint.position, Quaternion.identity, spawned.transform);

            currentCharacter.role = CharacterRole.Police;
        }
    }

    void NewCustomer()
    {
        currentCharacter = SpawnCharacter();
        RandomRole(currentCharacter.gameObject);
    }
}
