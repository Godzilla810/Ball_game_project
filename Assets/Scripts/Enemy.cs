using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject player;
    public bool isBoss = false;
    public float spawnInterval;
    private float nextSpawn;
    public int miniEnemySpawnCount;
    private SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if (isBoss){
            spawnManager = FindObjectsOfType<SpawnManager>()[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        //normalized不加的話，距離越遠跑越快
        enemyRb.AddForce(lookDirection * speed);

        if(isBoss){
            if (Time.time > nextSpawn){
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }
        //刪除掉出場外的Enemy
        if(transform.position.y < -10){
            Destroy(gameObject);
        }
    }
}
