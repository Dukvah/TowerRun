using DG.Tweening;
using UnityEngine;

public class ChestHand : MonoBehaviour
{
    private Sequence _mySequence;
    private void OnEnable()
    {
        transform.parent = null;
        
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        
        _mySequence = DOTween.Sequence();
        _mySequence.Append(transform.DOScale(transform.localScale * 2f, 1f));
        _mySequence.SetLoops(-1,LoopType.Yoyo);
    }

    private void OnDisable()
    {
        _mySequence.Kill();
    }
}