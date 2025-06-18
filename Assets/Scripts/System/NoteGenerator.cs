using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    public static NoteGenerator Instance { get; private set; }

    [SerializeField] private Note shortNote;
    [SerializeField] private Note longNote;
    [SerializeField] private Note startNote;

    private float laneWidth;
    private Vector3[] lanePositions;

    private float speed;
    private float offset;

    private Queue<NoteSpawnInfo> noteQueue = new Queue<NoteSpawnInfo>();
    private Transform noteParent;

    private void Awake()
    {
        Instance = this;
        InitLanePositions();
    }

    private void InitLanePositions()
    {
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        lanePositions = new Vector3[4];
        laneWidth = screenWidth / 4f;
        float startX = -screenWidth / 2f + laneWidth / 2f;

        for (int i = 0; i < 4; i++)
        {
            lanePositions[i] = new Vector3(startX + i * laneWidth, 0f, 0f);
        }

        UpdateSizeNote(startNote.transform);
        UpdateSizeNote(shortNote.transform);
        UpdateSizeNote(longNote.transform);
    }

    public void Prepare(SongData data, Transform parent)
    {
        speed = GameManager.Instance.speed;
        offset = GameManager.Instance.timeOffset;
        noteParent = parent;

        float[] lastNoteTimes = new float[4];
        float minSpacing = 0.5f;

        // Spawn 1 start note
        Note start = Instantiate(startNote, noteParent);
        Vector3 pos = lanePositions[Random.Range(0, 4)];
        pos.y = -3f;
        start.transform.localPosition = pos;

        foreach (var key in data.keys)
        {
            if (key.id != 0 && key.id != 12) continue;

            Note prefab = (key.id == 0) ? shortNote : longNote;
            if (prefab == null) continue;

            float noteTime = key.time + offset;

            // Chọn lane không trùng thời gian
            List<int> availableLanes = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (Mathf.Abs(noteTime - lastNoteTimes[i]) >= minSpacing)
                    availableLanes.Add(i);
            }

            int lane = (availableLanes.Count == 0)
                ? Random.Range(0, 4)
                : availableLanes[Random.Range(0, availableLanes.Count)];

            lastNoteTimes[lane] = noteTime;

            NoteSpawnInfo info = new NoteSpawnInfo
            {
                prefab = prefab,
                laneIndex = lane,
                time = noteTime,
                data = key
            };

            noteQueue.Enqueue(info);
        }
    }

    private float preSpawnTime = 5f; 

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        float currentTime = GameManager.Instance.GetTimeSong();

        while (noteQueue.Count > 0 && noteQueue.Peek().time <= currentTime + preSpawnTime)
        {
            SpawnNote(noteQueue.Dequeue());
        }
    }

    private void SpawnNote(NoteSpawnInfo info)
    {
        Note note = SimpleObjectPool.Instance.GetObjectFromPool(info.prefab, noteParent);
        Vector3 pos = lanePositions[info.laneIndex];
        pos.y = info.time * speed;
        note.transform.localPosition = pos;
        note.SetData(info.data);
    }

    private void UpdateSizeNote(Transform note)
    {
        if (note == null) return;

        BoxCollider2D box = note.GetComponent<BoxCollider2D>();
        if (box == null) return;

        float worldWidth = box.size.x * note.lossyScale.x;
        if (worldWidth <= 0f) return;

        float scaleFactor = laneWidth / worldWidth;
        note.localScale *= scaleFactor;
    }

    private class NoteSpawnInfo
    {
        public Note prefab;
        public int laneIndex;
        public float time;
        public SongKeyEvent data;
    }
}
