using UnityEngine;
using YG;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopSlot[] _knifes;
    [SerializeField] private ShopSlot[] _sliceable;
    [SerializeField] private ShopSlot[] _worlds;
    
    private void Awake()
    {
        UpdateLockState(_knifes, "Knife");
        UpdateLockState(_sliceable, "Sliceable");
        UpdateLockState(_worlds, "Worlds");
    }

    private void Start()
    {
        SetMoneyCount(YandexGame.savesData.money);
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    private void UpdateLockState(ShopSlot[] array, string type)
    {
        bool[] saveType = { };
        var lockedCount = 0;
        switch (type)
        {
            case "Knife":
            {
                saveType = YandexGame.savesData.unlockKnifes;
                break;
            }
            case "Sliceable":
            {
                saveType = YandexGame.savesData.unlockSliceable;
                break;
            }
            case "Worlds":
            {
                saveType = YandexGame.savesData.unlockWorlds;
                break;
            }
        }
        
        for(int i = 0; i < array.Length; i++)
        {
            if (saveType[i])
            {
                array[i].Unlock();
            }
            else
            {
                lockedCount++;
            }
        }
    }

    public void SetMoneyCount(int newValue)
    {
        YandexGame.savesData.money = newValue;
        YandexGame.SaveProgress();
        UIManager.Instance.SetMoneyText(YandexGame.savesData.money);
    }

    #region RewardedVideo
    public void OpenRewardAd()
    {
        YandexGame.RewVideoShow(0);
    }
    private void Rewarded(int id)
    {
        YandexGame.savesData.money += 150;
        SetMoneyCount(YandexGame.savesData.money);
        YandexGame.SaveProgress();
    }
    #endregion
}
