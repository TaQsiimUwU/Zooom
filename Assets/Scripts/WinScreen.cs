using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI timeText;
    public Button nextLevelButton;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Animation")]
    public float fadeInDuration = 0.5f;

    [Header("Audio (Optional)")]
    public AudioClip restartSound;
    public AudioClip nextLevelSound;
    private AudioSource audioSource;

    private CanvasGroup canvasGroup;
    private float startTime;

    private void Awake()
    {
        // Get or add canvas group for fading
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;

        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (restartSound != null || nextLevelSound != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        startTime = Time.time;

        // Set title
        if (titleText != null)
        {
            titleText.text = "LEVEL COMPLETE!";
        }

        // Display completion time from LevelTimer
        if (timeText != null)
        {
            if (LevelTimer.Instance != null)
            {
                timeText.text = $"Final Time: {LevelTimer.Instance.GetFormattedTime()}";
            }
            else
            {
                float completionTime = Time.timeSinceLevelLoad;
                timeText.text = $"Time: {completionTime:F2}s";
            }
        }

        // Start fade in
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public void RestartLevel()
    {
        // Play restart sound
        if (restartSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(restartSound);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        // Play next level sound
        if (nextLevelSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(nextLevelSound);
        }

        Time.timeScale = 1f;
        // This should be called from GoalTrigger with the next scene name
        // SceneManager.LoadScene(nextSceneName);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Assumes main menu is first scene
    }
}
