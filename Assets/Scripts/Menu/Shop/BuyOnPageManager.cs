using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public enum Category { Knife, Sliceable, World }

public class BuyOnPageManager : MonoBehaviour
{
    [SerializeField] private Category _category;
    [SerializeField] private ShopSlot[] _slots;
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _unlockText;
    [SerializeField] private int _price;

    private ShopManager _shopManager;

    private void Awake()
    {
        _shopManager = FindAnyObjectByType<ShopManager>();
        DisableBuyButtons();
    }

    public void BuyRandom()
    {
        if (_shopManager.GetMoney() >= _price)
        {
            List<int> lockedItems = new List<int>();
            if (_category == Category.Knife)
            {
                lockedItems = CheckUnlockedItems(lockedItems, _category);

                if (lockedItems.Count > 0)
                {
                    int randomIndex = lockedItems[Random.Range(0, lockedItems.Count)];

                    YandexGame.savesData.unlockKnifes[randomIndex] = true;

                    YandexGame.SaveProgress();

                    _slots[randomIndex].Unlock();

                    Debug.Log("Unlocked Knife ID: " + randomIndex);

                    _shopManager.SetMoneyCount(_shopManager.GetMoney() - _price);
                }
                else
                {
                    DisableBuyButtons();
                    Debug.Log("All Knifes are already unlocked!");
                }
            }
            else if (_category == Category.Sliceable)
            {
                lockedItems = CheckUnlockedItems(lockedItems, _category);

                if (lockedItems.Count > 0)
                {
                    int randomIndex = lockedItems[Random.Range(0, lockedItems.Count)];

                    YandexGame.savesData.unlockSliceable[randomIndex] = true;

                    YandexGame.SaveProgress();

                    _slots[randomIndex].Unlock();

                    Debug.Log("Unlocked Sliceable ID: " + randomIndex);

                    _shopManager.SetMoneyCount(_shopManager.GetMoney() - _price);
                }
                else
                {
                    DisableBuyButtons();
                    Debug.Log("All Sliceable are already unlocked!");
                }
            }
            else if (_category == Category.World)
            {
                lockedItems = CheckUnlockedItems(lockedItems, _category);

                if (lockedItems.Count > 0)
                {
                    int randomIndex = lockedItems[Random.Range(0, lockedItems.Count)];

                    YandexGame.savesData.unlockWorlds[randomIndex] = true;

                    YandexGame.SaveProgress();

                    _slots[randomIndex].Unlock();

                    Debug.Log("Unlocked World ID: " + randomIndex);

                    _shopManager.SetMoneyCount(_shopManager.GetMoney() - _price);
                }
                else
                {
                    DisableBuyButtons();
                    Debug.Log("All World are already unlocked!");
                }
            }
        }
    }

    public void ChooseSlot(int ID)
    {
        switch (_category)
        {
            case Category.Knife:
                YandexGame.savesData.pickedKnife = ID;
                YandexGame.SaveProgress();
                KnifeController.Instance.SetKnife(ObjectDatabase.Instance.knifes[ID]);
                break;
            case Category.Sliceable:
                FindAnyObjectByType<Generator>().SetPrefabsPack(ID);
                break;
            case Category.World:
                YandexGame.savesData.pickedWorld = ID;
                FindAnyObjectByType<WorldManager>().SwitchTheme(ID);
                YandexGame.SaveProgress();
                break;
        }
        Debug.Log("Picked: " + ID);
        // And make active slot
    }

    private List<int> CheckUnlockedItems(List<int> items, Category category)
    {
        switch (category)
        {
            case Category.Knife:
                for (int i = 0; i < YandexGame.savesData.unlockKnifes.Length; i++)
                {
                    if (!YandexGame.savesData.unlockKnifes[i])
                    {
                        items.Add(i);
                    }
                }
                break;
            case Category.Sliceable:
                for (int i = 0; i < YandexGame.savesData.unlockSliceable.Length; i++)
                {
                    if (!YandexGame.savesData.unlockSliceable[i])
                    {
                        items.Add(i);
                    }
                }
                break;
            case Category.World:
                for (int i = 0; i < YandexGame.savesData.unlockWorlds.Length; i++)
                {
                    if (!YandexGame.savesData.unlockWorlds[i])
                    {
                        items.Add(i);
                    }
                }
                break;
            default:
                Debug.Log("Uncorrect type of items");
                break;
        }
        return items;
    }

    private void DisableBuyButtons()
    {
        if ( CheckUnlockedItems(new List<int>(), _category).Count <= 0 )
        {
            _buyButton.SetActive(false);
            _unlockText.SetActive(true);
        }
    }
}
