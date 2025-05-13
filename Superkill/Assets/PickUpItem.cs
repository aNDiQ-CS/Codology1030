using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [Header("Настройки подбора")]
    public float pickupDistance = 2f; // Дистанция подбора
    public KeyCode pickupKey = KeyCode.E; // Клавиша подбора
    public Transform holdPosition; // Позиция удержания предмета

    private GameObject heldObject; // Текущий удерживаемый объект
    private Rigidbody heldObjectRb; // Rigidbody удерживаемого объекта
    private bool isHolding = false; // Флаг удержания предмета

    void Update()
    {
        // Проверка нажатия клавиши подбора
        if (Input.GetKeyDown(pickupKey))
        {
            if (!isHolding)
            {
                // Попытка подобрать предмет
                TryPickup();
            }
            else
            {
                // Если уже держим предмет - отпускаем его
                DropItem();
            }
        }

        // Если предмет в руках, можно применить дополнительные эффекты
        if (isHolding)
        {
            // Например, можно поворачивать предмет
            if (Input.GetMouseButton(1)) // Правая кнопка мыши
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
        // Пускаем луч от камеры
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            // Проверяем, есть ли у объекта тег "Pickable"
            if (hit.collider.CompareTag("Pickable"))
            {
                // Подбираем объект
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
            heldObjectRb.isKinematic = true; // Отключаем физику
            heldObjectRb.velocity = Vector3.zero;
            heldObjectRb.angularVelocity = Vector3.zero;
        }

        // Устанавливаем родителя и позицию
        heldObject.transform.SetParent(holdPosition);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        // Включаем флаг удержания
        isHolding = true;

        // Можно добавить дополнительные настройки:
        heldObject.layer = LayerMask.NameToLayer("HeldItem"); // Меняем слой
        foreach (Collider col in heldObject.GetComponents<Collider>())
        {
            col.enabled = false; // Отключаем коллайдеры
        }
    }

    void DropItem()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = false; // Включаем физику обратно
        }

        // Возвращаем оригинальный слой
        heldObject.layer = LayerMask.NameToLayer("Default");
        foreach (Collider col in heldObject.GetComponents<Collider>())
        {
            col.enabled = true; // Включаем коллайдеры
        }

        // Сбрасываем родителя
        heldObject.transform.SetParent(null);

        // Сбрасываем флаг и ссылки
        isHolding = false;
        heldObject = null;
        heldObjectRb = null;
    }
}
