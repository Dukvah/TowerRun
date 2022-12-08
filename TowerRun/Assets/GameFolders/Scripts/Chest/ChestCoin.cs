using UnityEngine;
using DG.Tweening;

public class ChestCoin : MonoBehaviour
{
    private void OnEnable()
    {
        var temp = transform.position;
        temp.y += 3f;
        temp.x += 1.5f;
        transform.DOMove(temp, 3f);
        transform.DOScale(Vector3.zero, 3f).OnComplete(() =>
        {
            GameManager.Instance.PlayerMoney += 1;
            Destroy(gameObject);
        });
    }
}
