using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] GameObject[] charList;
    [SerializeField] GameObject[] maskList;
    [SerializeField] GameObject policeBadge;

    [Header("Reference")]
    [SerializeField] Transform spawnPoint;
    Transform maskPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject SpawnCharacter()
    {
        GameObject charToSpawn = charList[Random.Range(0, charList.Length)];
        SpriteRenderer charSR = charToSpawn.GetComponent<SpriteRenderer>();

        float offsetY = charSR.bounds.size.y / 2;

        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y + offsetY, spawnPoint.position.z);

        GameObject spawnedChar = Instantiate(charToSpawn, spawnPos, Quaternion.identity);

        maskPoint = spawnedChar.transform.GetChild(0);

        Instantiate(maskList[Random.Range(0, maskList.Length)], maskPoint.transform.position, Quaternion.identity,spawnedChar.transform);

        return spawnedChar;
    }
}
