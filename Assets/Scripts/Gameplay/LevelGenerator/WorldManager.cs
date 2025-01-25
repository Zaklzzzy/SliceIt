using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Material _levelMaterial;
    [SerializeField] private Material _backgroundMaterial;

    public void SwitchTheme(int ID)
    {
        FindAnyObjectByType<Generator>().SetBlock(ObjectDatabase.Instance.worldPresets[ID].GetBlockPrefab());
        SetLevelTexture(ID);
        SetLevelBackground(ID);
    }
    private void SetLevelTexture(int ID)
    {
        _levelMaterial.mainTexture = ObjectDatabase.Instance.worldPresets[ID].GetLevelTexture();
    }
    private void SetLevelBackground(int ID)
    {
        _backgroundMaterial.mainTexture = ObjectDatabase.Instance.worldPresets[ID].GetBackgroundTexture();
    }
}
