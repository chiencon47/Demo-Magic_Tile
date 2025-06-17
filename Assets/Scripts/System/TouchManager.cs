using UnityEngine;
using System.Collections.Generic;

public class TouchManager : MonoBehaviour
{
    private Dictionary<int, LongNote> activeLongNotes = new Dictionary<int, LongNote>();

    private void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);
                    foreach (var hit in hits)
                    {
                        LongNote longNote = hit.GetComponent<LongNote>();
                        if (longNote != null)
                        {
                            longNote.OnNoteTouched();
                            activeLongNotes[touch.fingerId] = longNote;
                        }
                        else
                        {
                            hit.SendMessage("OnNoteTouched", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (activeLongNotes.TryGetValue(touch.fingerId, out LongNote longNoteEnd))
                    {
                        longNoteEnd.OnNoteReleased();
                        activeLongNotes.Remove(touch.fingerId);
                    }
                    break;
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);
            foreach (var hit in hits)
            {
                LongNote longNote = hit.GetComponent<LongNote>();
                if (longNote != null)
                {
                    longNote.OnNoteTouched();
                    activeLongNotes[-1] = longNote; 
                }
                else
                {
                    hit.SendMessage("OnNoteTouched", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (activeLongNotes.TryGetValue(-1, out LongNote longNoteEnd))
            {
                longNoteEnd.OnNoteReleased();
                activeLongNotes.Remove(-1);
            }
        }
#endif
    }
}
