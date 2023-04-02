using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_up : MonoBehaviour
{
    [SerializeField]
    float _speed = 3.0f;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down*Time.deltaTime*_speed);
        if (transform.position.y < -4)
        {
            Destroy(this.gameObject);
        }
    }
}
