using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    float _rotation_speed=1f;
    Vector3 rotation;
    private Animator _explosion;
    private Spawner spawner;
    
    // Start is called before the first frame update
    private void Start()
    {
        spawner = GameObject.FindObjectOfType<Spawner>().GetComponent<Spawner>();
        _explosion = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(0, 0, _rotation_speed);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            _explosion.SetTrigger("Explosion");
            spawner.StartSpawn();
            Destroy(this.gameObject, 1.5f);
        }
    }

}
