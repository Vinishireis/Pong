using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Goal : MonoBehaviour
{
    public event Action OnScoring = delegate { };

    [SerializeField] private Side side;
    [SerializeField] private LayerMask ballLayers;

    private new Collider2D collider;
    private AudioSource audioSource;

    private void OnValidate()
    {
        if (Camera.main != null)
        {
            collider = GetComponent<Collider2D>();
            transform.position = GetInitialPos();
        }
    }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        transform.position = GetInitialPos();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((ballLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            audioSource.Play();
            OnScoring();
        }
    }

    private Vector3 GetInitialPos()
    {
        float camZ = Camera.main.transform.position.z;
        Vector3 initialViewportPos;
        if (side == Side.Left)
        {
            initialViewportPos = new Vector3(0, 0.5f, -camZ);
            return Camera.main.ViewportToWorldPoint(initialViewportPos) + Vector3.left * collider.bounds.extents.x;
        }
        else
        {
            initialViewportPos = new Vector3(1, 0.5f, -camZ);
            return Camera.main.ViewportToWorldPoint(initialViewportPos) + Vector3.right * collider.bounds.extents.x;
        }
    }
}
