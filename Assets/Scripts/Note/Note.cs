using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public abstract class Note : MonoBehaviour
{
    protected SongKeyEvent songKeyEvent;
    protected bool isClick = false;
    public void OnNoteTouched()
    {
        if (isClick) return;
        isClick = true;
        OnTouched();
    }

    public abstract void OnTouched();

    public virtual void SetData(SongKeyEvent songKeyEvent)
    {
        this.songKeyEvent = songKeyEvent;
    }
    
    public virtual void Init()
    {
        isClick = false;
    }

    public bool IsClick => isClick;
}
