using UnityEngine;

public class BarrelStack : MonoBehaviour
{
    [SerializeField] private Barrel barrelPrefab;
    [SerializeField] private Transform barrelSpawnPos,barrelTarget;

    [SerializeField] private float barrelSpawnFrequency;

    public void SendBarrels()
    {
        InvokeRepeating(nameof(RollBarrel), 1f,barrelSpawnFrequency );
    }
    public void StopBarrels()
    {
        CancelInvoke(nameof(RollBarrel));
    }

    private void RollBarrel()
    {
        var barrel = Instantiate(barrelPrefab, barrelSpawnPos.position, Quaternion.identity, transform);
        barrel.RollToTarget(barrelTarget.position);
        
    }
}
