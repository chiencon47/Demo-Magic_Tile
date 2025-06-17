using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public float speed = 5;
    public float timeOffset = 1;

    [SerializeField] private AssetReference songData;
    [SerializeField] private Transform noteParent;
    [SerializeField] private AudioSource audioSource;
    private bool isPlay = false;
    private float visualTime = 0f;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevelData(songData);
    }


    private void Update()
    {
        if (isPlay == false) return;
        float targetTime = audioSource.timeSamples / (float)audioSource.clip.frequency;
        visualTime = Mathf.Lerp(visualTime, targetTime, Time.deltaTime * 30f);

        noteParent.transform.position = Vector3.down * visualTime * speed;
    }

    public void PlayeGame()
    {
        isPlay = true;
        audioSource.Play();
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
            NoteGenerator.Instance.Generator(data, noteParent);
        }
        else
        {
            Debug.LogError($"Failed to load level ");
        }
    }
}
