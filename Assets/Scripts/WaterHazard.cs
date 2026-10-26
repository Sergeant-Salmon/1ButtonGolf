
using System.Collections;
using UnityEngine;

public class WaterHazard : MonoBehaviour
{
    public Transform ballStartPosition;
    public PowerController powerController;
    public ParticleSystem waterSplash;

    public float sinkDuration = 0.25f;
    public float respawnDelay = 0.35f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        Rigidbody2D body = other.GetComponent<Rigidbody2D>();

        if (body == null || ballStartPosition == null)
            return;

        triggered = true;
        StartCoroutine(WaterPenalty(other.transform, body));
    }

    private IEnumerator WaterPenalty(
        Transform ball,
        Rigidbody2D body)
    {
        BallEffects effects = ball.GetComponent<BallEffects>();

        if (effects != null)
            effects.enabled = false;

        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.simulated = false;

        Vector3 originalScale = ball.localScale;

        if (waterSplash != null)
        {
            waterSplash.transform.position = ball.position;
            waterSplash.gameObject.SetActive(true);
            waterSplash.Play();
        }

        float elapsed = 0f;

        while (elapsed < sinkDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / sinkDuration);
            ball.localScale = Vector3.Lerp(
                originalScale,
                Vector3.zero,
                progress
            );

            yield return null;
        }

        if (powerController != null)
            powerController.AddPenaltyStroke();

        yield return new WaitForSeconds(respawnDelay);

        ball.position = ballStartPosition.position;
        ball.localScale = originalScale;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.simulated = true;

        if (effects != null)
            effects.enabled = true;

        triggered = false;
    }
}
