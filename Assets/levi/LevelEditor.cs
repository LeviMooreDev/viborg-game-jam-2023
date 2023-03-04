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


        var forward = Keyboard.current.dKey.ReadValue();
        var backward = -Keyboard.current.aKey.ReadValue();
        var left = -Keyboard.current.wKey.ReadValue();
        var right = Keyboard.current.sKey.ReadValue();
        pointer.position += (new Vector3(left + right, 0, forward + backward).normalized) * pointerMoveSpeed * Time.deltaTime;
        pointer.position = new Vector3(
            Mathf.Clamp(pointer.position.x, -size.x / 2, size.x / 2),
            pointer.position.y,
            Mathf.Clamp(pointer.position.z, -size.y / 2, size.y / 2)
        );
         
        Vector3 targetPosition = pointer.position + cameraOffset;
        builderCamera.position = Vector3.Lerp(builderCamera.position, targetPosition, Time.deltaTime * cameraSpeed);

        var spaceKey = Keyboard.current.spaceKey.isPressed;
        if (spaceKey)
        {
            GameObject go = Instantiate(prefabs[selectedprefab]);
            go.transform.parent = ground;
            go.transform.position = previewRoot.transform.position;
        }

        var leftKey = Keyboard.current.leftArrowKey.isPressed;
        if (leftKey)
        {
            SelectPrefab(selectedprefab - 1);
        }
        var rightKey = Keyboard.current.rightArrowKey.isPressed;
        if (rightKey)
        {
            SelectPrefab(selectedprefab + 1);
        }
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

        foreach (var item in prefabImages)
        {
            item.color = Color.white;
        }
        prefabImages[selectedprefab].color = new Color(0.31f, 1, 1);
    }

    public void MoveInput(Vector2 dir)
    {

    }
    public void PlaceInput()
    {

    }
    public void NextObjectInput()
    {

    }
    public void PreInput()
    {

    }
    public void PlayInput()
    {

    }
}
