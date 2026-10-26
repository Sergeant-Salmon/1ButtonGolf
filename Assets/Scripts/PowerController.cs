
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerController : MonoBehaviour
{
    public Slider powerBar;
    public float powerSpeed = 1f;
    public Rigidbody2D ball;
    public Transform aimPivot;
    public float maxShotForce = 8f;
    public int par = 3;
    public TMP_Text resultText;
    public TMP_Text completionMessage;
    public BallEffects ballEffects;
    public GameObject restartButton;
    public Image powerFill;
    public Color lowPowerColour = Color.green;
    public Color mediumPowerColour = Color.yellow;
    public Color highPowerColour = Color.red;

    public TMP_Text shotCounterText;

    private int shots = 0;
    private bool charging = false;
    private bool locked = false;
    private bool waitingForBall = false;
    private float stoppedTimer = 0f;

    public float stopSpeed = 0.08f;
    public float stopDelay = 0.7f;


    public float CurrentPower { get; private set; }

    void Update()
    {
        if (waitingForBall)
        {
            if (ball.linearVelocity.magnitude < stopSpeed)
            {
                stoppedTimer += Time.deltaTime;

                if (stoppedTimer >= stopDelay)
                {
                    waitingForBall = false;
                    stoppedTimer = 0f;
                    StartNextShot();
                }
            }
            else
            {
                stoppedTimer = 0f;
            }

            return;
        }

        if (!charging || locked)
            return;

        float power = Mathf.PingPong(Time.time * powerSpeed, 1f);
        CurrentPower = power;
        powerBar.value = power;

        if (powerFill != null)
        {
            if (power < 0.4f)
                powerFill.color = lowPowerColour;
            else if (power < 0.75f)
                powerFill.color = mediumPowerColour;
            else
                powerFill.color = highPowerColour;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            locked = true;
            Shoot(powerBar.value);
        }
    }

    public void StartCharging()
    {
        charging = true;
        locked = false;
    }

    void Shoot(float power)
    {
        Vector2 direction = aimPivot.right;

        AimController aim = aimPivot.GetComponent<AimController>();

        if (aim != null)
        {
            aim.HideAim();
        }

        ball.AddForce(
            direction * power * maxShotForce,
            ForceMode2D.Impulse
        );

        if (powerFill != null)
        {
            powerFill.color = Color.white;
        }

        if (ballEffects != null)
        {
            ballEffects.PlayShotEffect();
        }

        shots++;
        UpdateShotCounter();

        charging = false;
        waitingForBall = true;
        stoppedTimer = 0f;
    }

    void StartNextShot()
    {
        charging = false;
        locked = false;

        AimController aim = aimPivot.GetComponent<AimController>();

        if (aim != null)
        {
            aim.ResetAim();
        }

        Debug.Log("Ball stopped. Ready for the next shot.");
    }

    void UpdateShotCounter()
    {
        if (shotCounterText != null)
        {
            shotCounterText.text = "Shots: " + shots;
        }
    }

    
public void CompleteHole()
{
    charging = false;
    locked = true;
    waitingForBall = false;

    ball.linearVelocity = Vector2.zero;
    ball.angularVelocity = 0f;

    string result;

    if (shots == 1)
        result = "HOLE IN ONE!";
    else if (shots < par)
        result = "BIRDIE!";
    else if (shots == par)
        result = "PAR!";
    else
        result = "BOGEY!";

    if (completionMessage != null)
    {
        completionMessage.gameObject.SetActive(true);
        completionMessage.text =
            "HOLE COMPLETED!\nShots: " + shots;
    }

    if (resultText != null)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = result;
    }

    if (restartButton != null)
    {
        restartButton.SetActive(true);
    }

    Debug.Log(result + " Completed in " + shots + " shots.");
}


public void AddPenaltyStroke()
{
    shots++;
    UpdateShotCounter();

    Debug.Log("Water hazard! Penalty stroke added.");
}


}
