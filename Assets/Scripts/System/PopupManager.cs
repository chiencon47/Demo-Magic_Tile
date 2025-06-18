using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public CanvasGroup popupGameOver;
    public static PopupManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPopupGameOver()
    {
        popupGameOver.alpha = 0;
        popupGameOver.gameObject.SetActive(true);
        popupGameOver.DOFade(1, 0.5f);
    }
}
