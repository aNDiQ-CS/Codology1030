using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    [Header("Settings")]
    public float openAngle = 90f;          // ���� ��������
    public float openSpeed = 2f;           // �������� ��������
    public KeyCode interactKey = KeyCode.E; // ������� ��������������
    public float interactDistance = 2f;     // ��������� ��������������

    [Header("Pivot")]
    public Transform pivotPoint;           // ����� �������� (���� �����)

    private bool isOpen = false;
    private float currentAngle = 0f;

    void Update()
    {
        // �������� ������� �������
        if (Input.GetKeyDown(interactKey))
        {
            // �������� ���������� �� ������
            /*if (IsPlayerCloseEnough())*/
            //{
                ToggleDoor();
            //}
        }

        // ������� ��������
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

    bool IsPlayerCloseEnough()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        return Vector3.Distance(player.transform.position, transform.position) <= interactDistance;
    }
}