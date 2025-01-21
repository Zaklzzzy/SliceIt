using DG.Tweening;
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
    [SerializeField] private Sprite _originalSprite;
    [SerializeField] private Sprite _highlightSprite;
    [SerializeField] private SelectCard _selectCard;

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
            List<int> lockedItems = CheckUnlockedItems();

            if (lockedItems.Count > 0)
            {
                AnimatePurchase(lockedItems);
            }
            else
            {
                DisableBuyButtons();
            }
        }
    }

    private void AnimatePurchase(List<int> lockedItems)
    {
        if (lockedItems == null || lockedItems.Count == 0)
        {
            return;
        }

        int randomIndex = lockedItems[Random.Range(0, lockedItems.Count)];

        int totalLoops = 3; // Количество полных проходов по всем элементам
        float singleSlotTime = 0.15f; // Время выделения слота
        float outSlotTime = 0.01f; // Время возврата спрайта на слот

        Sequence sequence = DOTween.Sequence();

        // Полные круги по всем элементам
        for (int loop = 0; loop < totalLoops; loop++)
        {
            foreach (int item in lockedItems)
            {
                var slot = _slots[item];
                var slotImage = slot.GetComponent<Image>();
                if (slotImage == null)
                {
                    continue;
                }

                sequence.AppendCallback(() =>
                {
                    slotImage.sprite = _highlightSprite;
                });
                sequence.AppendInterval(singleSlotTime);
                sequence.AppendCallback(() =>
                {
                    slotImage.sprite = _originalSprite;
                });
                sequence.AppendInterval(outSlotTime);
            }
        }

        // Последний цикл до выбранного элемента
        foreach (int item in lockedItems)
        {
            var slot = _slots[item];
            var slotImage = slot.GetComponent<Image>();
            if (slotImage == null)
            {
                continue;
            }

            sequence.AppendCallback(() =>
            {
                slotImage.sprite = _highlightSprite;
            });
            sequence.AppendInterval(singleSlotTime);
            sequence.AppendCallback(() =>
            {
                slotImage.sprite = _originalSprite;
            });
            sequence.AppendInterval(outSlotTime);

            if (item == randomIndex)
            {
                break;
            }
        }

        // Окончательная анимация для выбранного элемента
        var selectedSlot = _slots[randomIndex];
        var selectedSlotImage = selectedSlot.GetComponent<Image>();
        if (selectedSlotImage == null)
        {
            return;
        }

        sequence.AppendCallback(() =>
        {
            selectedSlotImage.sprite = _originalSprite;
        });
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() =>
        {
            selectedSlotImage.sprite = _highlightSprite;

            selectedSlot.Unlock();

            switch (_category)
            {
                case Category.Knife:
                    YandexGame.savesData.unlockKnifes[randomIndex] = true;
                    Debug.Log("Unlocked Knife ID: " + randomIndex);
                    break;
                case Category.Sliceable:
                    YandexGame.savesData.unlockSliceable[randomIndex] = true;
                    Debug.Log("Unlocked Sliceable ID: " + randomIndex);
                    break;
                case Category.World:
                    YandexGame.savesData.unlockWorlds[randomIndex] = true;
                    Debug.Log("Unlocked World ID: " + randomIndex);
                    break;
            }

            YandexGame.SaveProgress();
            _shopManager.SetMoneyCount(_shopManager.GetMoney() - _price);
        });

        sequence.Play();
    }

    public void ChooseSlot(int ID)
    {
        switch (_category)
        {
            case Category.Knife:
                _selectCard.SetSelectedCard(ID);
                YandexGame.savesData.pickedKnife = ID;
                YandexGame.SaveProgress();
                KnifeController.Instance.SetKnife(ObjectDatabase.Instance.knifes[ID]);
                break;
            case Category.Sliceable:
                _selectCard.SetSelectedCard(ID);
                FindAnyObjectByType<Generator>().SetPrefabsPack(ID);
                break;
            case Category.World:
                _selectCard.SetSelectedCard(ID);
                YandexGame.savesData.pickedWorld = ID;
                FindAnyObjectByType<WorldManager>().SwitchTheme(ID);
                YandexGame.SaveProgress();
                break;
        }
    }

    private List<int> CheckUnlockedItems()
    {
        List<int> items = new();

        switch (_category)
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
        if ( CheckUnlockedItems().Count <= 0 )
        {
            Debug.Log("All " + _category + " are already unlocked!");

            _buyButton.SetActive(false);
            _unlockText.SetActive(true);
        }
    }
}
