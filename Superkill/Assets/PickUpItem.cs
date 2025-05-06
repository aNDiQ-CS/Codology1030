using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [Header("��������� �������")]
    public float pickupDistance = 2f; // ��������� �������
    public KeyCode pickupKey = KeyCode.E; // ������� �������
    public Transform holdPosition; // ������� ��������� ��������

    private GameObject heldObject; // ������� ������������ ������
    private Rigidbody heldObjectRb; // Rigidbody ������������� �������
    private bool isHolding = false; // ���� ��������� ��������

    void Update()
    {
        // �������� ������� ������� �������
        if (Input.GetKeyDown(pickupKey))
        {
            if (!isHolding)
            {
                // ������� ��������� �������
                TryPickup();
            }
            else
            {
                // ���� ��� ������ ������� - ��������� ���
                DropItem();
            }
        }

        // ���� ������� � �����, ����� ��������� �������������� �������
        if (isHolding)
        {
            // ��������, ����� ������������ �������
            if (Input.GetMouseButton(1)) // ������ ������ ����
            {
                float rotateSpeed = 100f;
                float xRotation = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
                float yRotation = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

                heldObject.transform.Rotate(Vector3.up, -xRotation, Space.World);
                heldObject.transform.Rotate(Vector3.right, yRotation, Space.World);
            }
        }
    }

    void TryPickup()
    {
        // ������� ��� �� ������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            // ���������, ���� �� � ������� ��� "Pickable"
            if (hit.collider.CompareTag("Pickable"))
            {
                // ��������� ������
                Pickup(hit.collider.gameObject);
            }
        }
    }

    void Pickup(GameObject objToPickup)
    {
        heldObject = objToPickup;
        heldObjectRb = heldObject.GetComponent<Rigidbody>();

        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = true; // ��������� ������
            heldObjectRb.velocity = Vector3.zero;
            heldObjectRb.angularVelocity = Vector3.zero;
        }

        // ������������� �������� � �������
        heldObject.transform.SetParent(holdPosition);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        // �������� ���� ���������
        isHolding = true;

        // ����� �������� �������������� ���������:
        heldObject.layer = LayerMask.NameToLayer("HeldItem"); // ������ ����
        foreach (Collider col in heldObject.GetComponents<Collider>())
        {
            col.enabled = false; // ��������� ����������
        }
    }

    void DropItem()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = false; // �������� ������ �������
        }

        // ���������� ������������ ����
        heldObject.layer = LayerMask.NameToLayer("Default");
        foreach (Collider col in heldObject.GetComponents<Collider>())
        {
            col.enabled = true; // �������� ����������
        }

        // ���������� ��������
        heldObject.transform.SetParent(null);

        // ���������� ���� � ������
        isHolding = false;
        heldObject = null;
        heldObjectRb = null;
    }
}
