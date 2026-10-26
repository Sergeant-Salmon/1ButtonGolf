
using System.Collections;
using UnityEngine;

public class BallEffects : MonoBehaviour
{
    public Rigidbody2D ballBody;

    [Header("Launch Effect")]
    public float punchScale = 1.25f;
    public float punchDuration = 0.12f;

    [Header("Movement Effect")]
    public float stretchAmount = 0.08f;
    public float stretchSpeed = 8f;
    public float maxStretchSpeed = 8f;

    private Vector3 originalScale;
    private Coroutine currentPunch;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (ballBody == null)
            return;

        float speed = ballBody.linearVelocity.magnitude;

        Vector3 targetScale = originalScale;

        if (speed > 0.1f)
        {
            Vector2 direction = ballBody.linearVelocity.normalized;

            float stretch = Mathf.Clamp01(
                speed / maxStretchSpeed
            ) * stretchAmount;

            targetScale.x = originalScale.x * (1f + stretch);
            targetScale.y = originalScale.y * (1f - stretch);

            float angle = Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }

        if (currentPunch == null)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                targetScale,
                Time.deltaTime * stretchSpeed
            );
        }
    }

    public void PlayShotEffect()
    {
        if (currentPunch != null)
        {
            StopCoroutine(currentPunch);
        }

        currentPunch = StartCoroutine(PunchScale());
    }

    IEnumerator PunchScale()
    {
        float elapsed = 0f;
        Vector3 enlargedScale = originalScale * punchScale;

        while (elapsed < punchDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / punchDuration;

            transform.localScale = Vector3.Lerp(
                enlargedScale,
                originalScale,
                t
            );

            yield return null;
        }

        transform.localScale = originalScale;
        currentPunch = null;
    }
}
