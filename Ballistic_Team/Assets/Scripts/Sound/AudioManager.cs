using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip backGround;
    public AudioClip Click;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        musicSource.clip = backGround;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddClickSoundToAllButtons();
    }

    private void AddClickSoundToAllButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true); // 비활성 오브젝트 포함하려면 true
        foreach (Button btn in allButtons)
        {
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayClickSound()
    {
        SFXSource.PlayOneShot(Click);
    }
}