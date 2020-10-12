using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _restartText;

    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprites;

    private WaitForSeconds _gameOverFlickerDelay = new WaitForSeconds(0.5f);

    void OnEnable()
    {
        Player.onUpdateScoreUI += UpdateScore;
        Player.onUpdateLivesUI += UpdateLives;
        Player.onPlayerDeath += ShowGameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];
    }

    void ShowGameOver()
    {
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return _gameOverFlickerDelay;
            _gameOverText.gameObject.SetActive(false);
            yield return _gameOverFlickerDelay;
        }
    }

    void OnDisable()
    {
        Player.onUpdateScoreUI -= UpdateScore;
        Player.onUpdateLivesUI -= UpdateLives;
        Player.onPlayerDeath -= ShowGameOver;
    }
}
