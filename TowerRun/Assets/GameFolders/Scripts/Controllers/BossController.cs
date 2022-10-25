using UnityEngine;

public class BossController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetAnim(bool win)
    {
        _animator.SetTrigger(win ? "isVictory" : "isLose");
    }
}
