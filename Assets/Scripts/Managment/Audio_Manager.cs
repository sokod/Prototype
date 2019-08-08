using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;

    [Range(0, 1f)]
    public float volume;
    [Range(0, 1)]
    public float pitch;

    public bool loop;

}

public class Audio_Manager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;

    public static Audio_Manager Instance;

    private void Awake()
    {
        if (Instance == null)
        { // Экземпляр менеджера был найден
            Instance = this; // Задаем ссылку на экземпляр объекта
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        else if (Instance != null)
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
            Debug.LogWarning("Tried to create another instance");
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    void OnSceneChanged(Scene previousScene, Scene changedScene)
    {
        foreach(Button button in Resources.FindObjectsOfTypeAll<Button>())
        {
            button.onClick.AddListener(PlayClickSound);
        }
        Debug.LogWarning("OnSceneChanged changedScene = " + changedScene.name);
    }

    void PlayClickSound()
    {
        foreach (Sound s in sounds)
        {
            if (s.name == "Button_Click")
            {
                s.source.Play();
                return;
            }
        }
    }

    public void MuteSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.volume = 0;
                return;
            }
        }
    }

    public void UnmuteSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.volume = 0.65f;
                return;
            }
        }
    }

    public void Play(string n)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == n)
            {
                s.source.Play();
                return;
            }
        }
    }
    public void Stop(string n)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == n)
            {
                s.source.Stop();
                return;
            }
        }
    }
    
}
