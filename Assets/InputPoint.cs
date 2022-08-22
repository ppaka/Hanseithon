using System.Collections.Generic;
using Note;
using UnityEngine;

public class InputPoint : MonoBehaviour
{
    public int number;
    public int ptr = 1;
    public List<NoteType> typeQueue = new();
    public List<NoteEventType> eventQueue = new();
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public void PlayHitAnim()
    {
        animator.Play("HitAnimation", -1, 0);
    }
}
