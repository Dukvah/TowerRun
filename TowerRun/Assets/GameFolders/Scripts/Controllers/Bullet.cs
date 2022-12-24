using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, 2.5f);
    }
}
