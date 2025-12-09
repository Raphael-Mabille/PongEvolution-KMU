using UnityEngine;

public class gameManager : MonoBehaviour
{

    public int scorePlayer1 = 0;
    public int scorePlayer2 = 0;
    public GameObject Ball;

    public GameObject player1;
    public GameObject player2;
    void Start()
    {
        
    }

    void ResetScores()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
    }

    void ResetGamePositions()
    {
        player1.transform.position = new Vector3(0, -22, 0);
        player1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        player2.transform.position = new Vector3(0, 22, 0);
        player2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        Ball.transform.position = Vector3.zero;
        Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Ball.transform.position.y < -25f)
        {
            ResetGamePositions();
            scorePlayer1 += 1;
        } else if (Ball.transform.position.y > 25f)
        {
            ResetGamePositions();
            scorePlayer2 += 1;
        }
    }
}
