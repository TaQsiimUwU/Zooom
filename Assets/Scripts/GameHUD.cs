using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Timer Settings")]
    public bool showTimer = true;
    public TextMeshProUGUI timerDisplay;

    [Header("Speed/Info Display (Optional)")]
    public TextMeshProUGUI speedDisplay;
    public bool showSpeed = false;

    private LevelTimer levelTimer;
    private Rigidbody playerRb;

    private void Start()
    {
        // Find or create timer
        levelTimer = FindObjectOfType<LevelTimer>();
        if (levelTimer == null && showTimer)
        {
            GameObject timerObj = new GameObject("LevelTimer");
            levelTimer = timerObj.AddComponent<LevelTimer>();
            levelTimer.timerTextTMP = timerDisplay;
        }

        // Find player rigidbody for speed display
        if (showSpeed)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerRb = player.GetComponent<Rigidbody>();
            }
        }

        // Hide speed display if not needed
        if (speedDisplay != null && !showSpeed)
        {
            speedDisplay.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Update speed display
        if (showSpeed && speedDisplay != null && playerRb != null)
        {
            float speed = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z).magnitude;
            speedDisplay.text = $"Speed: {speed:F1} m/s";
        }
    }
}
