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
    [Header("Starter")]
    [SerializeField] private GameObject _progressBar;
    [Header("Gameplay")]
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _startButton;
    [Header("Menu")]
    [SerializeField] private TextMeshProUGUI _moneyUI;
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
        SetMoneyText(YandexGame.savesData.money);
        _isMenuOpen = false;
    }
    public void GameStart()
    {
        //_menuButton.GetComponent<RectTransform>().DOMoveY(1000f, 1f); Сделать анимацию пропадания наверх
        _menuButton.SetActive(false);
        _startButton.SetActive(false);

        _progressBar.SetActive(true);
    }
    public void MainUI()
    {
        _menuButton.SetActive(true);
        _startButton.SetActive(true);

        _progressBar.SetActive(false);
    }
    public void SwitchMenu()
    {
        Vector2 _startShift = new Vector2(-900, 0f);
        Vector2 _endShift = new Vector2(0, 0f);

        if (_isMenuOpen)
        {
            _menu.transform.DOLocalMoveX(0f, 0.9f).SetEase(Ease.InOutQuint).Play();
            _isMenuOpen = false;
        }
        else
        {
            _menu.transform.DOLocalMoveX(-900f, 0.9f).SetEase(Ease.InOutQuint).Play();
            _isMenuOpen = true;
        }
    }
    public void SetMoneyText(int money)
    {
        _moneyUI.text = money.ToString();
    }

    #region Game Status Screens
    public void WinScreen(bool isActive) { _winScreen.SetActive(isActive); }
    public void FailScreen(bool isActive) { _failScreen.SetActive(isActive); }
    #endregion
}
