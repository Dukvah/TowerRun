using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Soldier"))
        {
            GameManager.Instance.PlayerMoney++;
            Destroy(gameObject);
        }
    }
}
