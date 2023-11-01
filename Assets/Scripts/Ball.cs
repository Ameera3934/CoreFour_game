using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for UI elements

public class Ball : MonoBehaviour
{
    public Transform target;
    public float Force;
    public TextMeshProUGUI scoreText; // Reference to the UI Text element for displaying the score
    public TextMeshProUGUI timerText; // Reference to the UI Text element for displaying the timer
    private int score; // Initial score
    private Vector3 initialPosition;
    private float gameDuration = 30f; // Duration of the game in seconds
    private float timeLeft; // Time remaining in the game
    private bool isGameOver = false;

    void Start()
    {
        initialPosition = new Vector3(0, 1, 35);

        // Find the Text components in the scene with the specified names
        scoreText = GameObject.Find("ScoreText").GetComponent < TextMeshProUGUI>();
        timerText = GameObject.Find("TimerText").GetComponent < TextMeshProUGUI>();

        score = 0;
        UpdateScoreText(); // Update the score text initially
        timeLeft = gameDuration; // Initialize the timer
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Update the timer
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timeLeft = 0;
                GameOver();
            }

            UpdateTimerText(); // Update the timer text

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector3 shootDirection = (target.position - transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(shootDirection * Force + new Vector3(0, 3f, 0), ForceMode.Impulse);

        StartCoroutine(ResetBallAfterDelay(1f));
    }

    IEnumerator ResetBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = initialPosition;

        UpdateScoreText(); // Update the score text after the ball returns to the initial position
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal")) // Check if the collision is with a goal object
        {
            score++; // Increment the score
            Debug.Log("Score: " + score);
            UpdateScoreText(); // Update the score text
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.Ceil(timeLeft).ToString(); // Display the rounded time remaining
    }

    void GameOver()
    {
        isGameOver = true;
        // Display "Game Over" message
        timerText.text = "Game Over";
    }
}