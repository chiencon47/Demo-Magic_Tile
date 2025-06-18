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

    float[] lastNoteTimes = new float[4]; 
    float minSpacing = 0.5f;
    public void Generator(SongData data, Transform noteParent)
    {
        float offset = GameManager.Instance.timeOffset;
        float speed = GameManager.Instance.speed;
        Note newStartNote = Instantiate(startNote, noteParent);
        Vector3 positon = lanePositions[Random.Range(0, 4)];
        positon.y = -3;
        newStartNote.transform.localPosition = positon;

        foreach (var key in data.keys)
        {
            if (key.id != 0 && key.id != 12) continue;

            Note prefab = (key.id == 0) ? shortNote : longNote;
            if (prefab == null) continue;

            float noteTime = key.time + offset;

            List<int> availableLanes = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (Mathf.Abs(noteTime - lastNoteTimes[i]) >= minSpacing)
                {
                    availableLanes.Add(i);
                }
            }

            int lane;

            if (availableLanes.Count == 0)
            {
                lane = Random.Range(0, 4);
            }
            else
            {
                lane = availableLanes[Random.Range(0, availableLanes.Count)];
            }

            lastNoteTimes[lane] = noteTime;

            Note noteInstance = Instantiate(prefab, noteParent);
            noteInstance.SetData(key);
            Vector3 pos = lanePositions[lane];
            pos.y = noteTime * speed;
            noteInstance.transform.localPosition = pos;
        }
    }


    private void UpdateSizeNote(Transform note)
    {
        if (note == null) return;

        BoxCollider2D box = note.GetComponent<BoxCollider2D>();
        if (box == null)
        {
            return;
        }

        float worldWidth = box.size.x * note.lossyScale.x;

        if (worldWidth <= 0f)
        {
            return;
        }

        float scaleFactor = laneWidth / worldWidth;

        note.localScale *= scaleFactor;
    }


}

