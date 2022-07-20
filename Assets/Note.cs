public enum NoteType
{
    Normal,
    Fast,
    Slow
}

public class Note
{
    public NoteType type = NoteType.Normal;
    public int startTime, endTime;
    public int number;
    public int pointIndex;
}