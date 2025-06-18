using UnityEngine;

public class Boder : MonoBehaviour
{
    [SerializeField] private float offsetY = 0.2f; 

    void Start()
    {
        MoveToBottom();
    }

    void MoveToBottom()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float bottomY = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, cam.nearClipPlane)).y;

        Vector3 newPos = transform.position;
        newPos.y = bottomY + offsetY;
        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Note>().IsClick == false)
        {
            GameManager.Instance.GameOver();
        }
    }
}
