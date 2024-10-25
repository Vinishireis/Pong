using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioOnCollision : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private LayerMask collisionLayers = ~0;
    [SerializeField] private bool panStereo = false;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collisionLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            audioSource.clip = audioClip;
            if (panStereo)
            {
                audioSource.panStereo = Camera.main.WorldToViewportPoint(GetAverageContactPoint(collision)).x;
            }
            audioSource.Play();
        }
    }

    private Vector3 GetAverageContactPoint(Collision2D collision)
    {
        Vector2 sumContactPoints = Vector2.zero;
        Array.ForEach(collision.contacts, contact => sumContactPoints += contact.point);
        return sumContactPoints /= collision.contactCount;
    }
}
