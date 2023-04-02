using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speedvar=4;
    public int _playerLife;
    Vector2 player_input;
    public bool isPlayer1, isPlayer2;
    [SerializeField]
    private GameObject _triple_shot_prefab;
    bool _triple_shot_active = false;
    private float _canfire = -1f;
    private float _playerDamageCD = 0.5f, _canDamage = -1f;
    [SerializeField]
    private float _fireRate=0.5f;
    [SerializeField]
    private GameObject _laser_prefab;
    private Laser _laserAccess;
    [SerializeField]
    GameObject _shield_prefab;
    [SerializeField]
    private GameObject[] fireDamage;
    [SerializeField]
    private AudioClip _laserSound_clip;
    private AudioSource _audioSource;
    private Animator _playerAnimation;
    private Player_Animation playerAnim;
    private UI_Manager _ui_manager;
    [SerializeField]
    private int _score_point = 0;
    private GameManager _gameManager;

    

    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log(gameObject.name);
        _audioSource = GetComponent<AudioSource>();
        _playerAnimation = GetComponent<Animator>();
        playerAnim=GetComponent<Player_Animation>();
        _laserAccess = _laser_prefab.GetComponent<Laser>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("Game manager not found");
        }

        _ui_manager = GameObject.Find("UI_Manager").GetComponent<UI_Manager>();
        if (_ui_manager == null)
        {
            Debug.Log("Score manager not found");
        }

        if (_audioSource == null)
        {
            Debug.Log("Audio source is null");
        }
        else
        {
            _audioSource.clip = _laserSound_clip;
        }
        if (_gameManager.isCoop == true)
        {
            Debug.Log(gameObject.name+"true");
        }
        if (_gameManager.isCoop == false)
        {
            Debug.Log(gameObject.name+"coop false");
            transform.position = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlayer1 == true)
        {
            Movement();
            if (Input.GetKey(KeyCode.Space) && Time.time > _canfire)
            {
                _canfire = Time.time + _fireRate;
                Shoot();
            }
            if (_playerLife <= -1)
            {
                StartCoroutine("death");
                _gameManager.setGameOver();

                _ui_manager.UILifeUpdate(-1);
                Destroy(this.gameObject, 1.5f);

            }
        }
        else if (isPlayer2 == true)
        {
            MovementPlayerTwo();
            if (Input.GetKey(KeyCode.RightControl) && Time.time > _canfire)
            {
                _canfire = Time.time + _fireRate;
                Shoot();
            }
            if (_playerLife <= -1)
            {
                StartCoroutine("death");
                _gameManager.setGameOver();
                _ui_manager.UILifeUpdate(-1);
                Destroy(this.gameObject, 1.5f);
            }
            //Shooting cooldown
            if (Input.GetKey(KeyCode.Space) && Time.time > _canfire)
            {
                _canfire = Time.time + _fireRate;
                Shoot();
            }
        }
        //Lock player position in-screen
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.7f, 9.7f), Mathf.Clamp(transform.position.y, -3.2f, 5.1f));
    }
    void Movement()
    {
        if (_playerLife >= 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector2.up * _speedvar * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector2.down * _speedvar * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * _speedvar * Time.deltaTime);
                playerAnim.rightAnim();
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.left * _speedvar * Time.deltaTime);
                playerAnim.leftAnim();
            }
            if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                playerAnim.idleAnim();
            }
        }
    }
    void MovementPlayerTwo()
    {
        if (_playerLife >= 0)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector2.up * _speedvar * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector2.down * _speedvar * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector2.right * _speedvar * Time.deltaTime);
                playerAnim.rightAnim();
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector2.left * _speedvar * Time.deltaTime);
                playerAnim.leftAnim();
            }
            if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                playerAnim.idleAnim();
            }
        }
    }
    void Shoot()
    {
        if (_triple_shot_active == true)
        {
            Instantiate(_triple_shot_prefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laser_prefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }
 
        public void Damage()
        {
            if (_shield_prefab.gameObject.activeSelf == false && Time.time > _canDamage)
            {
                _canDamage = Time.time + _playerDamageCD;
                _playerLife--;
                if (_playerLife >= 0)
                {
                    fireDamage[_playerLife].SetActive(true);
                }
                _ui_manager.UILifeUpdate(_playerLife);
            }
            else
            {
                _shield_prefab.gameObject.SetActive(false);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D player_coll)
        {
            if (player_coll.gameObject.tag == "Powerup" && _playerLife >= 0)
            {
                switch (player_coll.gameObject.name)
                {
                    case "Powerup_shot(Clone)":
                        _triple_shot_active = true;
                        StartCoroutine(powerup_handler(_triple_shot_active));
                        Destroy(player_coll.gameObject);
                        break;
                    case "Powerup_speed(Clone)":
                        _speedvar += 1.5f;
                        StartCoroutine(powerup_handler(_speedvar));
                        Destroy(player_coll.gameObject);
                        break;
                    case "Powerup_shield(Clone)":
                        _shield_prefab.gameObject.SetActive(true);
                        Destroy(player_coll.gameObject);
                        break;
                    default:
                        Destroy(player_coll.gameObject);
                        break;
                }
            }
        }
        public void Score_add(int points)
        {
            _score_point += points;
            _ui_manager.UpdateScore(_score_point);
        }

        IEnumerator powerup_handler(bool var_name)
        {
            if (_triple_shot_active == false) { _triple_shot_active = true; }
            yield return new WaitForSeconds(5f);
            _triple_shot_active = false;
            StopCoroutine(powerup_handler(var_name));
        }
        IEnumerator powerup_handler(float var_name)
        {
            yield return new WaitForSeconds(5f);
            _speedvar -= 1.5f;
            StopCoroutine(powerup_handler(var_name));
        }
        public IEnumerator death()
        {
            transform.Find("Thruster").gameObject.SetActive(false);
            _playerAnimation.SetTrigger("Explosion");
            foreach (var item in fireDamage)
            {
                item.SetActive(false);
            }
            return null;
        }
} 
