using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3.5f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f);

        transform.Translate(direction * speed * Time.deltaTime);
    }
}
