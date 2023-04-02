using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemy_speed =2;
    private Animator anim;
    private Player _player;
    bool enemyShooting = true;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;
    float _enemyCanFire = -1;
    [SerializeField]
    float _enemyFireRate = 0.35f;
    Vector2 _laserPosEnemy;
    [SerializeField]
    GameObject _enemyLaserPrefab;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.Log("Player not found");
        }
        
        if (_audioSource == null)
        {
            Debug.Log("Audio source not found");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (enemyShooting == true)
        {
            enemyLaserShoot();
        }   
    }
    void enemyLaserShoot()
    {
        if (Time.time > _enemyCanFire)
        {
            _enemyCanFire = Time.time + _enemyFireRate;
            GameObject _enemyLaserGameObject = Instantiate(_enemyLaserPrefab, this.transform);
            _enemyLaserGameObject.SetActive(true);
            Laser[] _enemyLaser = _enemyLaserGameObject.GetComponentsInChildren<Laser>();
            _enemyLaserPrefab.transform.position = new Vector2(this.transform.position.x, this.transform.position.y-1f);
            for (int i = 0; i < _enemyLaser.Length; i++)
            {
                _enemyLaser[i].enemyLaserActive();
            }           
        }      
     }
    void CalculateMovement()
    {
        transform.Translate(Vector2.down * _enemy_speed * Time.deltaTime);
        if (transform.position.y < -4)
        {
            Destroy(this.gameObject);
        }
    }

    public void EnemyDeath(GameObject laserCall)
    {
        BoxCollider2D enemy = GetComponent<BoxCollider2D>();
        _enemy_speed = 0;
        anim.SetTrigger("Enemy_hit");
        _player.Score_add(10);
        _audioSource.Play();
        enemyShooting = false;
        Destroy(this.gameObject,1.5f);
        Destroy(laserCall);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        BoxCollider2D enemy = GetComponent<BoxCollider2D>();
        if(coll.gameObject.tag== "Player")
        {
            enemy.enabled = false;
            anim.SetTrigger("Enemy_hit");
            _player.Damage();
            _audioSource.Play();
            Destroy(this.gameObject, 1.5f);
        }   
    } 
}
