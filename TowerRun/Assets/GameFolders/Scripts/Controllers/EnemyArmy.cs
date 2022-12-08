using System.Collections.Generic;
using UnityEngine;

public class EnemyArmy : MonoBehaviour
{
    [SerializeField] private EndGameController endGameController;
    [SerializeField] private List<EnemySoldier> soldiers = new();

    private int _soldierCount;
    private void Awake()
    {
        _soldierCount = soldiers.Count;
    }

    public void SoldierDied()
    {
        _soldierCount--;
        if (_soldierCount <= 0)
        {
            //GameManager.Instance.levelSuccess.Invoke();
            endGameController.StopGameOver();
            GameManager.Instance.chestOpen.Invoke();
        }
    }
}
