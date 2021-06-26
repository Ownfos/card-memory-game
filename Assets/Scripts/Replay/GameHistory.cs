using System;

[System.Serializable]
public struct GameHistory
{
    public int score;
    public DateTime dateTime;
    public ReplayBuffer replayBuffer;

    public GameHistory(int score, ReplayBuffer replayBuffer)
    {
        dateTime = DateTime.Now;
        this.score = score;
        this.replayBuffer = replayBuffer;
    }
}