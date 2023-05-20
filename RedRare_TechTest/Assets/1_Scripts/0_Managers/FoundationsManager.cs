using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationsManager : EventHandlerMono
{
    [SerializeField] private Foundation[] foundations;

    private void Reset()
    {
        if (foundations == null || foundations.Length == 0)
        {
            foundations = GameObject.FindObjectsOfType<Foundation>();
        }
    }

    protected override void EventRegister()
    {
        foreach (var foundation in foundations) 
        {
            foundation.OnCompletedStateChange += OnFoundationCompletedStateChange;
        }
    }

    protected override void EventUnRegister()
    {
        foreach (var foundation in foundations)
        {
            foundation.OnCompletedStateChange -= OnFoundationCompletedStateChange;
        }
    }

    protected override void Start()
    {
        CheckIfFoundationsArrayAreCorrect();

        for (int i = 0; i < foundations.Length; i++)
        {
            foundations[i].Setup(i);
        }

        base.Start();
    }

    private void CheckIfFoundationsArrayAreCorrect()
    {
        if (foundations == null || foundations.Length == 0)
        {
            SetupFoundationsArray();
            return;
        }

        foreach (var item in foundations)
        {
            if (item == null)
            {
                SetupFoundationsArray();
                return;
            }
        }
    }

    private void SetupFoundationsArray()
    {
        foundations = GameObject.FindObjectsOfType(typeof(Foundation)) as Foundation[];
    }

    private void OnFoundationCompletedStateChange(int id, bool newState)
    {
        this.FoundationCompletedStateChange(id, newState);

        foreach (var item in foundations)
        {
            if (!item.IsCompleted) return;
        }

        this.EveryFoundationCompleted();
    }
}
