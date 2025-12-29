using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoseScreen : MonoBehaviour
{
    [Header("UI References")]
    public GameObject losePanel;
    public TMPro.TextMeshProUGUI loseText;
    public TMPro.TextMeshProUGUI timeText;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Visual Effects")]
    public Image fadeImage; // Full-screen image for fade effect
    public Color fadeStartColor = Color.white;
    public Color fadeEndColor = Color.black;

    [Header("Timing")]
    public float slowMotionDuration = 1.5f;
    public float slowMotionScale = 0.1f;
    public float fadeDuration = 1.0f;
    public float uiDelayAfterFade = 0.5f;    [Header("Audio (Optional)")]
    public AudioClip fallSound;
    public AudioClip restartSound;
    private AudioSource audioSource;

    public static LoseScreen Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && fallSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Hide UI at start
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
            fadeImage.color = new Color(1, 1, 1, 0);
        }
    }

    public void TriggerLoseScreen()
    {
        StartCoroutine(LoseSequence());
    }

    private IEnumerator LoseSequence()
    {
        // 1. Play sound effect
        if (fallSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fallSound);
        }

        // 2. Activate fade image
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
        }

        // 3. Start slow motion
        Time.timeScale = slowMotionScale;

        // 4. Fade to white
        float elapsed = 0f;
        while (elapsed < fadeDuration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (fadeDuration / 2f);

            if (fadeImage != null)
            {
                Color currentColor = Color.Lerp(
                    new Color(fadeStartColor.r, fadeStartColor.g, fadeStartColor.b, 0),
                    new Color(fadeStartColor.r, fadeStartColor.g, fadeStartColor.b, 1),
                    t
                );
                fadeImage.color = currentColor;
            }

            yield return null;
        }

        // 5. Wait during slow motion
        yield return new WaitForSecondsRealtime(slowMotionDuration - fadeDuration);

        // 6. Fade from white to black
        elapsed = 0f;
        while (elapsed < fadeDuration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (fadeDuration / 2f);

            if (fadeImage != null)
            {
                Color currentColor = Color.Lerp(fadeStartColor, fadeEndColor, t);
                fadeImage.color = currentColor;
            }

            yield return null;
        }

        // 7. Pause the game completely
        Time.timeScale = 0f;

        // 8. Show UI after delay
        yield return new WaitForSecondsRealtime(uiDelayAfterFade);

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        // Set lose text
        if (loseText != null)
        {
            loseText.text = "YOU FELL!";
        }

        // Show time survived
        if (timeText != null && LevelTimer.Instance != null)
        {
            timeText.text = $"Time: {LevelTimer.Instance.GetFormattedTime()}";
        }

        // Stop the timer
        if (LevelTimer.Instance != null)
        {
            LevelTimer.Instance.StopTimer();
        }

        // Unlock cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }    public void RestartLevel()
    {
        // Play restart sound
        if (restartSound != null && audioSource != null)
        {
            // Play sound but don't wait for it
            audioSource.PlayOneShot(restartSound);
        }

        // Restart immediately
        PerformRestart();
    }

    private IEnumerator RestartAfterSound()
    {
        // Wait for the sound to play (using unscaled time since game is paused)
        yield return new WaitForSecondsRealtime(restartSound.length);
        PerformRestart();
    }

    private void PerformRestart()
    {
        // Reset time scale
        Time.timeScale = 1f;

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Assumes main menu is first scene
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
