using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("Win Settings")]
    [Tooltip("Time to wait before loading next level")]
    public float delayBeforeNextLevel = 2f;

    [Tooltip("Name of the next scene to load (leave empty to reload current level)")]
    public string nextSceneName = "";

    [Header("Effects")]
    public ParticleSystem winParticles;
    public AudioClip winSound;

    [Header("UI")]
    public GameObject winUI;

    private bool hasWon = false;
    private AudioSource audioSource;

    private void Start()
    {
        // Get or add audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && winSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Hide win UI at start
        if (winUI != null)
        {
            winUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player entered the goal
        if (other.CompareTag("Player") && !hasWon)
        {
            hasWon = true;
            WinLevel();
        }
    }    private void WinLevel()
    {
      //   Debug.Log("🎉 Level Complete!");

        // Stop the timer
        if (LevelTimer.Instance != null)
        {
            LevelTimer.Instance.StopTimer();
        }

        // Show win UI
        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        // Play particles
        if (winParticles != null)
        {
            winParticles.Play();
        }

        // Play sound
        if (winSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(winSound);
        }

        // Unlock cursor so player can click UI buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Optionally slow down time for dramatic effect
        // Time.timeScale = 0.3f;

        // Load next level after delay
        if (delayBeforeNextLevel > 0)
        {
            Invoke(nameof(LoadNextLevel), delayBeforeNextLevel);
        }
    }

    private void LoadNextLevel()
    {
        // Reset time scale
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // Load specified scene
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Reload current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Public method to be called from UI buttons
    public void RestartLevel()
    {
        Time.timeScale = 1f;
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
