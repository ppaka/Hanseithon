using System.Collections.Generic;
using Note;
using UnityEngine;

public class InputPoint : MonoBehaviour
{
    public int number;
    public Queue<NoteType> typeQueue = new();
    public Queue<NoteEventType> eventQueue = new();
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public void PlayHitAnim()
    {
        animator.Play("HitAnimation");
    }
}
