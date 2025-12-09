using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(Rigidbody))]
public class Paddle : Agent
{
    public GameObject player1;
    public GameObject player2;
    public float ballSpeed = 15f;
    Rigidbody rBody;
    public GameObject ball;
    public float speed = 10f;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    void SetRandomBallDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDirection = new Vector3(randomX, 0, randomZ).normalized;
        ball.GetComponent<Rigidbody>().linearVelocity = randomDirection * ballSpeed;
    }

    void ResetGamePositions()
    {
        player1.transform.position = new Vector3(0, 0.5f, 22);
        player1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        player2.transform.position = new Vector3(0, 0.5f, -22);
        player2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        ball.transform.position = new Vector3(0, 0.5f, 0);
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public override void OnEpisodeBegin()
    {
        print("Episode Begin");
        ResetGamePositions();
        SetRandomBallDirection();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ball and Agent positions
        sensor.AddObservation(ball.transform.localPosition); // Ball position 3
        sensor.AddObservation(this.transform.localPosition); // Agent position 3
        // Agent velocity
        sensor.AddObservation(rBody.linearVelocity.x); // 1
        sensor.AddObservation(rBody.linearVelocity.z); // 1

        // we add the ball velocity so that the agent can know how it's moving
        sensor.AddObservation(ball.GetComponent<Rigidbody>().linearVelocity.x); // 1
        sensor.AddObservation(ball.GetComponent<Rigidbody>().linearVelocity.z); // 1
        // Total: 10
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        print("Action Received");
        // Actions, size = 1
        var vectorAction = actions.ContinuousActions;
        Vector3 controlSignal = Vector3.zero;
        controlSignal.y = vectorAction[0];
        rBody.linearVelocity = new Vector3(controlSignal.y * speed, 0, 0);

        SetReward(0.001f);

        if (ball.transform.position.z < -25f)
        {
            SetReward(-1.0f);
            EndEpisode();
        } else if (ball.transform.position.z > 25f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
    }

    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[1] = -Input.GetAxis("Vertical");
    }
}