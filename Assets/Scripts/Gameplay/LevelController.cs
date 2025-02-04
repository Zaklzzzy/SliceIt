using DG.Tweening;
using DG.Tweening.Core;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Generator _generator;

    [Header("Movement")]
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speedIncreaseStep;
    private float _currentSpeed;
    private bool _isLevelRunning = false;

    [Header("Difficult")]
    [SerializeField] private float _currentPercentage;
    private float _step = 2.5f;
    private const float _maxPercentage = 20f;

    [Header("Score Settings")]
    [SerializeField] private Image _scoreFiller;
    [SerializeField] private ParticleSystem _successParticle;
    [SerializeField] private ParticleSystem _finishParticle;
    private float _score = 0;
    private int _maxScore = 0;
    private float _successThreshold = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _particleSystem.Stop();
        CalculateDifficult();
    }

    #region Movement
    private void Update()
    {
        if (_isLevelRunning)
        {
            MoveLevel();
        }
    }

    private void MoveLevel()
    {
        float step = _currentSpeed * Time.deltaTime;
        // Debug.Log("_currentSpeed " + _currentSpeed);
        transform.position += Vector3.left * step;

        if (transform.position.x <= _endPoint.position.x)
        {
            EndLevel();
        }
    }

    private void CalculateDifficult()
    {
        if (YandexGame.savesData.globalPercentage + 5f == _maxPercentage) YandexGame.savesData.globalPercentage = 15f;
        
        _currentPercentage = YandexGame.savesData.globalPercentage + (((YandexGame.savesData.level % 10) - 1) * _step);

        _currentSpeed = _baseSpeed + (_baseSpeed * (_currentPercentage / 100));

        YandexGame.SaveProgress();

        //Debug.Log("_currentPercentage - " + _currentPercentage);
        //Debug.Log("_currentSpeed - " + _currentSpeed);
    }
    #endregion

    #region Level Control
    public void StartLevel()
    {
        if (_isLevelRunning) return;

        _isLevelRunning = true;

        _maxScore = FindObjectsByType<Sliceable>(FindObjectsSortMode.None).Length;
        _successThreshold = _maxScore * 0.92f;

        KnifeController.Instance.isLevelStarted = true;
        _particleSystem.Play();

        UIManager.Instance.GameStart();
    }
    public void ResetLevel()
    {
        AudioManager.Instance.PlayButtonSound();

        transform.position = _startPoint.position;
        _isLevelRunning = false;

        CalculateDifficult();

        _generator.GenerateWithProperties();

        KnifeController.Instance.StopAnimation();

        ClearScore();

        UIManager.Instance.WinScreen(false);
        UIManager.Instance.FailScreen(false);
        UIManager.Instance.MainUI();

        YandexGame.FullscreenShow();
    }
    #endregion

    #region Win Status
    public void EndLevel()
    {
        if (!_isLevelRunning) return;

        _finishParticle.Play();

        _isLevelRunning = false;
        KnifeController.Instance.isLevelStarted = false;
        KnifeController.Instance.StopAnimation();
        _particleSystem.Stop();

        if (_score >= _successThreshold)
        {
            WinLevel();
        }
        else
        {
            FailLevel();
        }
    }
    private void WinLevel()
    {
        UIManager.Instance.WinScreen(true);
        AudioManager.Instance.PlayWinSound();

        // Calculate rewards
        int moneyReward = Random.Range(100 + (YandexGame.savesData.level * 6), 200 + (YandexGame.savesData.level * 6));

        UIManager.Instance.SetRewardCoins(moneyReward);

        // Save progress and advance to the next level
        YandexGame.NewLeaderboardScores("LevelLeaderboard", YandexGame.savesData.level);
        YandexGame.savesData.level++;
        YandexGame.savesData.money += moneyReward;
        YandexGame.SaveProgress();
    }
    public void FailLevel()
    {
        UIManager.Instance.FailScreen(true);
        AudioManager.Instance.PlayFailSound();

        _isLevelRunning = false;
        KnifeController.Instance.isLevelStarted = false;
        _particleSystem.Stop();
        gameObject.transform.DOKill();
    }
    #endregion

    #region Score
    public void AddScore(float addValue)
    {
        if (!_isLevelRunning) return;

        _score += addValue;
        _scoreFiller.fillAmount = (_score / _successThreshold) * 100 / 100;

        if (_score >= _successThreshold) _successParticle.Play();

        _currentSpeed = Mathf.Min(_currentSpeed + _speedIncreaseStep, _maxSpeed);
    }
    private void ClearScore()
    {
        _score = 0;
        _scoreFiller.fillAmount = 0;
    }
    #endregion
}