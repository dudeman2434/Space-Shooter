using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;
    GameObject _triple_shot_parent;
    private bool _enemyLaser=false;
    private Player[] _player;
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _player = GameObject.FindObjectsOfType<Player>();
        }
        catch
        {
            Debug.Log("_player is null on Laser script");
        }
        if (transform.parent != null)
        {
            _triple_shot_parent = transform.parent.gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (_enemyLaser == false)
        {
            laserUp();
        }
        else if(_enemyLaser == true)
        {
            laserDown();
        }  
    }
    void laserUp()
    {
        transform.Translate(Vector2.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y > 7.5f)
        {
            try
            {
                Destroy(this.transform.parent.gameObject);
            }
            catch
            {
                Destroy(this.transform.gameObject);
            }
        }
    }
    void laserDown()
    {
        transform.Translate(Vector2.down * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -4)
        {
            this.transform.parent.gameObject.SetActive(false);
        }
    }
    public void enemyLaserActive()
    {
        _enemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Enemy"&& _enemyLaser == false)
        {
            collision.GetComponent<Enemy>().EnemyDeath(this.gameObject);
        }
        else if (collision.gameObject.tag=="Player"&& _enemyLaser==true)
        {
            
            for (int i = 0; i < _player.Length; i++)
            {
                _player[i].Damage();
            }
        }
    }
}
