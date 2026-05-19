using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button startButton;
    private Button quitButton;
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();

        startButton = uiDocument.rootVisualElement.Q<Button>("StartButton");
        quitButton = uiDocument.rootVisualElement.Q<Button>("QuitButton");

        if (startButton != null) startButton.clicked += StartGame;
        if (quitButton != null) quitButton.clicked += QuitGame;
    }

    void StartGame()
    {
        Time.timeScale = 1f; 
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
    }
    void QuitGame()
    {
        Debug.Log("Quit the Game...");
        Application.Quit();
    }
}
