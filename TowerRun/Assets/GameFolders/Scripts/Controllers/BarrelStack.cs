using UnityEngine;

public class BarrelStack : MonoBehaviour
{
    [SerializeField] private Barrel barrelPrefab;
    [SerializeField] private Transform barrelSpawnPos,barrelTarget;
    [SerializeField] private float barrelSpawnFrequency;
    [SerializeField] private int barrelCount;

    private void OnDisable()
    {
        CancelInvoke(nameof(RollBarrel));
    }

    public void SendBarrels()
    {
        InvokeRepeating(nameof(RollBarrel), 0f,barrelSpawnFrequency);
    }
    public void StopBarrels()
    {
        CancelInvoke(nameof(RollBarrel));
    }

    private void RollBarrel()
    {
        for (int i = 0; i < barrelCount; i++)
        {
            var barrel = Instantiate(barrelPrefab, barrelSpawnPos.position, Quaternion.identity, transform);
            barrel.RollToTarget(barrelTarget.position);
        }
    }
}
