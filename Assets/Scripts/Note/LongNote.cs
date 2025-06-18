using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : Note
{
    [SerializeField] private Transform star;
    [SerializeField] private Transform mask;
    [SerializeField] private Transform subMask;
    [SerializeField] private ParticleSystem touchFx;
    private float holdTime = 0f;
    private bool isHolding = false;
    private float percent;
    private float height;
    private Vector3 touchWorldPos;
    public void SetTouchWorldPos(Vector2 touchWorldPos)
    {
        this.touchWorldPos = touchWorldPos;
        Vector2 localPos = transform.InverseTransformPoint(touchWorldPos);
        height = GetComponent<Collider2D>().bounds.size.y;
        percent = localPos.y/1.5f;
    }

    public override void OnTouched()
    {
        isHolding = true;
        holdTime = 0f;
        mask.gameObject.SetActive(true);
        subMask.gameObject.SetActive(true);
        touchFx.gameObject.SetActive(true);
        EffectManager.Instance.TurnOnLights();

    }

    public void OnNoteReleased()
    {
        if (isHolding == false) return; 
        isHolding = false;
        touchFx.gameObject.SetActive(false);
        EffectManager.Instance.TurnOffLights();
        CalculateScore();

    }

    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
            UpdateMask();
        }
    }

    private float smoothPercent = 0f;
    private void UpdateMask()
    {
        float duration = height / GameManager.Instance.speed;
        float targetPercent = Mathf.Clamp01(percent + (holdTime / duration));

        smoothPercent = Mathf.Lerp(smoothPercent, targetPercent, Time.deltaTime * 30);

        mask.transform.localScale = new Vector3(mask.localScale.x, smoothPercent, mask.localScale.z);
        subMask.transform.localPosition = Vector3.up * (smoothPercent * 2f);
    }

    public override void SetData(SongKeyEvent songKeyEvent)
    {
        base.SetData(songKeyEvent);
        Init();
        
    }
    private void SetScaleFromDuration(float duration, float speed)
    {
        float targetHeight = duration * speed;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        float originalHeight = col.size.y;

        float newScaleY = targetHeight / originalHeight;

        float scaleFactor = newScaleY / transform.localScale.y;

        transform.localScale = new Vector3(
            transform.localScale.x,
            newScaleY,
            transform.localScale.z
        );

        star.localScale = new Vector3(star.localScale.x, star.localScale.y / scaleFactor, star.localScale.z);
        subMask.localScale = new Vector3(subMask.localScale.x, subMask.localScale.y / scaleFactor, subMask.localScale.z);
    }

    public override void Init()
    {
        base.Init();
        SetScaleFromDuration(songKeyEvent.duration, GameManager.Instance.speed);
        mask.gameObject.SetActive(false);
        subMask.gameObject.SetActive(false);
    }

    private void CalculateScore()
    {
        if (songKeyEvent.time <= 0f) return; 

        float ratioHold = holdTime / songKeyEvent.duration;
        Debug.Log(ratioHold);
        if (ratioHold >= 0.7f)
        {
            GameManager.Instance.AddScore(15);
            EffectManager.Instance.SpawnEffectPerfect();
        }
        else if (ratioHold >= 0.5f)
        {
            GameManager.Instance.AddScore(10);
            EffectManager.Instance.SpawnEffectGreat();
        }
        else
        {
            GameManager.Instance.AddScore(5);
            EffectManager.Instance.SpawnEffectGood();
        }
    }

}
