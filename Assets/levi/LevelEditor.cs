using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    public Vector2 size;
    public Transform ground;
    public Transform builderCamera;

    public Transform pointer;
    public float pointerMoveSpeed;

    public Vector3 cameraOffset;
    public float cameraSpeed;

    public int selectedprefab;
    public Image[] prefabImages;
    public GameObject[] prefabs;
    public GameObject previewRoot;

    public TextMeshProUGUI waitTimeTextmesh;
    public float waitTime;
    private float timeLeftCounter;

    public int objectPlaceCount;
    private int objectmissingCOunt;

    public int playerIndex;

    private Vector2 moveInput = Vector2.zero;

    public float safeEndSize;

    public PlayerCheckCollider checkCollider;

    public TextMeshProUGUI statusTetxtMehs;

    private void Awake()
    {
        SelectPrefab(0);
    }

    public void Enter()
    {
        moveInput = Vector2.zero;
        timeLeftCounter = waitTime;
        objectmissingCOunt = objectPlaceCount;
        pointer.position = Vector3.zero;
        Vector3 targetPosition = pointer.position + cameraOffset;
        builderCamera.position = targetPosition;
    }

    private void Update()
    {
        string statusText = "";
        statusText += "Deliveries Made: " + (playerIndex == 0 ? GameManager.Instance.player1Score : GameManager.Instance.player2Score);
        statusText += "\nTime Left: " + (int)GameManager.Instance.gameTimeCountdown + "s";
        statusTetxtMehs.SetText(statusText);

        timeLeftCounter -= Time.deltaTime;

        string wahtToDoText = "";
        if (objectmissingCOunt > 0 && timeLeftCounter > 0)
        {
            wahtToDoText = "You need to place " + objectmissingCOunt + " more objects and wait for " + (int)timeLeftCounter + "s";
        }
        else if (objectmissingCOunt > 0 && timeLeftCounter <= 0)
        {
            wahtToDoText = "You need to place " + objectmissingCOunt + " more objects";
        }
        else if (objectmissingCOunt <= 0 && timeLeftCounter > 0)
        {
            wahtToDoText = "You need to wait " + (int)timeLeftCounter + "s";
        }
        else
        {
            if (playerIndex == 0)
            {
                GameManager.Instance.SetPlayer1State(0);
            }
            else
            {
                GameManager.Instance.SetPlayer2State(0);
            }
        }
        waitTimeTextmesh.SetText(wahtToDoText);

        var inputX = moveInput.x;
        var inputY = moveInput.y;
        if (playerIndex == 1)
        {
            inputX = -inputX;
            inputY = -inputY;
        }

        pointer.position += (new Vector3(inputX, 0, inputY).normalized) * pointerMoveSpeed * Time.deltaTime;
        pointer.position = new Vector3(
            Mathf.Clamp(pointer.position.x, -size.x / 2, size.x / 2),
            pointer.position.y,
            Mathf.Clamp(pointer.position.z, -(size.y - safeEndSize) / 2, (size.y - safeEndSize) / 2)
        );

        Vector3 targetPosition = pointer.position + cameraOffset;
        builderCamera.position = Vector3.Lerp(builderCamera.position, targetPosition, Time.deltaTime * cameraSpeed);
    }

    private void SelectPrefab(int index)
    {
        if (index < 0)
        {
            index = prefabs.Length - 1;
        }
        if (index >= prefabs.Length)
        {
            index = 0;
        }

        selectedprefab = index;
        int a = previewRoot.transform.childCount;
        for (int i = 0; i < a; i++)
        {
            Destroy(previewRoot.transform.GetChild(0).gameObject);
        }
        GameObject go = Instantiate(prefabs[index]);
        go.layer = gameObject.layer;
        foreach (Transform item in go.GetComponentsInChildren<Transform>())
        {
            item.gameObject.layer = gameObject.layer;
        }
        go.transform.parent = previewRoot.transform;
        go.transform.localPosition = Vector3.zero;
        foreach (var item in go.GetComponentsInChildren<Collider>())
        {
            item.enabled = false;
        }

        foreach (var item in prefabImages)
        {
            item.color = Color.white;
        }
        if (playerIndex == 0)
        {
            prefabImages[selectedprefab].color = Color.blue;
        }
        if (playerIndex == 1)
        {
            prefabImages[selectedprefab].color = Color.red;
        }
    }

    public void MoveInput(Vector2 dir)
    {
        moveInput = dir.normalized;
    }
    public void PlaceInput()
    {
        if (checkCollider.on)
        {
            return;
        }

        if (objectmissingCOunt <= 0)
        {
            return;
        }
        objectmissingCOunt--;

        GameObject go = Instantiate(prefabs[selectedprefab]);
        go.transform.parent = ground;
        go.transform.position = previewRoot.transform.position;
    }
    public void NextObjectInput()
    {
        SelectPrefab(selectedprefab + 1);
    }
    public void PreInput()
    {
        SelectPrefab(selectedprefab - 1);
    }
    public void PlayInput()
    {

    }
}
