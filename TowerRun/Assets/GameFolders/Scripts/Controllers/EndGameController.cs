using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    private bool _isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Soldier"))
        {
            if (!_isTriggered)
            {
                _isTriggered = true;
                GameManager.Instance.goBattle.Invoke();
            }
        }
    }
}
