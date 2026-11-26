using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Monedas")]
    public int carriedCoins = 0;
    public int depositedCoins = 0;

    [Header("Contra el tiempo")]
    public float timeLimit = 60f;
    private float timeRemaining;
    private bool isGameOver = false;

    [Header("HUD (TextMeshPro)")]
    public TMP_Text carriedCoinsText;
    public TMP_Text depositedCoinsText;
    public TMP_Text timerText;
    public TMP_Text statusMessageText;

    private Coroutine messageCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        timeRemaining = timeLimit;
        isGameOver = false;
        if (statusMessageText != null)
            statusMessageText.text = string.Empty;

        UpdateHUD();
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame();
        }

        UpdateTimerUI();
    }

    public void AddCoin(int amount = 1)
    {
        if (isGameOver) return;

        carriedCoins += amount;
        UpdateHUD();
    }

    public int DepositAllCoins()
    {
        if (isGameOver || carriedCoins <= 0) return 0;

        int amount = carriedCoins;
        depositedCoins += amount;
        carriedCoins = 0;
        UpdateHUD();
        ShowMessage($"+{amount} monedas depositadas en el baúl");
        return amount;
    }

    public void ShowMessage(string message, float duration = 2.5f)
    {
        if (statusMessageText == null) return;

        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        statusMessageText.text = message;
        if (!isGameOver)
            messageCoroutine = StartCoroutine(ClearMessageAfterDelay(duration));
    }

    private IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (statusMessageText != null && !isGameOver)
            statusMessageText.text = string.Empty;
    }

    private void EndGame()
    {
        isGameOver = true;
        UpdateTimerUI();
        ShowMessage($"¡TIEMPO AGOTADO!\nMonedas en baúl: {depositedCoins}\nPulsa R para reiniciar");
    }

    private void UpdateHUD()
    {
        if (carriedCoinsText != null)
            carriedCoinsText.text = $"Monedas: {carriedCoins}";

        if (depositedCoinsText != null)
            depositedCoinsText.text = $"Baúl: {depositedCoins}";

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int seconds = Mathf.CeilToInt(timeRemaining);
        int mins = seconds / 60;
        int secs = seconds % 60;
        timerText.text = $"Tiempo: {mins:00}:{secs:00}";
    }
}
