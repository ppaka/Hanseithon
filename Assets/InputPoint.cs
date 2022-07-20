using System.Collections.Generic;
using UnityEngine;

public class InputPoint : MonoBehaviour
{
    public int number;
    public Queue<NoteType> typeQueue = new();
    public SpriteRenderer spriteRenderer;
}
