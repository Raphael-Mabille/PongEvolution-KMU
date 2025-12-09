using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TMP_Text scoreTextPlayer1;
    public TMP_Text scoreTextPlayer2;
    private int scorePlayer1 = 0;
    private int scorePlayer2 = 0;
    public GameObject Ball;
    public float startDelay = 2f;
    public float ballSpeed = 10f;
    public bool autoStart = true;
    public bool endOnScore = true;

    public GameObject player1;
    public GameObject player2;
    void Start()
    {
        if (autoStart)
        {
            RestartGame();
        }
    }

    void ResetScores()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;

        scoreTextPlayer1.text = scorePlayer1.ToString();
        scoreTextPlayer2.text = scorePlayer2.ToString();
    }

    public void RestartGame()
    {
        ResetScores();
        ResetGamePositions();
        Invoke("SetRandomBallDirection", startDelay);
    }

    void SetRandomBallDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        // Make sure the ball doesn't go straight horizontally
        while (randomZ == 0 && Mathf.Abs(randomX) == 1.0f)
        {
            randomZ = Random.Range(-1f, 1f);
        }
        // Make sure the ball doesn't take forever to reach the paddle
        while (Mathf.Abs(randomZ) < 0.1f)
        {
            randomZ = Random.Range(-1f, 1f);
        }
        Vector3 randomDirection = new Vector3(randomX, 0, randomZ).normalized;

        Ball.GetComponent<Rigidbody>().linearVelocity = randomDirection * ballSpeed;
    }

    void ResetGamePositions()
    {
        player1.transform.position = new Vector3(0, 0.5f, 22);
        player1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        player2.transform.position = new Vector3(0, 0.5f, -22);
        player2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        Ball.transform.position = new Vector3(0, 0.5f, 0);
        Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    bool IsGameOver()
    {
        if (endOnScore == false) {
            return false;
        }
        return scorePlayer1 >= 5 || scorePlayer2 >= 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Ball.transform.position.z < -25f)
        {
            ResetGamePositions();
            scorePlayer1 += 1;
            scoreTextPlayer1.text = scorePlayer1.ToString();
            if (!IsGameOver()) {
                Invoke("SetRandomBallDirection", startDelay);
            }
        } else if (Ball.transform.position.z > 25f)
        {
            ResetGamePositions();
            scorePlayer2 += 1;
            scoreTextPlayer2.text = scorePlayer2.ToString();
            if (!IsGameOver()) {
                Invoke("SetRandomBallDirection", startDelay);
            }
        }
    }
}
