using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action EventOne;
    public event Action EventOneTransition;
    public event Action EventTwoStart;
    public event Action EventTwo;
    public event Action EventThree;
    public event Action EventFour;
    public event Action EventFive;

    public event Action EventEightStart;
    public event Action EventEight;
    public event Action EventFinish;
    public event Action MonsterCaught;

    public void SceneOne()
    {
        EventOne?.Invoke();
    }

    public void SceneOneTransitionToTwo()
    {
        EventOneTransition?.Invoke();
    }

    public void SceneTwoStart()
    {
        EventTwoStart?.Invoke();
    }

    public void SceneTwo()
    {
        EventTwo?.Invoke();
    }

    public void SceneThree()
    {
        EventThree?.Invoke();
    }

    public void SceneFour()
    {
        EventFour?.Invoke();
    }

    public void SceneFive()
    {
        EventFive?.Invoke();
    }
    public void SceneEightBegin()
    {
        EventEightStart?.Invoke();
    }
    public void SceneEight()
    {
        EventEight?.Invoke();
    }
    public void CaughtByMonster()
    {
        MonsterCaught?.Invoke();
    }
    public void FinalScene()
    {
        EventFinish?.Invoke();
    }
}
