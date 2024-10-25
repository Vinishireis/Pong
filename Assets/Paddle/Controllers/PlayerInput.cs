using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Paddle))]
public class PlayerInput : Controller
{
    private Paddle paddle;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    private void FixedUpdate()
    {
        paddle.Move(Input.GetAxisRaw("Vertical"));
    }
}
