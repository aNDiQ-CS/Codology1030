using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    public float openAngle = 90f;          // Угол открытия
    public float openSpeed = 2f;           // Скорость открытия
    public KeyCode interactKey = KeyCode.E; // Клавиша взаимодействия
    public float interactDistance = 2f;     // Дистанция взаимодействия

    [Header("Pivot")]
    public Transform pivotPoint;           // Точка вращения (край двери)

    private bool isOpen = false;
    private float currentAngle = 0f;

    void Update()
    {
        RotateDoor();
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    void RotateDoor()
    {
        float targetAngle = isOpen ? openAngle : 0f;
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, openSpeed * Time.deltaTime);

        if (pivotPoint != null)
        {
            transform.RotateAround(pivotPoint.position, Vector3.up, currentAngle - transform.eulerAngles.y);
        }
    }

    /*bool IsPlayerCloseEnough()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        *//*GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        return Vector3.Distance(player.transform.position, transform.position) <= interactDistance;*//*
    }*/

    public void Interacte()
    {
        Debug.Log("Мы типа взаимодействуем");
        if (Input.GetKeyDown(interactKey))
        {
            // Проверка расстояния до игрока
            ToggleDoor();

        }
    }
    public string ShowHint()
    {
        return "Нажмите Е для взаимодействия ";
    }
    public void OnCursorIn()
    {
       
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
    public void OnCursorOut()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}