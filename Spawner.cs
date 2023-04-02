using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject[] _powerups;
    public bool player_alive;
    [SerializeField]
    private Player temp_player;
    int life;
    void Start()
    {
        player_alive = true;
    }
    private void Update()
    {
        life=temp_player._playerLife;
        
        if (life>=0)
        {
            player_alive = true;
        }
        else if (life <0)
        {
            player_alive = false;
            StopAllCoroutines();
        }
    }
    public void StartSpawn()
    {

        StartCoroutine("spawnEnemy");
        StartCoroutine("spawnPower");
    }
    IEnumerator spawnEnemy()
    {
        while (player_alive)
        {
            Vector2 enemy_pos = new Vector2(Random.Range(-9.7f, 9.7f), 5.1f);
            
            Instantiate(_enemy, enemy_pos, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }

    }
    IEnumerator spawnPower()
    {
        while (player_alive)
        {
            Vector2 powerup_pos = new Vector2(Random.Range(-9.7f, 9.7f), 5.1f);
            int powerup_index = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[powerup_index], powerup_pos, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }

    }
}