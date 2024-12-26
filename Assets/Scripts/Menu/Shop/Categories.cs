using UnityEngine;

public class Categories : MonoBehaviour
{
    [SerializeField] private GameObject[] _categoryPages;
    [SerializeField] private GameObject[] _categoryNames;
    private int _lastIndex;

    private void Awake()
    {
        _lastIndex = 0;

        _categoryPages[0].gameObject.SetActive(true);
        _categoryNames[0].gameObject.SetActive(true);
    }
    public void SwitchCategory(int category)
    {
        CategoryActive(_lastIndex, category);
        _lastIndex = category;
    }

    private void CategoryActive(int lastIndex, int nowIndex)
    {
        _categoryPages[lastIndex].gameObject.SetActive(false);
        _categoryPages[nowIndex].gameObject.SetActive(true);

        _categoryNames[lastIndex].gameObject.SetActive(false);
        _categoryNames[nowIndex].gameObject.SetActive(true);
    }
}