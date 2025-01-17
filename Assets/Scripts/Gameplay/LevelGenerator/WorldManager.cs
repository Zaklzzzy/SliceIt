using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class WorldManager : MonoBehaviour
{
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
        SetLevelBackground(ID);
    }
    private void SetLevelTexture(int ID)
    {
        _levelMaterial.mainTexture = _levelMaterialTextures[ID];
    }
    private void SetLevelBackground(int ID)
    {

    }
}
