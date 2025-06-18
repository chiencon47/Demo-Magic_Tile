using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartNote : Note
{
    public override void OnTouched()
    {
        GameManager.Instance.PlayeGame();
        gameObject.SetActive(false);
    }
}
