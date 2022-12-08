using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject coinPrefab, hand, effect;
    [SerializeField] private int coin;
    
    private Sequence _mySequence;

    public void OpenChest()
    {
        gameObject.transform.DOScale(Vector3.one, 1.5f).OnComplete(() =>
        {
            InvokeRepeating(nameof(CheckChestClick),0f,Time.deltaTime);
            
            hand.SetActive(true);
            effect.SetActive(true);
            ShakeChest();
            
        });
    }

    private void CheckChestClick()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit,layerMask))
            {
                if (hit.collider != null)
                {
                    gameObject.transform.GetChild(0).DOLocalRotate(Vector3.right * -150, 1.5f);
                    CancelInvoke(nameof(CheckChestClick));
                    StartCoroutine(CoinCreatorAsync());
                    hand.SetActive(false);
                    effect.SetActive(true);
                    _mySequence.Kill();
                    
                }
            }
        }
    }

    private IEnumerator CoinCreatorAsync()
    {
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < coin; i++)
        {
            Vector3 temp = transform.position;
            temp.y += 0.3f;
            temp.x = Random.Range(-0.12f, 0.12f);
            Instantiate(coinPrefab,temp,Quaternion.Euler(new Vector3(0,78f,0)));
            
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);
        GameManager.Instance.levelSuccess.Invoke();
    }
    private void ShakeChest()
    {
        _mySequence = DOTween.Sequence();
        _mySequence.Append(transform.DOPunchScale(transform.lossyScale * 0.2f, 0.8f,5,0)).SetEase(Ease.Linear);
        _mySequence.SetLoops(-1);
    }
}
