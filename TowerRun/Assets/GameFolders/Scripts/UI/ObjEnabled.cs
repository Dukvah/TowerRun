using UnityEngine;
using DG.Tweening;

public class ObjEnabled : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOScale(Vector3.one, 1f);
    }
}
