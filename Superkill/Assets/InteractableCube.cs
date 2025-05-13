using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCube : MonoBehaviour, IInteractable
{
    public void OnCursorOut()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public void Interacte()
    {
        Debug.Log("�� ���� ���������������");
    }

    public void OnCursorIn()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    public string ShowHint()
    {
        return "������� � ��� �������������� ";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
