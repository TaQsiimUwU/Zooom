using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [Header("Timer Display")]
    public UnityEngine.UI.Text timerText; // For legacy UI
    public TMPro.TextMeshProUGUI timerTextTMP; // For TextMeshPro

    [Header("Display Settings")]
    public string prefix = "Time: ";
    public bool showMilliseconds = true;

    private float startTime;
    private bool isRunning = true;

    public static LevelTimer Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern - only one timer can exist
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        startTime = Time.time;

        // Ensure timer UI is visible
        if (timerText != null)
        {
            timerText.enabled = true;
        }

        if (timerTextTMP != null)
        {
            timerTextTMP.enabled = true;
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            float currentTime = Time.time - startTime;
            string timeString = prefix + FormatTime(currentTime);

            if (timerText != null)
            {
                timerText.text = timeString;
            }

            if (timerTextTMP != null)
            {
                timerTextTMP.text = timeString;
            }
        }
    }    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);

        if (showMilliseconds)
        {
            int milliseconds = (int)((time * 100f) % 100f);
            return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
        }
        else
        {
            return $"{minutes:00}:{seconds:00}";
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public float GetElapsedTime()
    {
        return Time.time - startTime;
    }

    public string GetFormattedTime()
    {
        return FormatTime(GetElapsedTime());
    }
}
