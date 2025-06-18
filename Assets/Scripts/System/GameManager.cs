using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public float speed = 5;
    public float timeOffset = 1;

    [SerializeField] private AssetReference songData;
    [SerializeField] private Transform noteParent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI textScore;
    private bool isPlay = false;
    private float visualTime = 0f;
    private int score = 0;
    private int maxScore;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevelData(songData);
    }

    public bool IsPlaying()
    {
        return isPlay;
    }

    public void SetMaxScore(int maxScore)
    {
        this.maxScore = maxScore;
    }
    private void Update()
    {
        if (isPlay == false) return;
        float targetTime = audioSource.timeSamples / (float)audioSource.clip.frequency;
        visualTime = Mathf.Lerp(visualTime, targetTime, Time.deltaTime * 30f);
        noteParent.transform.position = Vector3.down * visualTime * speed;

        if (!audioSource.isPlaying && audioSource.time >= audioSource.clip.length - 0.5f)
        {
            GameOver();
        }
    }

    public void PlayeGame()
    {
        isPlay = true;
        DOVirtual.DelayedCall(timeOffset, () =>
        {
            audioSource.Play();

        });
    }

    public void LoadLevelData(AssetReference reference)
    {
        Addressables.LoadAssetAsync<TextAsset>(reference).Completed += OnLoadDone;
    }

    private void OnLoadDone(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string levelJson = obj.Result.text;
            SongData data = JsonUtility.FromJson<SongData>(levelJson);
            NoteGenerator.Instance.Prepare(data, noteParent);
        }
        else
        {
            Debug.LogError($"Failed to load level ");
        }
    }

    public float GetTimeSong()
    {
        return visualTime;
    }

    public void AddScore(int amount)
    {
        score += amount;
        textScore.text = score.ToString();
        textScore.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            textScore.transform.DOScale(1f, 0.1f);
        });
    }

    public void GameOver()
    {
        PopupManager.Instance.ShowPopupGameOver();
        audioSource.Pause();
        isPlay = false;
    }

}
