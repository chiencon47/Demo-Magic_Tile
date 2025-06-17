using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNote : Note
{
    public override void OnNoteTouched()
    {
        GameManager.Instance.PlayeGame();
        gameObject.SetActive(false);
    }
}
