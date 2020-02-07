using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Sam Ferstein
 * GameManager.cs
 * Assignment 3
 * This is the game manager that also includes a list of observers for the Observer Design Pattern
 */

public class GameManager : MonoBehaviour, ISubject
{
    private List<IObserver> observerList = new List<IObserver>();
    private float scoreIncrease;

    public List<GameObject> targets;
    private float spawnRate = 1;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameWonText;
    private int score;
    public bool isGameActive;
    public bool isGameWon = false;
    public Button restartButton;
    public GameObject titleScreen;

    public void RegisterObserver(IObserver observer)
    {
        observerList.Add(observer);
        observer.UpdateData(score);
    }

    public void RemoveObserver(IObserver observer)
    {
        if(observerList.Contains(observer))
        {
            observerList.Remove(observer);
        }
    }

    public void NotifyObservers()
    {
        foreach (IObserver observer in observerList)
        {
            observer.UpdateData(score);
        }
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;

        if(score >= 150)
        {
            GameWon();
        }
    }

    public void GameWon()
    {
        isGameWon = true;
        restartButton.gameObject.SetActive(true);
        gameWonText.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        isGameWon = false;
        isGameActive = true;
        score = 0;
        spawnRate /= difficulty;

        StartCoroutine(SpawnTarget());
        UpdateScore(0);

        titleScreen.gameObject.SetActive(false);
    }

    public void changeScore()
    {
        score += 10;
        NotifyObservers();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            changeScore();
            Debug.Log("Score increase toggled.");
        }
    }
}
