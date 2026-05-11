using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;

    private int highScore = 0;
    private Label highScoreText;

    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;

    Rigidbody2D rb;

    public UIDocument uiDocument;
    private Label scoreText;
    private Button restartButton;
    public GameObject explosionEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");

        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        highScoreText.text = "High Score: " + highScore;
        highScoreText.style.display = DisplayStyle.None; // ADDED

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;

        highScoreText.text = "High Score: " + highScore;

        if (Mouse.current.leftButton.isPressed)
        {

            // Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        int currentScore = Mathf.FloorToInt(score);

        if (currentScore > highScore)
        {
            highScore = currentScore;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex; // ADDED
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}