using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    private int highScore = 0;

    [Header("Life System Settings")]
    public int maxLives = 3;
    private int currentLives;
    private bool isInvincible = false;
    public float invincibilityDuration = 2f;
    private SpriteRenderer spriteRenderer;

    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;

    [Header("Power-up Settings")]
    public bool hasShield = false;
    public GameObject shieldVisual;
    private Coroutine shieldCoroutine;

    Rigidbody2D rb;

    public UIDocument uiDocument;
    private Label scoreText;
    private Label liveText;
    private Label highScoreText;
    private Button restartButton;
    private Button menuButton;

    public GameObject explosionEffect;
    public GameObject boosterFlame;
    public GameObject borderParent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentLives = maxLives;

        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");

        liveText = uiDocument.rootVisualElement.Q<Label>("LivesLabel");
        UpdateLivesUI();

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
        highScoreText.style.display = DisplayStyle.None; 

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;

        menuButton = uiDocument.rootVisualElement.Q<Button>("MainMenuButton");
        if (menuButton != null)
        {
            menuButton.style.display = DisplayStyle.None;
            menuButton.clicked += GoToMainMenu;
        }

        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }
        Time.timeScale = 0f;
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
        if (isInvincible) return;

        currentLives--;
        UpdateLivesUI();

        if (currentLives > 0)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            StartCoroutine(BecomeInvincible());
        }
        else
        {
            GameOver();
        }
    }
    public void ActivateShield()
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldTimer(7f));
    }
    IEnumerator ShieldTimer(float duration)
    {
        hasShield = true;
        if (shieldVisual != null) shieldVisual.SetActive(true);

        yield return new WaitForSeconds(duration);

        hasShield = false;
        if (shieldVisual != null) shieldVisual.SetActive(false);

        shieldCoroutine = null;
    }
    void DeactivateShield()
    {
        hasShield = false;
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }
    }
    void GameOver()
    {
        int currentScore = Mathf.FloorToInt(score);
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        borderParent.SetActive(false);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        if (menuButton != null) menuButton.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex;

        Destroy(gameObject);
    }
    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float timer = 0;

        SpriteRenderer[] allRenderers = GetComponentsInChildren<SpriteRenderer>();

        while (timer < invincibilityDuration)
        {
            foreach (SpriteRenderer sr in allRenderers)
            {
                if (sr != null) sr.enabled = !sr.enabled;
            }
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        foreach (SpriteRenderer sr in allRenderers)
        {
            if (sr != null) sr.enabled = true;
        }

        isInvincible = false;
    }
    void UpdateLivesUI()
    {
        if (liveText != null)
        {
            liveText.text = "Lives: " + currentLives;
        }
    }
    void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
   
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}