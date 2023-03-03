using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //values
    public float Turn { get; private set; }
    public float TurnRaw { get; private set; }
    public float Forward { get; private set; }
    public float ForwardRaw { get; private set; }
    public float Reverse { get; private set; }
    public float ReverseRaw { get; private set; }
    public float BreakRaw { get; private set; }
    public float Break { get; private set; }
    
    private void Update()
    {
        var targetForward = 0;
        ForwardRaw = Keyboard.current.wKey.ReadValue();
        if (ForwardRaw > 0) targetForward = 1;
        Forward = Mathf.MoveTowards(Forward, targetForward, Time.deltaTime / .1f);
        Forward = Mathf.Clamp01(Forward);

        var targetReverse = 0;
        ReverseRaw = Keyboard.current.sKey.ReadValue();
        if (ReverseRaw > 0) targetReverse = 1;
        Reverse = Mathf.MoveTowards(Reverse, targetReverse, Time.deltaTime / .1f);
        Reverse = Mathf.Clamp01(Reverse);

        var targetTurn = 0;
        TurnRaw = Keyboard.current.aKey.ReadValue() * -1 + Keyboard.current.dKey.ReadValue();
        if (TurnRaw < 0) targetTurn = -1;
        else if (TurnRaw > 0) targetTurn = 1;
        Turn = Mathf.MoveTowards(Turn, targetTurn, Time.deltaTime / .1f);
        Turn = Mathf.Clamp(Turn, -1, 1);

        BreakRaw = Keyboard.current.spaceKey.ReadValue();
        Break = BreakRaw > 0 ? 1 : 0;
    }
}
