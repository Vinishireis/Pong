using UnityEngine;

public class NormalCPU : CPUController
{
    private void FixedUpdate()
    {
        bool ballIsComing = ballRigidbody.linearVelocity.x * (paddle.Side == Side.Left ? -1 : 1) > 0;

        bool computePrediction = targetY == 0 || ballRigidbody.position == Vector2.zero;
        targetY = ballIsComing ? (computePrediction ? GetClampPredictedY() : targetY) : 0;
        if (Mathf.Abs(transform.position.y - targetY) > 2)
        {
            goingUp = transform.position.y < targetY;
        }
        else
        {
            // Avoid fully perpendicular throws by enforcing movement
            if (!paddle.IsMoving())
            {
                goingUp = !goingUp;
            }
        }

        float distanceFromBallX = Mathf.Abs(transform.position.x - ballRigidbody.position.x);
        float distanceFactor = 1 - (distanceFromBallX / screenSizeInUnits.x);
        paddle.Move((goingUp ? 1 : -1) * distanceFactor * (ballIsComing ? 1 : 0.5f));
    }
}
