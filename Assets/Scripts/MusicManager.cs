using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [Header("Music Tracks")]
    [Tooltip("Menu/Main Menu theme music")]
    public AudioClip menuTheme;

    [Tooltip("Gameplay/Level theme music")]
    public AudioClip gameplayTheme;

    [Header("Scene Detection")]
    [Tooltip("Names of scenes that should play menu music (e.g., 'MainMenu', 'LevelSelect')")]
    public string[] menuSceneNames = { "MainMenu", "Menu" };

    [Header("Settings")]
    [Tooltip("Volume of the music (0-1)")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    [Tooltip("Should music play on start?")]
    public bool playOnStart = true;

    [Tooltip("Loop the music?")]
    public bool loopMusic = true;

    [Tooltip("Fade duration when changing tracks")]
    public float fadeDuration = 1.0f;

    [Header("UI References (Optional)")]
    public Button musicToggleButton;
    public TMPro.TextMeshProUGUI buttonText;
    public Image buttonIcon;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    private AudioSource audioSource;
    private bool isMusicPlaying = true;
    private bool isFading = false;
    private AudioClip currentClip;

    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern - keeps music playing across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Music persists between scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Setup audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure audio source
        audioSource.loop = loopMusic;
        audioSource.volume = musicVolume;
        audioSource.playOnAwake = false;

        // Subscribe to scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Start playing appropriate music
        if (playOnStart)
        {
            PlayAppropriateMusic();
        }

        // Setup button if assigned
        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.AddListener(ToggleMusic);
        }

        UpdateButtonUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically switch music based on scene
        PlayAppropriateMusic();
    }

    private void PlayAppropriateMusic()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if current scene is a menu scene
        bool isMenuScene = false;
        foreach (string menuSceneName in menuSceneNames)
        {
            if (currentSceneName.Equals(menuSceneName, System.StringComparison.OrdinalIgnoreCase))
            {
                isMenuScene = true;
                break;
            }
        }

        // Select appropriate music
        AudioClip targetClip = isMenuScene ? menuTheme : gameplayTheme;

        // Only change if different from current
        if (targetClip != null && targetClip != currentClip)
        {
            if (fadeDuration > 0 && audioSource.isPlaying)
            {
                // Fade transition
                StartCoroutine(CrossfadeToClip(targetClip));
            }
            else
            {
                // Instant change
                ChangeMusic(targetClip);
            }
        }
    }

    private System.Collections.IEnumerator CrossfadeToClip(AudioClip newClip)
    {
        if (isFading) yield break;

        isFading = true;
        float startVolume = audioSource.volume;

        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeDuration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (fadeDuration / 2f));
            yield return null;
        }

        // Change track
        audioSource.Stop();
        audioSource.clip = newClip;
        currentClip = newClip;
        if (isMusicPlaying)
        {
            audioSource.Play();
        }

        // Fade in
        elapsed = 0f;
        while (elapsed < fadeDuration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, musicVolume, elapsed / (fadeDuration / 2f));
            yield return null;
        }

        audioSource.volume = musicVolume;
        isFading = false;
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (newClip == null) return;

        audioSource.Stop();
        audioSource.clip = newClip;
        currentClip = newClip;

        if (isMusicPlaying)
        {
            audioSource.Play();
        }
    }

    public void PlayMusic()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            isMusicPlaying = true;
            UpdateButtonUI();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            isMusicPlaying = false;
            UpdateButtonUI();
        }
    }

    public void PauseMusic()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
            isMusicPlaying = false;
            UpdateButtonUI();
        }
    }

    public void ResumeMusic()
    {
        if (audioSource != null)
        {
            audioSource.UnPause();
            isMusicPlaying = true;
            UpdateButtonUI();
        }
    }

    public void ToggleMusic()
    {
        if (isMusicPlaying)
        {
            PauseMusic();
        }
        else
        {
            ResumeMusic();
        }
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (audioSource != null && !isFading)
        {
            audioSource.volume = musicVolume;
        }
    }

    public bool IsMusicPlaying()
    {
        return isMusicPlaying && audioSource != null && audioSource.isPlaying;
    }

    private void UpdateButtonUI()
    {
        if (musicToggleButton == null) return;

        // Update button text
        if (buttonText != null)
        {
            buttonText.text = isMusicPlaying ? "Music: ON" : "Music: OFF";
        }

        // Update button icon
        if (buttonIcon != null)
        {
            if (isMusicPlaying && musicOnIcon != null)
            {
                buttonIcon.sprite = musicOnIcon;
            }
            else if (!isMusicPlaying && musicOffIcon != null)
            {
                buttonIcon.sprite = musicOffIcon;
            }
        }
    }

    // Volume slider callback
    public void OnVolumeSliderChanged(float value)
    {
        SetVolume(value);
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene events
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Clean up button listener
        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.RemoveListener(ToggleMusic);
        }
    }
}
