using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Generator : MonoBehaviour
{
    [Header("--TEST ONLY--")]
    [SerializeField] private GameObject _testPrefab;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Transform _levelContainer;
    [SerializeField] private Transform _lastPosition;
    [SerializeField] private GameObject _endObject;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _endPoint;

    private int _allElements;
    private int _blockElements;

    private void Start()
    {
        GenerateWithProperties();
    }

    public void GenerateWithProperties()
    {
        _allElements = Random.Range(40, 52);
        _blockElements = Random.Range((_allElements / 4 - 2), (_allElements / 4));

        GenerateLevel(_allElements, _blockElements);
    }

    private void GenerateLevel(int allElements, int blockElements)
    {
        // Очистка уровня при генерации
        ClearContainer();

        // Объявление и заполнение структуры уровня пустыми ячейками
        var level = new string[allElements];
        for (int i = 0; i < level.Length; i++) { level[i] = ""; }
        
        // Исключение blockElements из начала и конца уровня
        level[0] = "s";
        level[allElements - 1] = "s";

        var bCounter = 0;

        // Генерация blockElements по условиям
        for(int i = 1; bCounter != blockElements; i++)
        {
            if (bCounter != blockElements)
            {
                var index = Random.Range(1, allElements - 2);
                if (level[index] == "")
                {
                    if (index != allElements - 2) level[index + 2] = "s";
                    level[index + 1] = "s";
                    level[index] = "B";
                    level[index - 1] = "s";
                    if (index != 1) level[index - 2] = "s";

                    bCounter++;
                }
            }
        }
        // Заполнение оставшихся пустых элементов
        for(int i = 1; i < level.Length; i++)
        {
            if (level[i] == "") level[i] = "s";
        }

        for (int i = 0; i < level.Length; i++)
        {
            if (level[i] == "s")
            {
                SpawnObject(_testPrefab, _lastPosition.position, _levelContainer);
            }
            else if (level[i] == "B")
            {
                SpawnObject(_blockPrefab, _lastPosition.position, _levelContainer);
            }

            //Debug.Log(i+1 + " " + level[i]);
        }
        // Перемещение EndPoint в конец генерации
        _endPoint.transform.position = new Vector3(_lastPosition.position.x - 4, _lastPosition.position.y, _lastPosition.position.z);
        // Добавление крайнего объекта для ножа
        var endPointPos = _endPoint.transform.position;
        SpawnObject(_endObject, new Vector3(endPointPos.x - 4, endPointPos.y + 2, endPointPos.z), _levelContainer);
    }

    private void SpawnObject(GameObject prefab, Vector3 position, Transform levelContainer)
    {
        var newPosition = new Vector3(position.x - 2, position.y, position.z);
        var obj = Instantiate(prefab, newPosition, prefab.transform.rotation, levelContainer);
        UpdatePosition(obj.GetComponent<ObjectContainer>().EndPoint);
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
}