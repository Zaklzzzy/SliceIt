using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class SelectCard : MonoBehaviour
{
    [SerializeField] private GameObject[] _cards;
    [SerializeField] private Category _category;

    private Tween _tween;

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
        _tween.Kill();

        gameObject.transform.position = _cards[ID].transform.position;

        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

        _tween = gameObject.transform.DOShakeScale(0.5f, 1f, 10, 45f).Play();
    }
}
