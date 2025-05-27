using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    float _health = 10;
    bool _isHit;
    Slider enemyHealthSlider;
    private float currentEnemyHealth;
    [SerializeField] private Slider EnemyHealthSlider;

    public void TakeDamage(float damage)
    {
        if (_health > 0)
        {
            _health -= damage;
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        TakeDamage(1.0f);
    }*/
    

    private void Update()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log(_health);        
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
            Color.Lerp(Color.red, Color.green, healthPercent);
    }

    private void Start()
    {
        InitializeEnHealth();
        UpdateEnHealthUI();
    }
}
