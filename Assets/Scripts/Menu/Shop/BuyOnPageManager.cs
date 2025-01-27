using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public enum Category { Knife, Sliceable, World }

public class BuyOnPageManager : MonoBehaviour
{
    [SerializeField] private Category _category;
    [SerializeField] private ShopSlot[] _slots;
    [SerializeField] private ParticleSystem _highlightParticles;
    [SerializeField] private GameObject _buttonsContainer;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;
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
                AudioManager.Instance.PlayButtonSound();
                AnimatePurchase(lockedItems);
                _buyButton.enabled = false;
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

        int totalLoops = 2; // Количество полных проходов по всем элементам
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
                    AudioManager.Instance.PlayRouletteSound();
                });
                sequence.AppendInterval(singleSlotTime);
                sequence.AppendCallback(() =>
                {
                    slotImage.sprite = _originalSprite;
                    AudioManager.Instance.PlayRouletteSound();
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
                AudioManager.Instance.PlayRouletteSound();
            });
            sequence.AppendInterval(singleSlotTime);
            sequence.AppendCallback(() =>
            {
                slotImage.sprite = _originalSprite;
                AudioManager.Instance.PlayRouletteSound();
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
            AudioManager.Instance.PlayRouletteSound();
        });
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() =>
        {
            selectedSlotImage.sprite = _highlightSprite;

            _highlightParticles.gameObject.transform.position = selectedSlot.gameObject.transform.position;
            _highlightParticles.Play();

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

            UpdatePrice();
            DisableBuyButtons();

            YandexGame.SaveProgress();
            _shopManager.SetMoneyCount(_shopManager.GetMoney() - _price);
            
            _buyButton.enabled = true;
        });

        sequence.Play();
    }

    public void ChooseSlot(int ID)
    {
        
        switch (_category)
        {
            case Category.Knife:
                AudioManager.Instance.PlayKnifeButtonSound();
                _selectCard.SetSelectedCard(ID);
                YandexGame.savesData.pickedKnife = ID;
                YandexGame.SaveProgress();
                KnifeController.Instance.SetKnife(ObjectDatabase.Instance.knifes[ID]);
                break;
            case Category.Sliceable:
                AudioManager.Instance.PlayObjectButtonSound();
                _selectCard.SetSelectedCard(ID);
                FindAnyObjectByType<Generator>().SetPrefabsPack(ID, false);
                break;
            case Category.World:
                AudioManager.Instance.PlayWorldButtonSound();
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

            _buttonsContainer.SetActive(false);
            _unlockText.SetActive(true);
        }
    }

    private void UpdatePrice()
    {
        var count = 0;

        switch (_category)
        {
            case Category.Knife:
                count = GetUnlockedItemByType(YandexGame.savesData.unlockKnifes);
                break;
            case Category.Sliceable:
                count = GetUnlockedItemByType(YandexGame.savesData.unlockSliceable);
                break;
            case Category.World:
                count = GetUnlockedItemByType(YandexGame.savesData.unlockWorlds);
                break;
            default:
                Debug.Log("Uncorrect type of items");
                break;
        }

        _price = 500 + (count * 150);
        UpdatePriceText();
    }
    private int GetUnlockedItemByType(bool[] arr)
    {
        var count = 0;
        foreach (var item in arr)
        {
            if (item) count++;
        }
        return count;
    }
    private void UpdatePriceText()
    {
        _buyButtonText.text = _price.ToString();
    }
}
