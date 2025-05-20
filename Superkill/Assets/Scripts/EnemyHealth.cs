using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    float _health = 100;
    bool _isHit;

    public void TakeDamage(float damage)
    {
        if ((_health > 0) && (_isHit==true))
        {
            _health -= damage;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isHit = true;
    }

    private void Update()
    {
        TakeDamage(1.0f);
        Debug.Log(_health);
    }
}
