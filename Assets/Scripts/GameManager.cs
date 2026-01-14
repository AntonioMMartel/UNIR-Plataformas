using System;
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
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject UI;

    private bool waitingForPlayerInput = false;

    public static event Action LevelStarts;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "0";
        Coin.OnCoinCollect += IncreaseScore; // Suscripción a CoinHub
        Flag.OnFlagCollect += LoadNextLevel; // Suscripción a OnlyFlags
        PlayerHP.OnPlayerLose += GameOver;
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

    void GameOver()
    {
        UI.SetActive(false);
        gameOverCanvas.SetActive(true);
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
            if (Input.anyKeyDown)             
            {
                waitingForPlayerInput = false;
                nextLevelCanvas.SetActive(false);
                gameOverCanvas.SetActive(false);
                Time.timeScale = 1f;
                LevelStarts.Invoke();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnDisable()
    {
        Coin.OnCoinCollect -= IncreaseScore;
        Flag.OnFlagCollect -= LoadNextLevel;
        PlayerHP.OnPlayerLose -= GameOver;
    }
}
