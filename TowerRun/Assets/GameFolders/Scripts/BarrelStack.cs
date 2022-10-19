using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelStack : MonoBehaviour
{
    [SerializeField] private List<Barrel> barrels = new List<Barrel>();
    [SerializeField] private Transform barrelTarget;

    [SerializeField] private float barrelTime;

    public void SendBarrels()
    {
        StartCoroutine(RollBarrel());
    }

    private IEnumerator RollBarrel()
    {
        foreach (var barrel in barrels)
        {
            barrel.RollToTarget(barrelTarget.position);
            yield return new WaitForSeconds(barrelTime);
        }
    }
}
