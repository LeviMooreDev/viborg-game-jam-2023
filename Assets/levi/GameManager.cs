using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

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
    public GameObject backwallPlayer1;
    public GameObject backwallPlayer2;

    public int player1Score;
    public int player2Score;

    public float totalGameTime;
    public float gameTimeCountdown;

    public TextMeshProUGUI readytextMehsmPlayer1;
    public TextMeshProUGUI readytextMehsmPlayer2;

    public GameObject winRootPlayer1;
    public TextMeshProUGUI winTextMEshPlayer1;
    public GameObject winRootPlayer2;
    public TextMeshProUGUI winTextMEshPlayer2;

    private void Awake()
    {
        Instance = this;
        ground.localScale = new Vector3(levelEditor.size.x, 1, levelEditor.size.y);

        wallCol1.transform.position = new Vector3(-levelEditor.size.x / 2 - .5f, 0, 0);
        wallCol2.transform.position = new Vector3(levelEditor.size.x / 2 + .5f, 0, 0);

        backwallPlayer1.transform.position = new Vector3(0, 0, -levelEditor.size.y / 2 - .5f);
        backwallPlayer2.transform.position = new Vector3(0, 0, levelEditor.size.y / 2 + .5f);

        gameTimeCountdown = totalGameTime;
    }

    public bool running;

    public static bool resetKeyDown;
    private void Update()
    {
        if (Keyboard.current.rKey.isPressed)
        {
            if (!resetKeyDown)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            resetKeyDown = true;
        }
        else
        {
            resetKeyDown = false;
        }

        if (running)
        {
            gameTimeCountdown -= Time.deltaTime;
            if (gameTimeCountdown <= 0)
            {
                gameTimeCountdown = 0;
                SetPlayer1State(-1);
                SetPlayer2State(-1);

                winRootPlayer1.SetActive(true);
                winRootPlayer2.SetActive(true);

                if (player1Score > player2Score)
                {
                    winTextMEshPlayer1.SetText("You won with " + +player1Score + " deliveries!");
                    winTextMEshPlayer2.SetText("You lost with " + +player2Score + " deliveries :(");
                }
                if (player1Score < player2Score)
                {
                    winTextMEshPlayer2.SetText("You won with " + +player2Score + " deliveries!");
                    winTextMEshPlayer1.SetText("You lost with " + +player1Score + " deliveries :(");
                }
                if (player1Score == player2Score)
                {
                    winTextMEshPlayer2.SetText("It was a draw with " + +player2Score + " deliveries!");
                    winTextMEshPlayer1.SetText("It was a draw with " + +player1Score + " deliveries!");
                }
            }
        }
    }

    public void AddPlayerController(PlayerController asd)
    {
        playerControllers.Add(asd.playerIndex, asd);

        if (asd.playerIndex == 0)
        {
            readytextMehsmPlayer1.SetText("Player 1 Ready!");
        }
        if (asd.playerIndex == 1)
        {
            readytextMehsmPlayer1.SetText("Player 2 Ready!");
        }

        if (playerControllers.Keys.Count >= 2)
        {
            SetPlayer1State(player1State);
            SetPlayer2State(player2State);
            running = true;
        }
    }

    public void SetPlayer1State(int state)
    {
        playerControllers[0].SetState(state);

        if (state == 0)
        {
            player1Build.SetActive(false);

            player1Roll = Instantiate(player1RollPrefab);
            player1Roll.transform.parent = player1Root.transform;
            player1Roll.transform.position = new Vector3(0, 1, -levelEditor.size.y / 2 + 2);
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
            if (player1Roll != null)
            {
                player1Roll.GetComponentInChildren<CarController>().DestroyMotor();
            }
            Destroy(player1Roll);
        }
    }
    public void SetPlayer2State(int state)
    {
        playerControllers[1].SetState(state);

        if (state == 0)
        {
            player2Build.SetActive(false);

            player2Roll = Instantiate(player2RollPrefab);
            player2Roll.transform.parent = player2Root.transform;
            player2Roll.transform.position = new Vector3(0, 1, levelEditor.size.y / 2 - 2);
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
            if (player2Roll != null)
            {
                player2Roll.GetComponentInChildren<CarController>().DestroyMotor();
            }
            Destroy(player2Roll);
        }
        if (state == -1)
        {
            player2Build.SetActive(false);
            Destroy(player2Roll);
            player1Build.SetActive(false);
            Destroy(player1Roll);
        }
    }

    public void GivePointPlayer1()
    {
        player1Score++;
    }
    public void GivePointPlayer2()
    {
        player2Score++;
    }

    IEnumerator Test(System.Action action)
    {
        yield return new WaitForSeconds(3);
        //action();
    }
}
