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
    private int _score = 0;
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
        transform.position = _startPoint.position;
        _isLevelRunning = false;
        _currentSpeed = _baseSpeed;

        _generator.GenerateWithProperties();


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
        _particleSystem.Stop();

        float successThreshold = _maxScore * 0.8f; // 4/5 of max score

        if (_score >= successThreshold)
        {
            // Start end animation
            //WinLevel();
        }
        else
        {
            // Start end animation
            // FailLevel();
        }
    }
    private void WinLevel()
    {
        UIManager.Instance.WinScreen(true);

        // Calculate rewards
        int moneyReward = Mathf.FloorToInt(_score / 100f);

        // Save progress and advance to the next level
        YandexGame.savesData.level++;
        YandexGame.savesData.money += moneyReward;
        YandexGame.SaveProgress();
    }
    public void FailLevel()
    {
        if (!_isLevelRunning) return;

        _isLevelRunning = false;
        KnifeController.Instance.isLevelStarted = false;
        _particleSystem.Stop();
        gameObject.transform.DOKill();

        UIManager.Instance.FailScreen(true);
    }
    #endregion

    #region Score
    public void AddScore(int addValue)
    {
        if (!_isLevelRunning) return;

        _score += addValue;
        _scoreFiller.fillAmount = Mathf.Clamp01((float)_score / _maxScore); // Fix this

        _currentSpeed = Mathf.Min(_currentSpeed + _speedIncreaseStep, _maxSpeed);
    }
    private void ClearScore()
    {
        _score = 0;
        _scoreFiller.fillAmount = 0;
    }
    #endregion
}