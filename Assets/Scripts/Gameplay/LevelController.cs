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

    [Header("Score Settings")]
    [SerializeField] private Image _scoreFiller;
    private float _score = 0;
    private int _maxScore = 0;

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
        _currentSpeed = _baseSpeed;
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
        transform.position += Vector3.left * step;

        if (transform.position.x <= _endPoint.position.x)
        {
            EndLevel();
        }
    }
    #endregion

    #region Level Control
    public void StartLevel()
    {
        if (_isLevelRunning) return;

        _isLevelRunning = true;
        
        _maxScore = FindObjectsByType<Sliceable>(FindObjectsSortMode.None).Length;

        KnifeController.Instance.isLevelStarted = true;
        _particleSystem.Play();

        UIManager.Instance.GameStart();
    }
    public void ResetLevel()
    {
        AudioManager.Instance.PlayButtonSound();

        transform.position = _startPoint.position;
        _isLevelRunning = false;
        _currentSpeed = _baseSpeed;

        _generator.GenerateWithProperties();

        KnifeController.Instance.StopAnimation();

        ClearScore();

        UIManager.Instance.WinScreen(false);
        UIManager.Instance.FailScreen(false);
        UIManager.Instance.MainUI();
    }
    #endregion

    #region Win Status
    public void EndLevel()
    {
        if (!_isLevelRunning) return;

        _isLevelRunning = false;
        KnifeController.Instance.isLevelStarted = false;
        KnifeController.Instance.StopAnimation();
        _particleSystem.Stop();

        float successThreshold = _maxScore * 0.8f; // 4/5 of max score

        Debug.Log("_score = " + _score);
        Debug.Log("successThreshold = " + successThreshold);

        if (_score >= successThreshold)
        {
            // Start end animation
            WinLevel();
        }
        else
        {
            // Start end animation
            FailLevel();
        }
    }
    private void WinLevel()
    {
        UIManager.Instance.WinScreen(true);
        AudioManager.Instance.PlayWinSound();

        // Calculate rewards
        int moneyReward = Random.Range(19, 22);

        UIManager.Instance.SetRewardCoins(moneyReward);

        // Save progress and advance to the next level
        YandexGame.savesData.level++;
        YandexGame.savesData.money += moneyReward; // Учесть это в UI, оставить по +20 или так, коммент чтобы не забыть!
        YandexGame.SaveProgress();
    }
    public void FailLevel()
    {
        //if (!_isLevelRunning) return;
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
        _scoreFiller.fillAmount = (_score / (_maxScore * 0.8f)) * 100 / 100;

        _currentSpeed = Mathf.Min(_currentSpeed + _speedIncreaseStep, _maxSpeed);
    }
    private void ClearScore()
    {
        _score = 0;
        _scoreFiller.fillAmount = 0;
    }
    #endregion
}