using UnityEngine;

public class RifleProjectile : MonoBehaviour
{
    public int damage = 1; // ���� �������
    public GameObject hitEffect; // ������ ���������

    void OnCollisionEnter(Collision collision)
    {
        // ��������� �����
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth _health))
        {
            _health.TakeDamage(damage);
        }

        // ����� �������
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}