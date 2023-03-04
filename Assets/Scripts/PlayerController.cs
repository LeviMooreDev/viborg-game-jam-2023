using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int playerIndex;

    private int state = -1; //0 = roll, 1 = build, 2 = waiting to roll

    public PlayerIndexer indexer;
    public CarInput carInput;
    public LevelEditor levelEditor;
    public PlayerInput playerInput;

    void Awake()
    {

        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;

        PlayerIndexer[] indexers = FindObjectsOfType<PlayerIndexer>();


        foreach (PlayerIndexer pi in indexers)
        {
            if (pi.index == playerIndex)
            {
                indexer = pi;
                break;
            }
        }

        carInput = indexer.GetComponentInChildren<CarInput>(true);
        levelEditor = indexer.GetComponentInChildren<LevelEditor>(true);
    }

    private void Start()
    {
        GameManager.Instance.AddPlayerController(this);
    }

    public void OnForwardTrigger(InputValue value)
    {
        float input = (float)(value.Get() ?? 0f);
        if (state == 0)
        {
            if (carInput != null)
            {
                carInput.Forward(input);
            }
        }


    }

    public void OnBackwardsTrigger(InputValue value)
    {
        float input = (float)(value.Get() ?? 0f);
        if (state == 0)
        {
            carInput.Backwards(input);
        }


    }

    public void OnJoystick(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input == null)
        {
            input = Vector2.zero;
        }
        if (state == 0)
        {
            carInput.Turn(input);
        }
        else
        {
            levelEditor.MoveInput(input);
        }

    }

    public void OnBreak(InputValue value)
    {
        float input = (float)(value.Get() ?? 0f);
        if (state == 0)
        {
            carInput.Break(input);
        }
    }


    public void OnButtonA()
    {
        if (state == 1)
        {
            levelEditor.PlaceInput();
        }
    }

    public void OnDpadNext()
    {
        if (state == 1)
        {
            levelEditor.NextObjectInput();
        }
    }

    public void OnDpadPre()
    {
        if (state == 1)
        {
            levelEditor.PreInput();
        }

    }

    internal void SetState(int state)
    {
        this.state = state;
    }
}
