
using UnityEngine;

public class AimController : MonoBehaviour
{
    public float rotationSpeed = 180f;
    public PowerController powerController;

    public SpriteRenderer aimRenderer;
    public SpriteRenderer dotRenderer;

    public Color aimingColour = Color.white;
    public Color lockedColour = Color.green;

    private bool aiming = true;

    void Start()
    {
        SetAimVisible(true);
        SetAimColour(aimingColour);
    }

    void Update()
    {
        if (!aiming)
            return;

        transform.Rotate(
            Vector3.forward,
            rotationSpeed * Time.deltaTime
        );

        if (Input.GetKeyDown(KeyCode.Space))
        {
            aiming = false;
            SetAimColour(lockedColour);

            if (powerController != null)
            {
                powerController.StartCharging();
            }
        }
    }

    public void HideAim()
    {
        aiming = false;
        SetAimVisible(false);
    }

    public void ResetAim()
    {
        aiming = true;
        SetAimVisible(true);
        SetAimColour(aimingColour);
    }

    void SetAimVisible(bool visible)
    {
        if (aimRenderer != null)
            aimRenderer.enabled = visible;

        if (dotRenderer != null)
            dotRenderer.enabled = visible;
    }

    void SetAimColour(Color colour)
    {
        if (aimRenderer != null)
            aimRenderer.color = colour;

        if (dotRenderer != null)
            dotRenderer.color = colour;
    }
}
