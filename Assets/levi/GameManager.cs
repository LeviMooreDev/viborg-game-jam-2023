using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player1Root;
    public GameObject player1RollPrefab;
    public GameObject player1Roll;
    public GameObject player1Build;
    public int player1State;

    public GameObject player2Root;
    public GameObject player2RollPrefab;
    public GameObject player2Roll;
    public GameObject player2Build;
    public int player2State;

    public Transform ground;
    public LevelEditor levelEditor;
    public Dictionary<int, PlayerController> playerControllers = new();

    public GameObject wallCol1;
    public GameObject wallCol2;

    private void Awake()
    {
        Instance = this;
        ground.localScale = new Vector3(levelEditor.size.x, 1, levelEditor.size.y);

        wallCol1.transform.position = new Vector3(-levelEditor.size.x / 2 - .5f, 0, 0);
        wallCol2.transform.position = new Vector3(levelEditor.size.x / 2 + .5f, 0, 0);
    }

    public void AddPlayerController(PlayerController asd)
    {
        playerControllers.Add(asd.playerIndex, asd);
        if (asd.playerIndex == 0)
        {
            SetPlayer1State(0);
            SetPlayer1State(1);
            if (player1State != 0)
            {
                SetPlayer1State(0);
            }

            SetPlayer1State(player1State);
        }
        if (asd.playerIndex == 1)
        {
            SetPlayer2State(0);
            SetPlayer2State(1);
            if (player2State != 0)
            {
                SetPlayer2State(0);
            }

            SetPlayer2State(player2State);
        }
    }

    public void SetPlayer1State(int state)
    {
        playerControllers[0].state = state;

        if (state == 0)
        {
            player1Build.SetActive(false);

            player1Roll = Instantiate(player1RollPrefab);
            player1Roll.transform.parent = player1Root.transform;
            player1Roll.transform.position = new Vector3(0, 1, -levelEditor.size.y / 2 + 5);
            playerControllers[0].carInput = player1Roll.GetComponentInChildren<CarInput>();

            StartCoroutine(Test(() =>
            {
                SetPlayer1State(1);
            }));
        }
        if (state == 1)
        {
            player1Build.GetComponent<LevelEditor>().Enter();
            player1Build.SetActive(true);
            player1Roll.GetComponentInChildren<CarController>().DestroyMotor();
            Destroy(player1Roll);
        }
    }
    public void SetPlayer2State(int state)
    {
        playerControllers[1].state = state;

        if (state == 0)
        {
            player2Build.SetActive(false);

            player2Roll = Instantiate(player2RollPrefab);
            player2Roll.transform.parent = player2Root.transform;
            player2Roll.transform.position = new Vector3(0, 1, levelEditor.size.y / 2 - 5);
            player2Roll.transform.eulerAngles = new Vector3(0, 180, 0);
            playerControllers[1].carInput = player2Roll.GetComponentInChildren<CarInput>();

            StartCoroutine(Test(() =>
            {
                SetPlayer2State(1);
            }));
        }
        if (state == 1)
        {
            player2Build.GetComponent<LevelEditor>().Enter();
            player2Build.SetActive(true);
            player2Roll.GetComponentInChildren<CarController>().DestroyMotor();
            Destroy(player2Roll);
        }
    }

    IEnumerator Test(System.Action action)
    {
        yield return new WaitForSeconds(5);
        action();
    }
}