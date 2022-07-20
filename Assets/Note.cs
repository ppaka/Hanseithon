using System;

namespace Note
{
    public enum NoteType
    {
        Normal,
        Fast,
        Slow
    }

    public enum NoteEventType
    {
        Normal,
        Reverse
    }

    [Serializable]
    public class Note
    {
        public NoteType type = NoteType.Normal;
        public NoteEventType eventType = NoteEventType.Normal;
        public int startTime, endTime;
        public int number;
        public int pointIndex;
        public bool lastNote;
    }
}