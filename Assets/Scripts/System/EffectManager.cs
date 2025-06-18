using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private TextEffect effectGood;
    [SerializeField] private TextEffect effectGreat;
    [SerializeField] private TextEffect effectPerfect;
    [SerializeField] private Image glow;
    [SerializeField] private TextCombo txtCombo;
    private Vector3 spawnPosition;
    private TextEffect currentEffect;
    private TextCombo currentTxtCombo;
    private int countCombo = 0;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, spawnPoint.position);
        float zDistance = 10f;
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDistance));

    }

    public void SpawnEffectGood()
    {
        countCombo = 0;
        SpawnEffect(effectGood);
    }

    public void SpawnEffectGreat()
    {
        countCombo = 0;
        SpawnEffect(effectGreat);
    }

    public void SpawnEffectPerfect()
    {
        countCombo++;
        SpawnEffect(effectPerfect);
    }

    public void SpawnEffect(TextEffect effect)
    {
        if(currentEffect != null)
        {
            currentEffect.gameObject.SetActive(false);
        }
        var newEffect = SimpleObjectPool.Instance.GetObjectFromPool(effect, spawnPosition);
        currentEffect = newEffect;
        glow.DOFade(1f, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            glow.DOFade(0.5f, 0.2f);
        });

        if(countCombo >= 2)
        {
            if (currentTxtCombo != null)
            {
                currentTxtCombo.gameObject.SetActive(false);
            }
            var newTxtCombo = SimpleObjectPool.Instance.GetObjectFromPool(txtCombo, spawnPosition + Vector3.right * 1);
            newTxtCombo.ShowCombo(countCombo);
            currentTxtCombo = newTxtCombo;
        }
    }

    public void TurnOnLights()
    {
        glow.DOFade(1f, 0.1f);
    }

    public void TurnOffLights()
    {
        glow.DOFade(0f, 0.1f);
    }
}
