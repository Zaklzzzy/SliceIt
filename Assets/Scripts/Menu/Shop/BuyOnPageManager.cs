using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public enum Category { Knife, Sliceable, World }

public class BuyOnPageManager : MonoBehaviour
{
    [SerializeField] private Category _category;
    [SerializeField] private ShopSlot[] _slots;
    [SerializeField] private Button _buyButton;
    [SerializeField] private int _price;

    private ShopManager _shopManager;

    private void Awake()
    {
        _shopManager = FindAnyObjectByType<ShopManager>();
    }

    public void BuyRandom()
    {
        if (_shopManager.GetMoney() >= _price)
        {
            List<int> lockedItems = new List<int>();
            for (int i = 0; i < YandexGame.savesData.unlockKnifes.Length; i++)
            {
                if (!YandexGame.savesData.unlockKnifes[i])
                {
                    lockedItems.Add(i);
                }
            }

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
                Debug.Log("All items are already unlocked!");
            }
        }
    }

    public void ChooseSlot(int ID)
    {
        switch (_category)
        {
            case Category.Knife:
                KnifeController.Instance.SetKnife(ObjectDatabase.Instance.knifes[ID]);
                break;
            case Category.Sliceable:
                KnifeController.Instance.SetKnife(ObjectDatabase.Instance.sliceableObjects[ID]);
                break;
            case Category.World:
                KnifeController.Instance.SetKnife(ObjectDatabase.Instance.sliceableObjects[ID]);
                break;
        }
        Debug.Log("Picked: " + ID);
        // And make active slot
    }
}
