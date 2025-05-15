[System.Serializable]
public class ScoreEntry
{
    public string sessionId;
    public int score;
    public long timestamp;

    public ScoreEntry() { }

    public ScoreEntry(string sessionId, int score, long timestamp)
    {
        this.sessionId = sessionId;
        this.score = score;
        this.timestamp = timestamp;
    }
}