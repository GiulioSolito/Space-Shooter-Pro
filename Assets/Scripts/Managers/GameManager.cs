using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool _IsGameOver { get; private set; }

    void OnEnable()
    {
        Player.onPlayerDeath += GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _IsGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void GameOver()
    {
        _IsGameOver = true;
    }

    void OnDisable()
    {
        Player.onPlayerDeath -= GameOver;
    }
}
