using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "NewAnimationTemplate", menuName = "Animations/Animation Template")]
public class AnimationTemplate : ScriptableObject
{
    [Header("Animation Settings")]
    public float duration = 1f;
    public Ease ease = Ease.Linear;
    public Vector3 endValue = Vector3.zero;

    public bool loopAnimation = false;
    public int loopCount = -1; // -1 for infinite loops
    public LoopType loopType = LoopType.Yoyo;

    public void Play(Transform target)
    {
        var tween = target.DOMove(endValue, duration)
                          .SetEase(ease);

        if (loopAnimation)
        {
            tween.SetLoops(loopCount, loopType);
        }
    }
}