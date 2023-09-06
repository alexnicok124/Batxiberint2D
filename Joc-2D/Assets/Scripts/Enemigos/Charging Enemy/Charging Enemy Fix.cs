using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemyFix : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    HealthEnemy health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<HealthEnemy>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.viu == false)
        {
            rigidbody2d.velocity = Vector3.zero;
        }


    }
}
