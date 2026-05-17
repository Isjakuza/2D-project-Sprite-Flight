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
    public GameObject boosterFlame;
    public GameObject borderParent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");

        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        highScoreText.text = "High Score: " + highScore;
        highScoreText.style.display = DisplayStyle.None; 

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

        // MOBILE + PC INPUT
        bool isPressing = false;
        Vector2 pointerPosition = Vector2.zero;

        // TOUCH
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            isPressing = true;
            pointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        // MOUSE
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            isPressing = true;
            pointerPosition = Mouse.current.position.ReadValue();
        }

        // BOOSTER ON
        if (isPressing)
        {
            if (!boosterFlame.activeSelf)
            {
                boosterFlame.SetActive(true);
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(pointerPosition);
            mousePos.z = 0f;

            Vector2 direction = (mousePos - transform.position).normalized;

            transform.up = direction;

            rb.AddForce(direction * thrustForce);

            // LIMIT SPEED
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, 10f);
        }

        // BOOSTER OFF
        else
        {
            if (boosterFlame.activeSelf)
            {
                boosterFlame.SetActive(false);
            }
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
        borderParent.SetActive(false);
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex; 
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}