using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SelectCard : MonoBehaviour
{
    [SerializeField] private GameObject[] _cards;
    [SerializeField] private Category _category;

    private void Start()
    {
        switch (_category)
        {
            case Category.Knife:
                SetSelectedCard(YandexGame.savesData.pickedKnife);
                break;
            case Category.Sliceable:
                SetSelectedCard(YandexGame.savesData.pickedObjects);
                break;
            case Category.World:
                SetSelectedCard(YandexGame.savesData.pickedWorld);
                break;
            default:
                break;
        }
    }
    public void SetSelectedCard(int ID)
    {
        gameObject.transform.position = _cards[ID].transform.position;
    }
}
