
using UnityEngine;

public class SandTrap : MonoBehaviour
{
    [Header("Sand Settings")]
    public float speedMultiplier = 0.98f;
    public float minimumSpeed = 0.15f;

    [Header("Visual Effects")]
    public ParticleSystem sandParticles;

    private bool particlesPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (sandParticles != null && !particlesPlayed)
        {
            sandParticles.transform.position = other.transform.position;
            sandParticles.gameObject.SetActive(true);
            sandParticles.Play();

            particlesPlayed = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Rigidbody2D ballBody = other.GetComponent<Rigidbody2D>();

        if (ballBody == null)
            return;

        ballBody.linearVelocity *= speedMultiplier;

        if (ballBody.linearVelocity.magnitude < minimumSpeed)
        {
            ballBody.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        particlesPlayed = false;
    }
}
