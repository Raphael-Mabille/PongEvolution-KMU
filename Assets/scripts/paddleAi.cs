using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PaddleAI : MonoBehaviour
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
            rBody.linearVelocity = new Vector3(moveValue.y * -speed, 0, 0);
        } else
        {
            var ballPosition = ball.transform.position;
            if (ballPosition.x > this.transform.position.x) {
                rBody.linearVelocity = new Vector3(speed, 0, 0);
            } else if (ballPosition.x < this.transform.position.x) {
                rBody.linearVelocity = new Vector3(-speed, 0, 0);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Kinematic object collided with: " + collision.gameObject.name);
    }
}