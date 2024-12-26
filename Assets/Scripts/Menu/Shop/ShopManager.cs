using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopSlot[] _knifes;
    // Add other types
    [SerializeField] private int money;
    
    private void Awake()
    {
        UpdateLockState(_knifes, "Knife");
        // Add other types
    }

    private void UpdateLockState(ShopSlot[] array, string type)
    {
        bool[] saveType = { };
        var lockedCount = 0;
        switch (type) // Add other types
        {
            case "Knife":
                {
                    saveType = YandexGame.savesData.unlockKnifes;
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

    public int GetMoney() { return money; }
    public void SetMoneyCount(int newValue)
    {
        money = newValue;
        YandexGame.savesData.money = money;
        YandexGame.SaveProgress();
        FindAnyObjectByType<UIManager>().SetMoneyText(money);
    }
}
