
using UnityEngine;

public class BallSoundEffects : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip wallHitSound;
    public float minimumImpactSpeed = 0.5f;
    public float volumeMultiplier = 0.3f;
    public CameraShake cameraShake;
    public float minimumShakeSpeed = 2f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;
        
        if (impactSpeed >= minimumShakeSpeed && cameraShake != null)
        {
            cameraShake.Shake();
        }

        if (impactSpeed < minimumImpactSpeed)
            return;

        if (audioSource == null || wallHitSound == null)
            return;

        float volume = Mathf.Clamp01(
            impactSpeed * volumeMultiplier
        );

        audioSource.PlayOneShot(wallHitSound, volume);
    }
}
