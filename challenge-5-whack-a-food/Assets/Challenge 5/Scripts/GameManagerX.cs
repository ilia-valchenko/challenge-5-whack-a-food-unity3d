using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    private const float SpaceBetweenSquares = 2.5f;
    private const float MinValueX = -3.75f; //  x value of the center of the left-most square
    private const float MinValueY = -3.75f; //  y value of the center of the bottom-most square
    private const int InitialScoreValue = 0;

    private int _score;
    private int _lives;
    private float _spawnRate = 1.5f;
    private float _timeLeft = 60.0f;

    public bool isGameActive;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI livesText;
    public GameObject titleScreen;
    public Button restartButton;
    public List<GameObject> targetPrefabs;

    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        _spawnRate /= difficulty;
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        _score = 0;
        this.UpdateScore(InitialScoreValue);
        this.UpdateTimer(System.Convert.ToInt32(_timeLeft));
        titleScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            _timeLeft -= Time.deltaTime;
            // timerText.text = $"Timer: {timeLeft}";
            UpdateTimer(System.Convert.ToInt32(_timeLeft));

            if (_timeLeft < 0)
            {
                this.GameOver();
            }
        }
    }

    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(_spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = MinValueX + (RandomSquareIndex() * SpaceBetweenSquares);
        float spawnPosY = MinValueY + (RandomSquareIndex() * SpaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;
    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        scoreText.text = $"Score: {_score}";
    }

    public void UpdateTimer(int timeLeft)
    {
        timerText.text = $"Timer: {timeLeft}";
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
