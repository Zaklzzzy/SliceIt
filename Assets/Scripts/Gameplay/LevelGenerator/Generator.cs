using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;


public class Generator : MonoBehaviour
{
    [Header("Generator Fields")]
    [SerializeField] private GameObject[] _objectsPrefabs;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Transform _levelContainer;
    [SerializeField] private Transform _lastPosition;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _endPoint;

    private int _allElements;
    private int _blockElements;
    private const int gapBetweenBlocks = 2;

    private void Start()
    {
        SetPrefabsPack(YandexGame.savesData.pickedObjects, true);
        FindAnyObjectByType<WorldManager>().SwitchTheme(YandexGame.savesData.pickedWorld);
        //GenerateWithProperties();
    }

    public void GenerateWithProperties()
    {
        _allElements = Random.Range(40, 52);
        //_blockElements = Random.Range((_allElements / 4 - 2), (_allElements / 4));
        _blockElements = 11;

        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // Очистка уровня при генерации
        ClearContainer();

        var level = new string[_allElements];

        level = GenerateLevelPreset(level);

        /*        int avaliableBlocks = _allElements / 3 - 1;
                //Debug.Log("avaliableBlocks - " + avaliableBlocks);
                if (_blockElements > avaliableBlocks) _blockElements = avaliableBlocks;

                var blockIndexesList = new List<int>(_blockElements) { 0 };

                for(int i = 1; blockIndexesList.Count < blockIndexesList.Capacity; i++)
                {
                    var index = Random.Range(1, _allElements - 2);

                    if (Enumerable.Range(index - gapBetweenBlocks, gapBetweenBlocks * 2).Any(blockIndexesList.Contains)) continue;

                    blockIndexesList.Add(index);
                    level[index] = "B";
                }*/

        ShowLevelPresetToConsole(level);

        // Спавн объектов
        SpawnAllLevelObjects(level);
    }
    private void ShowLevelPresetToConsole(string[] level)
    {
        for (int i = 0; i < level.Length; i++)
        {
            Debug.Log(i + 1 + " " + level[i]);
        }
    }
    private string[] GenerateLevelPreset(string[] level)
    {
        for (int i = 0; i < level.Length; i++) { level[i] = "s"; }

        int avaliableBlocks = _allElements / 3 - 1;
        //Debug.Log("avaliableBlocks - " + avaliableBlocks);
        if (_blockElements > avaliableBlocks) _blockElements = avaliableBlocks;

        var blockIndexesList = new List<int>(_blockElements) { 0 };

        for (int i = 1; blockIndexesList.Count < blockIndexesList.Capacity; i++)
        {
            var index = Random.Range(1, _allElements - 2);

            if (Enumerable.Range(index - gapBetweenBlocks, gapBetweenBlocks * 2).Any(blockIndexesList.Contains)) continue;

            blockIndexesList.Add(index);
            level[index] = "B";
        }

        return level;
    }
    private void SpawnAllLevelObjects(string[] level)
    {
        for (int i = 0; i < level.Length; i++)
        {
            if (level[i] == "s")
            {
                SpawnObject(_objectsPrefabs[Random.Range(0, _objectsPrefabs.Length)], _lastPosition.position, _levelContainer);
            }
            else if (level[i] == "B")
            {
                SpawnObject(_blockPrefab, _lastPosition.position, _levelContainer);
            }

            //Debug.Log(i+1 + " " + level[i]);
        }
        // Перемещение EndPoint в конец генерации
        _endPoint.transform.position = new Vector3(_lastPosition.position.x - 10, 0.43f, 0);
    }

    private void SpawnObject(GameObject prefab, Vector3 position, Transform levelContainer)
    {
        var newPosition = new Vector3(position.x - 2, 0.5f, 0);
        var obj = Instantiate(prefab, newPosition, prefab.transform.rotation, levelContainer);
        try
        {
            UpdatePosition(obj.GetComponent<ObjectContainer>().EndPoint);
        }
        catch
        {
            UpdatePosition(obj.GetComponentInChildren<ObjectContainer>().EndPoint);
        }
    }
    private void UpdatePosition(Transform newPosition)
    {
        _lastPosition = newPosition;
    }
    private void ClearContainer()
    {
        _lastPosition = _startPoint;

        var container = _levelContainer.GetComponentsInChildren<ObjectContainer>();
        if (container.Length > 0)
        {
            foreach (var item in container)
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void SetPrefabsPack(int ID, bool isStartInvoke)
    {
        YandexGame.savesData.pickedObjects = ID;
        YandexGame.SaveProgress();

        var startIndex = 0;
        var endIndex = 0;

        switch (ID)
        {
            case 0:
                startIndex = 0;
                endIndex = 4;
                break;
            case 1:
                startIndex = 5;
                endIndex = 10;
                break;
            case 2:
                startIndex = 11;
                endIndex = 18;
                break;
            case 3:
                startIndex = 19;
                endIndex = 23;
                break;
            case 4:
                startIndex = 24;
                endIndex = 28;
                break;
            case 5:
                startIndex = 29;
                endIndex = 32;
                break;
            case 6:
                startIndex = 33;
                endIndex = 37;
                break;
            case 7:
                startIndex = 38;
                endIndex = 42;
                break;
            case 8:
                startIndex = 43;
                endIndex = 46;
                break;
            default:
                Debug.Log("Error Sliceable Pick");
                break;
        }

        GameObject[] newPack = new GameObject[endIndex - startIndex + 1];
        var counter = 0;

        for (int i = startIndex; i <= endIndex; i++)
        {
            newPack[counter] = ObjectDatabase.Instance.sliceableObjects[i];
            counter++;
        }
        _objectsPrefabs = newPack;

        if (!isStartInvoke) GenerateWithProperties();
    }

    public void SetBlock(GameObject _newBlock)
    {
        _blockPrefab = _newBlock;

        GenerateWithProperties();
    }
}