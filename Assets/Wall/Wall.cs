using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class Wall : MonoBehaviour
{
    public enum Side { Top, Bottom }

    [SerializeField] private Side side;
    [SerializeField] private float offsetY = -1;
    [SerializeField] private float widthOffset = -1;

    private SpriteRenderer spriteRenderer;

    private void OnValidate()
    {
        if (Camera.main != null)
        {
            transform.position = GetInitialPos();
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = GetInitialPos();
        SetInitialWidth();
    }

    private Vector3 GetInitialPos()
    {
        float camZ = Camera.main.transform.position.z;
        Vector3 initialViewportPos = new Vector3(0.5f, side == Side.Top ? 1 : 0, -camZ);
        Vector3 offset = Vector3.up * (side == Side.Top ? offsetY : -offsetY);
        return Camera.main.ViewportToWorldPoint(initialViewportPos) + offset;
    }

    private void SetInitialWidth()
    {
        float width = Camera.main.ViewportToWorldPoint(Vector3.right).x * 2 + widthOffset;
        Vector2 newSize = spriteRenderer.size;
        newSize.x = width;
        spriteRenderer.size = newSize;
    }
}
