using UnityEngine;
using DG.Tweening;

public class SkyDomeRotator : MonoBehaviour
{
    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 360, 0), 60f, RotateMode.FastBeyond360).SetRelative(true)
            .SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);
    }
}
