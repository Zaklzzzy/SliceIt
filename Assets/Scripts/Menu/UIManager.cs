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
    [Header("Gameplay")]
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _progressBar;
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
    public void OpenMenu()
    {
        _gameplay.SetActive(false);
        _menu.SetActive(true);
    }
    public void CloseMenu()
    {
        _gameplay.SetActive(true);
        _menu.SetActive(false);
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
