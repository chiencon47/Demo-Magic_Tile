using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextCombo : MonoBehaviour
{
    private TextMeshPro txtCombo;

    private void Awake()
    {
        txtCombo = GetComponent<TextMeshPro>();
    }
    public void ShowCombo(int combo)
    {
        txtCombo.text = "x" + combo;
        txtCombo.DOFade(1, 0f);
        txtCombo.transform.DOScale(1.2f, 0.1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            txtCombo.transform.DOScale(1f, 0.2f).OnComplete(() =>
            {
                txtCombo.DOFade(0, 0.2f);
            });
        });
    }
    private void OnDisable()
    {
        DOTween.Kill(txtCombo);
        DOTween.Kill(txtCombo.transform);
    }
}