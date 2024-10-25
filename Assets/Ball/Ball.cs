using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrailRenderer))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 20;
    [SerializeField] private LayerMask acceleratorLayers;
    [SerializeField] private float speedIncrease = 10f;
    [SerializeField] private LayerMask goalLayers;
    [SerializeField] private ParticleSystem collisionParticlesPrefab;

    private new Rigidbody2D rigidbody;
    private TrailRenderer trailRenderer;

    public bool ModernEffectsEnabled
    {
        get { return trailRenderer.enabled; }
        set { trailRenderer.enabled = value; }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        ModernEffectsEnabled = false;

        // Game supports multiple aspect ratios, but is designed for 16:9.
        // Ball speed is scaled by aspect ratio to equalize the gameplay experience.
        float speedScale = Camera.main.aspect / (16f / 9);
        initialSpeed *= speedScale;
        speedIncrease *= speedScale;
    }

    private IEnumerator Start()
    {
        // Fail-safe (two players may cooperate to set the ball in a
        // perfectly horizontal direction, achieving unlimited ball speed).
        while (true)
        {
            yield return new WaitForSeconds(5);
            Vector3 ballViewPos = Camera.main.WorldToViewportPoint(rigidbody.position);
            if (rigidbody.linearVelocity.magnitude > 500 || Mathf.Abs(ballViewPos.x) > 1.5f)
            {
                ResetPos();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((acceleratorLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            rigidbody.linearVelocity += rigidbody.linearVelocity.normalized * speedIncrease;
        }
        if (ModernEffectsEnabled)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector2.right, collision.contacts[0].normal);
            Instantiate(collisionParticlesPrefab, collision.contacts[0].point, rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((goalLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            ResetPos();
        }
    }

    public void ResetPos()
    {
        transform.position = Vector2.zero;
        rigidbody.linearVelocity = GetInitialDir() * initialSpeed;
        trailRenderer.Clear();
    }

    private Vector2 GetInitialDir()
    {
        float x = Random.Range(0.5f, 1) * Mathf.Sign(Random.Range(-1, 1));
        return new Vector2(x, Random.Range(-0.5f, 0.5f)).normalized;
    }
}
