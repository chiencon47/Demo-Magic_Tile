using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonReplay : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnReplayButtonClicked);
    }
    public void OnReplayButtonClicked()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
