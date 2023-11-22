using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    float health;

    public float GetHealth()
    {
        return health;
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
}
