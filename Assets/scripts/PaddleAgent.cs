using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PaddleAgent : Agent
{
    public bool isPlayer = true;

    Rigidbody rBody;

    public GameObject ball;
    //private PlayerInput playerInput;
    InputAction moveAction;

    public float speed = 10f;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        //playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (isPlayer)
        {
            Vector2 moveValue = moveAction.ReadValue<Vector2>();

            //rBody.MovePosition(rBody.position + speed * Time.fixedDeltaTime * new Vector3(moveValue.y * -1, 0, 0));        
            rBody.linearVelocity = new Vector3(moveValue.y * -speed, 0, 0);
        } else
        {
            ;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Kinematic object collided with: " + collision.gameObject.name);

        /*if (collision.gameObject.CompareTag("border"))
        {
            rBody.linearVelocity = Vector3.zero;
        }*/

        // Add your collision handling logic here
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
        //var inputValue = playerInput.actions["Move"].ReadValue<float>();
        //continuousActionsOut[0] = -inputValue;
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