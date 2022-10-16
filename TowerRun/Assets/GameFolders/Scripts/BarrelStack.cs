using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelStack : MonoBehaviour
{
    [SerializeField] private List<Barrel> barrels = new List<Barrel>();

    [SerializeField] private Transform barrelTarget;

    private void Start()
    {
        StartCoroutine(RollBarrel());
    }

    private IEnumerator RollBarrel()
    {
        foreach (var barrel in barrels)
        {
            barrel.RollToTarget(barrelTarget.position);
            yield return new WaitForSeconds(3f);
        }
    }
}
