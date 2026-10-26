
using System.Collections;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    public PowerController powerController;
    public ParticleSystem holeParticles;

    [Header("Hole Settings")]
    public float maxEntrySpeed = 2f;
    public float sinkDuration = 0.3f;

    private bool completed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (completed || !other.CompareTag("Player"))
            return;

        Rigidbody2D ballBody = other.GetComponent<Rigidbody2D>();

        if (ballBody == null)
            return;

        // The ball must be moving slowly enough to sink.
        if (ballBody.linearVelocity.magnitude > maxEntrySpeed)
            return;

        completed = true;
        StartCoroutine(SinkBall(other.transform, ballBody));
    }

    IEnumerator SinkBall(Transform ballTransform, Rigidbody2D ballBody)
    {
        BallEffects ballEffects = ballTransform.GetComponent<BallEffects>();

        if (ballEffects != null)
            ballEffects.enabled = false;

        ballBody.linearVelocity = Vector2.zero;
        ballBody.angularVelocity = 0f;
        ballBody.simulated = false;

        Vector3 originalScale = ballTransform.localScale;
        float elapsed = 0f;

        while (elapsed < sinkDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / sinkDuration);

            ballTransform.localScale = Vector3.Lerp(
                originalScale,
                Vector3.zero,
                progress
            );

            yield return null;
        }

        ballTransform.localScale = Vector3.zero;

        if (holeParticles != null)
        {
            holeParticles.transform.position = transform.position;
            holeParticles.gameObject.SetActive(true);
            holeParticles.Play();
        }

        if (powerController != null)
            powerController.CompleteHole();
    }
}
