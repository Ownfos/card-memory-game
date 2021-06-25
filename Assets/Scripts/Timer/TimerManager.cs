using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    // Event that triggers when remaining time is less than zero
    public event EventHandler OnTimerEnd;

    // The color of timer UI's background bar.
    // Final color is interpolated between full and empty color.
    //
    // Example)
    //  100% time remaining -> timerFullColor
    //  50% time remaining -> mix of timerFullColor and timerEmptyColor
    //  0% time remaining -> timerEmptyColor
    [SerializeField] private Color timerFullColor;
    [SerializeField] private Color timerEmptyColor;

    // Properties for max and current timer value
    public float MaxTime { get; private set; } = 0.0f;
    public float RemainingTime
    {
        get
        {
            return _remainingTime;
        }
        private set
        {
            _remainingTime = value;

            // Resize fill bar
            UpdateUI(_remainingTime / MaxTime);

            // Trigger timer end event
            if (_remainingTime < 0.0f)
            {
                OnTimerEnd?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    private float _remainingTime = 0.0f;

    // Color controller for background bar
    private ImageColorController backgroundColorController;

    // Scale controller for fill bar
    private ImageScaleController fillBarScaleController;

    // Find the components related to timer UI (background and fill bar)
    private void Awake()
    {
        backgroundColorController = GetComponentInChildren<ImageColorController>();
        fillBarScaleController = GetComponentInChildren<ImageScaleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RemainingTime > 0.0f)
        {
            RemainingTime -= Time.deltaTime;
        }
    }

    // Set the remaning time to duration
    public void StartTimer(float duration)
    {
        MaxTime = duration;
        RemainingTime = duration;
    }

    // Update fill bar and background bar of timer UI
    private void UpdateUI(float remaningTimeRatio)
    {
        // Interpolate background bar's color between timerFullColor and timerEmptyColor
        backgroundColorController.SetImageColor(Color.Lerp(timerEmptyColor, timerFullColor, remaningTimeRatio));

        // Change the y-scale of fill bar w.r.t. remaning time ratio (0.0 ~ 1.0)
        fillBarScaleController.SetImageScale(new Vector3(1.0f, remaningTimeRatio, 1.0f));
    }
}
