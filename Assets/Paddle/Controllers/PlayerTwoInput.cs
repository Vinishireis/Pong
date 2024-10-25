using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Paddle))]
public class PlayerTwoInput : Controller
{
    private Paddle paddle;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    private void FixedUpdate()
    {
        paddle.Move((Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0));
    }
}
