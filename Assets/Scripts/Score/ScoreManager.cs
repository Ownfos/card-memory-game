using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScoreManager is a class that calculates and record scores.
// It also keeps the best score until now.
//
// Whenever a game ends, StageManager should call RecordFinalScore().
public class ScoreManager : MonoBehaviour
{
    // Event that triggers when score is modified
    public event EventHandler<int> OnScoreChange;

    // Property for score
    public int BestScore { get; private set; } = 0;
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

    public void RecordFinalScore()
    {
        if (Score > BestScore)
        {
            BestScore = Score;
        }
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void OnPairMatchHandler(object sender, EventArgs e)
    {
        Score += ((Stage)sender).MatchReward;
    }

    public void OnPairMismatchHandler(object sender, EventArgs e)
    {
        Score = Math.Max(0, Score - ((Stage)sender).MismatchPanelty);
    }
}
