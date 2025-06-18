using DG.Tweening;
using UnityEngine;

public class ShortNote : Note
{
    [SerializeField] private SpriteRenderer fill;
    [SerializeField] private SpriteRenderer boder;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void OnTouched()
    {
        ShowEffect();
        CalculateScore();
    }

    public void ShowEffect()
    {
        fill.gameObject.SetActive(true);
        boder.gameObject.SetActive(true);
        spriteRenderer.enabled = false;
        boder.transform.DOScale(1.1f, 0.3f).SetEase(Ease.OutBack);
        boder.DOFade(0, 0.5f);
        fill.DOFade(0, 0.5f);
        fill.transform.DOScale(0.3f, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public override void SetData(SongKeyEvent songKeyEvent)
    {
        base.SetData(songKeyEvent);
        Init();
    }
    public override void Init()
    {
        base.Init();
        fill.gameObject.SetActive(false);
        boder.gameObject.SetActive(false);
        spriteRenderer.enabled = true;
        fill.transform.localScale = Vector3.one;
        boder.transform.localScale = Vector3.one;
        boder.DOFade(1, 0f);
        fill.DOFade(1, 0f);
    }
    private void CalculateScore()
    {
        float timingOffset = Mathf.Abs(GameManager.Instance.GetTimeSong() - songKeyEvent.time);
        Debug.Log(timingOffset);
        if (timingOffset <= 0.9f)
        {
            GameManager.Instance.AddScore(3);
            EffectManager.Instance.SpawnEffectPerfect();
        }
        else if (timingOffset <= 1.1f)
        {
            GameManager.Instance.AddScore(2);
            EffectManager.Instance.SpawnEffectGreat();
        }
        else
        {
            GameManager.Instance.AddScore(1);
            EffectManager.Instance.SpawnEffectGood();
        }
    }

}
