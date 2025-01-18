using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Containers")]
    [SerializeField] private GameObject _gameplay;
    [SerializeField] private GameObject _menu;
    private bool _isMenuOpen;
    [Header("Gameplay")]
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _coins;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _gameMoneyUI;
    [Header("Menu")]
    [SerializeField] private TextMeshProUGUI _menuMoneyUI;
    [Header("Game Status Screens")]
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _failScreen;
    // Декорации для менюшной сцены

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _isMenuOpen = false;

        SetDefaultMenuPosition();
    }
    public void GameStart()
    {
        _menuButton.SetActive(false);
        _startButton.SetActive(false);
        _coins.SetActive(false);

        _progressBar.SetActive(true);
    }
    public void GameEnd()
    {
        _progressBar.SetActive(false);
    }
    public void MainUI()
    {
        _menuButton.SetActive(true);
        _startButton.SetActive(true);
        _coins.SetActive(true);

        _progressBar.SetActive(false);
    }
    private void SetDefaultMenuPosition()
    {
        _menu.transform.localPosition = new Vector3(-Screen.width, 0f, 0f);
    }
    public void SwitchMenu()
    {
        if (_isMenuOpen)
        {
            _menu.transform.DOLocalMoveX(0f, 0.9f).SetEase(Ease.InOutQuint).Play();
            _isMenuOpen = false;
        }
        else
        {
            _menu.transform.DOLocalMoveX(-Screen.width, 0.9f).SetEase(Ease.InOutQuint).Play();
            _isMenuOpen = true;
        }
    }
    public void SetLevelText(int level)
    {
        _levelText.text = level.ToString() + " " + _levelText.text.Split(' ')[1];
    }
    public void SetMoneyText(int money)
    {
        _menuMoneyUI.text = ConvertDigits(money.ToString());
        _gameMoneyUI.text = ConvertDigits(money.ToString());
    }

    #region Game Status Screens
    public void WinScreen(bool isActive) 
    {
        GameEnd();
        _winScreen.SetActive(isActive); 
    }
    public void FailScreen(bool isActive) 
    {
        GameEnd();
        _failScreen.SetActive(isActive); 
    }
    #endregion

    #region Text
    public string ConvertDigits(string number)
    {
        string result = "";
        switch (number.Length)
        {
            case 4:
                result = number[0].ToString() + "," + number[1].ToString() + "K";
                break;
            case 5:
                result = number[0].ToString() + number[1].ToString() + "," + number[2].ToString() + "K";
                break;
            case 6:
                result = number[0].ToString() + number[1].ToString() + number[2].ToString() + "," + number[3].ToString() + "K";
                break;
            default:
                result = number;
                break;
        }

        return result;
    }
    #endregion
}
