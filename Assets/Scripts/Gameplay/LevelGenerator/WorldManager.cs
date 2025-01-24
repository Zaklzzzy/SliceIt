using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private WorldPreset[] _worldPresets;

    [SerializeField] private Material _levelMaterial;
    [SerializeField] private Material _backgroundMaterial;

    private void Start()
    {
        SwitchTheme(YandexGame.savesData.pickedWorld);
    }
    public void SwitchTheme(int ID)
    {
        FindAnyObjectByType<Generator>().SetBlock(_worldPresets[ID].GetBlockPrefab());
        SetLevelTexture(ID);
        SetLevelBackground(ID);
    }
    private void SetLevelTexture(int ID)
    {
        _levelMaterial.mainTexture = _worldPresets[ID].GetLevelTexture();
    }
    private void SetLevelBackground(int ID)
    {
        _backgroundMaterial.mainTexture = _worldPresets[ID].GetBackgroundTexture();
    }
}
