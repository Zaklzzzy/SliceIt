using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class KnifeController : MonoBehaviour
{
    public static KnifeController Instance { get; private set; }

    [Header("Knife")]
    [SerializeField] private GameObject _knifePoint;
    [SerializeField] private GameObject _defaultKnife;
    [SerializeField] private float _speed = 0.1f;
    private GameObject _knife;

    public bool IsKnifeEnabled { get; private set; }

    public bool isLevelStarted;

    private Transform _knifeTransform;
    private Quaternion _defaultRotation;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _knife = _knifePoint.GetComponentInChildren<MeshRenderer>().gameObject;
        //if (_knifePoint.transform.childCount == 0) SetKnife(_defaultKnife); else 
        IsKnifeEnabled = false;

        _knifeTransform = _knife.transform;
        _defaultRotation = _knifeTransform.localRotation;
    }

    private void Start()
    {
        SetKnife(ObjectDatabase.Instance.knifes[YandexGame.savesData.pickedKnife]);
    }

    public void KnifeEnable(InputAction.CallbackContext context)
    {
        if (isLevelStarted)
        {
            if (context.performed)
            {
                IsKnifeEnabled = true;
                StartCoroutine(RepeatAnimation());
            }
            else if (context.canceled)
            {
                IsKnifeEnabled = false;
                StopAnimation();
            }
        }
    }

    private IEnumerator RepeatAnimation()
    {
        Vector3 enableAngles = new Vector3(_defaultRotation.eulerAngles.x + 48, _defaultRotation.eulerAngles.y, _defaultRotation.eulerAngles.z);
        Vector3 disableAngles = _defaultRotation.eulerAngles;

        while (IsKnifeEnabled)
        {
            yield return _knifeTransform.DOLocalRotate(enableAngles, _speed).SetEase(Ease.Linear).Play().WaitForCompletion();

            if (!IsKnifeEnabled)
                break;

            yield return _knifeTransform.DOLocalRotate(disableAngles, _speed).SetEase(Ease.Linear).Play().WaitForCompletion();
        }
    }

    private void StopAnimation()
    {
        _knifeTransform.DOLocalRotate(_defaultRotation.eulerAngles, _speed, RotateMode.Fast).Play();
        //_knife.transform.DOKill();
    }

    public void Stop()
    {
        _knife.transform.DOKill();
        IsKnifeEnabled = false;
        isLevelStarted = false;
        LevelController.Instance.FailLevel();
    }
    public void SetKnife(GameObject _knifeObj)
    {
        if (_knife != null) Destroy(_knife);
        _knife = Instantiate(_knifeObj, _knifePoint.transform);
        _knifeTransform = _knife.transform;
        _defaultRotation = _knifeTransform.localRotation;
    }
}
