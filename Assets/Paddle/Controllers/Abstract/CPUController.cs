using UnityEngine;

public abstract class CPUController : Controller
{
    protected Paddle paddle;
    protected Rigidbody2D ballRigidbody;
    private LayerMask ignoreLayers;
    protected Vector2 screenSizeInUnits;
    protected float targetY;
    protected bool goingUp;

    private void OnDrawGizmos()
    {
        Vector3 targetPos = new Vector3(paddle.transform.position.x, targetY, 0);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetPos, 1);
    }

    protected virtual void Awake()
    {
        paddle = GetComponent<Paddle>();

        GameObject ball = GameObject.FindWithTag("Ball");
        ballRigidbody = ball.GetComponent<Rigidbody2D>();

        ignoreLayers.value = LayerMask.GetMask("Paddles");

        float pixelsToUnits = (Camera.main.orthographicSize * 2) / Screen.height;
        screenSizeInUnits = new Vector2(Screen.width, Screen.height) * pixelsToUnits;
    }

    protected float GetClampPredictedY()
    {
        float paddleMaxY = Camera.main.orthographicSize - 2.95f;
        return Mathf.Clamp(GetPredictedY(), -paddleMaxY, paddleMaxY);
    }

    private float GetPredictedY()
    {
        RaycastHit2D hit = Physics2D.Raycast(ballRigidbody.position, ballRigidbody.linearVelocity, Mathf.Infinity, ~ignoreLayers.value);
        while (hit && hit.collider.GetComponent<Goal>() == null)
        {
            Vector2 newDir = Vector2.Reflect(ballRigidbody.linearVelocity, hit.normal);
            hit = Physics2D.Raycast(hit.point, newDir, Mathf.Infinity, ~ignoreLayers.value);
        }
        return hit ? hit.point.y : 0;
    }
}
