using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    int _health = 10;
    bool _isHit;
    private int currentEnemyHealth;
    [SerializeField] private Slider EnemyHealthSlider;

    public void TakeDamage(int damage)
    {
        if (_health > 0)
        {
            _health -= damage;
        }
    }

    private void Update()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log(_health);
        UpdateEnHealthUI();
    }

    

    void Start()
    {
        InitializeEnHealth();
        UpdateEnHealthUI();
    }

    void InitializeEnHealth()
    {
        currentEnemyHealth = _health;
        EnemyHealthSlider.maxValue = _health;
        EnemyHealthSlider.value = _health;
    }

    void UpdateEnHealthUI()
    {
        EnemyHealthSlider.value = currentEnemyHealth;

        float healthPercent = (float)currentEnemyHealth / _health;
        EnemyHealthSlider.fillRect.GetComponent<Image>().color =
            Color.Lerp(Color.green, Color.red, healthPercent);
    }
}
