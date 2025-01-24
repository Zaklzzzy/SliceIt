using UnityEngine;

[CreateAssetMenu(menuName = "World Preset")]

public class WorldPreset : ScriptableObject
{
    [SerializeField] private Texture2D _levelTexture;
    [SerializeField] private Texture2D _backgroundTexture;
    [SerializeField] private GameObject _blockPrefab;

    public Texture GetLevelTexture() => _levelTexture;
    public Texture GetBackgroundTexture() => _backgroundTexture;
    public GameObject GetBlockPrefab() => _blockPrefab;

}
