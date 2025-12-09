using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

public class PaddleAgent : Agent
{
    Rigidbody rBody;

    public GameObject ball;
    private PlayerInput playerInput;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    public override void OnEpisodeBegin()
    {

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

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        var inputValue = playerInput.actions["Move"].ReadValue<float>();
        continuousActionsOut[0] = -inputValue;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Actions, size = 1
        var vectorAction = actions.ContinuousActions;
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        rBody.AddForce(controlSignal * 10);
    }
}
