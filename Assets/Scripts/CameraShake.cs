
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.12f;
    public float shakeMagnitude = 0.08f;

    private Vector3 originalPosition;
    private Coroutine shakeRoutine;

    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        transform.localPosition = originalPosition;
        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPosition
                + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        shakeRoutine = null;
    }
}
