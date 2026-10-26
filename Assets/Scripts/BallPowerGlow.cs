
using UnityEngine;

public class BallPowerGlow : MonoBehaviour
{
    public SpriteRenderer ballRenderer;
    public PowerController powerController;

    public Color lowPowerColour = Color.white;
    public Color highPowerColour = Color.red;

    private Color currentColour;

    void Update()
    {
        if (ballRenderer == null || powerController == null)
            return;

        float power = powerController.CurrentPower;

        currentColour = Color.Lerp(
            lowPowerColour,
            highPowerColour,
            power
        );

        ballRenderer.color = currentColour;
    }
}
