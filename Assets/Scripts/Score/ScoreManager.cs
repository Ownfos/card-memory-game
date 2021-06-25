using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Event that triggers when score is modified
    public event EventHandler<int> OnScoreChange;

    // Property for score
    public int Score
    {
        get { return _score; }
        set
        { 
            if (_score != value)
            {
                _score = value;
                OnScoreChange?.Invoke(this, _score);
            }
        } 
    }
    private int _score = 0;

    // Amount of score to deduct when pair mismatch happens
    private int mismatchPanelty = 1;

    // Amount of score to add when pair match happends
    private int matchReward = 20;

    public void ResetScore()
    {
        Score = 0;
    }

    public void OnPairMatchHandler(object sender, EventArgs e)
    {
        Score += matchReward;
    }

    public void OnPairMismatchHandler(object sender, EventArgs e)
    {
        Score = Math.Max(0, Score - mismatchPanelty);
    }
}
