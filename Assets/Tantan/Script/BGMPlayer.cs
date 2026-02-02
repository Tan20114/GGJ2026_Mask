using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance;

    AudioSource source => GetComponent<AudioSource>();
    [SerializeField] AudioClip firstShot;
    [SerializeField] AudioClip secondShot;

    bool started;

    void Awake()
    {
        // Singleton guard
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        
    }

    void Start()
    {
        if (started) return;
        started = true;

        StartCoroutine(PlayBGM());
    }

    IEnumerator PlayBGM()
    {
        source.loop = false;
        source.clip = firstShot;
        source.Play();

        yield return new WaitForSeconds(firstShot.length);

        source.clip = secondShot;
        source.loop = true;
        source.Play();
    }
}