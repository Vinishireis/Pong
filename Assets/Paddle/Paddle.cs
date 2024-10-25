using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Paddle : MonoBehaviour
{
    [SerializeField] private Side side;
    [SerializeField] private float speed = 10;
    [SerializeField] private float offsetX = 1;

    private new Rigidbody2D rigidbody;

    public Side Side { get { return side; } }

    private void OnValidate()
    {
        if (Camera.main != null)
        {
            transform.position = GetInitialPos();
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.position = GetInitialPos();
    }

    private Vector3 GetInitialPos()
    {
        float camZ = Camera.main.transform.position.z;
        Vector3 initialViewportPos = new Vector3(side == Side.Left ? 0 : 1, 0.5f, -camZ);
        Vector3 offset = Vector3.right * (side == Side.Left ? offsetX : -offsetX);
        return Camera.main.ViewportToWorldPoint(initialViewportPos) + offset;
    }

    public void Move(float verticalAxis)
    {
        verticalAxis = Mathf.Clamp(verticalAxis, -1, 1);
        rigidbody.linearVelocity = new Vector2(0, verticalAxis * speed);
    }

    public void ResetPos()
    {
        transform.position = GetInitialPos();
    }

    public bool IsMoving()
    {
        return rigidbody.linearVelocity.y != 0;
    }
}
