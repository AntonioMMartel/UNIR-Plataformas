using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] List<GameObject> levels;
    private int currentLevel = 0;
    [SerializeField] GameObject nextLevelCanvas;
    [SerializeField] GameObject UI;

    private bool waitingForPlayerInput = false;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "0";
        Coin.OnCoinCollect += IncreaseScore; // Suscripción a CoinHub
        Flag.OnFlagCollect += LoadNextLevel; // Suscripción a OnlyFlags
        nextLevelCanvas.SetActive(false);
        UI.SetActive(true);
    }

    void LoadNextLevel()
    {
        UI.SetActive(false);
        nextLevelCanvas.SetActive(true);
        Time.timeScale = 0f;

        waitingForPlayerInput = true;


    }

    void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForPlayerInput)
        {
            if (Input.anyKeyDown) // Me da pereza poderosa hacerlo con el InputSystem (lo siento jefe!)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                waitingForPlayerInput = false;
                nextLevelCanvas.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}
