using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
    {
    bool _isHit;
    [SerializeField] private int currentEnemyHealth;
    [SerializeField] private Slider EnemyHealthSlider;

    public void TakeDamage(int damage)
    {
        if (currentEnemyHealth > 0)
        {
            currentEnemyHealth -= damage;
        }
        UpdateEnHealthUI();

    }

    private void Update()
    {
        if (currentEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    

    void Start()
    {
        InitializeEnHealth();
        UpdateEnHealthUI();
    }

    void InitializeEnHealth()
    {
        EnemyHealthSlider.maxValue = currentEnemyHealth;
        EnemyHealthSlider.value = currentEnemyHealth;
    }

    void UpdateEnHealthUI()
    {
        EnemyHealthSlider.value = currentEnemyHealth;
    }
}
