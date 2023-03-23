using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_SG : MonoBehaviour
{
    Vector2 move;

    public string direction = "right";
    public Rigidbody2D rb;
    public Movement movement;
    public FixedJoystick Joystick;
    public float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        move.x = Joystick.Horizontal;
        move.y = Joystick.Vertical;
        if (move.x < 0 && direction == "right")
        {
            this.transform.Rotate(Vector3.up * 180);
            direction = "left";
        }
        else if (move.x > 0 && direction == "left")
        {
            this.transform.Rotate(Vector3.up * 180);
            direction = "right";
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (direction == "right")
            {
                this.transform.Rotate(Vector3.up * 180);
                direction = "left";
            }
            this.movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (direction == "left")
            {
                this.transform.Rotate(Vector3.up * 180);
                direction = "right";
            }
            this.movement.SetDirection(Vector2.right);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}
