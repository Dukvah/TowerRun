using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    [SerializeField] private List<Animator> showcases = new();
    [SerializeField] private Scrollbar scrollBar;

    private float _scrollPos = 0;
    private float[] _pos;
    private float _distance;

    public int CurrentCard { get; private set; } = 0;
    private void Start()
    {
        _pos = new float[transform.childCount];
        _distance = 1f / (_pos.Length - 1f);
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Controller),0f,1f * Time.deltaTime);

        showcases[CurrentCard].SetBool("isRun", true);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Controller));
    }

    private void Controller()
    {
        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = _distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            _scrollPos = scrollBar.value;
        }
        else
        {
            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scrollPos < _pos[i] + (_distance / 2) && _scrollPos > _pos[i] - (_distance / 2))
                {
                    scrollBar.value = Mathf.Lerp(scrollBar.value, _pos[i], 0.1f);
                    ChangeShowcase(i);
                }
            }
        }

        for (int i = 0; i < _pos.Length; i++)
        {
            if (_scrollPos < _pos[i] + (_distance / 2) && _scrollPos > _pos[i] - (_distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, Vector2.one, 0.04f);
                for (int j = 0; j < _pos.Length; j++)
                {
                    if (j != i)
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.7f, 0.7f), 0.04f);
                }
            }
        }
    }

    private void ChangeShowcase(int index)
    {
        if (index == CurrentCard) return;

        CurrentCard = index;
        
        foreach (var showcase in showcases)
        {
            showcase.gameObject.SetActive(false);
        }
        
        showcases[CurrentCard].gameObject.SetActive(true);
        showcases[CurrentCard].SetBool("isRun",true);
    }
}
