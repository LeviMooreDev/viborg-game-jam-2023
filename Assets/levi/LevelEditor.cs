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



    private Vector2 moveInput = Vector2.zero;

    private void Awake()
    {
        ground.localScale = new Vector3(size.x, 1, size.y);

        SelectPrefab(0);

        timeLeftCounter = waitTime;
    }

    private void Update()
    {
        timeLeftCounter -= Time.deltaTime;
        waitTimeTextmesh.SetText("Get ready to roll in: " + (int)timeLeftCounter + "s");

        pointer.position += (new Vector3(-moveInput.y, 0, moveInput.x).normalized) * pointerMoveSpeed * Time.deltaTime;
        pointer.position = new Vector3(
            Mathf.Clamp(pointer.position.x, -size.x / 2, size.x / 2),
            pointer.position.y,
            Mathf.Clamp(pointer.position.z, -size.y / 2, size.y / 2)
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
        prefabImages[selectedprefab].color = new Color(0.31f, 1, 1);
    }

    public void MoveInput(Vector2 dir)
    {
        moveInput = dir.normalized;
    }
    public void PlaceInput()
    {
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
