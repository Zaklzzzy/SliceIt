using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject _worldObject;
    [SerializeField] private GameObject[] _blockPrefabs;

    [SerializeField] private Material _levelMaterial;
    [SerializeField] private Texture[] _levelMaterialTextures;

    private void Start()
    {
        SwitchTheme(YandexGame.savesData.pickedWorld);
    }
    public void SwitchTheme(int ID)
    {
        FindAnyObjectByType<Generator>().SetBlock(_blockPrefabs[ID]);
        SetLevelTexture(ID);
        //Set Level Background
    }
    private void SetLevelTexture(int ID)
    {
        _levelMaterial.mainTexture = _levelMaterialTextures[ID];
    }
}
