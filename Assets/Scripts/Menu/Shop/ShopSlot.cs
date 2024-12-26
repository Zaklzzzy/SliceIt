using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Sprite _unlockedSprite;

    public void Unlock()
    {
        gameObject.GetComponent<Image>().sprite = _unlockedSprite;
        gameObject.GetComponent<Button>().enabled = true;
    }
}
