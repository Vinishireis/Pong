using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Paddle))]
public class PlayerOneInput : Controller
{
    private Paddle paddle;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    private void FixedUpdate()
    {
        paddle.Move((Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0));
    }
}
