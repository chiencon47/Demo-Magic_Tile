using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : Note
{
    private float holdTime = 0f;
    private bool isHolding = false;

    public override void OnNoteTouched()
    {
        isHolding = true;
        holdTime = 0f;
    }

    public void OnNoteReleased()
    {
        if (holdTime >= 1f)
        {
            Debug.Log("LongNote success!");
            Destroy(gameObject); // tính điểm
        }
        else
        {
            Debug.Log("LongNote failed!");
        }

        isHolding = false;
    }

    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
        }
    }
}
