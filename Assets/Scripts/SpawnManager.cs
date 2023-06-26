using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject[] miniEnemyPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs; 
    private float SpawnRange = 9.0f;
    public int enemyCount;
    public int waveNumber = 1;
    public int bossRound;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(powerupPrefabs[0], GenerateSpawnPosition(), 
        powerupPrefabs[0].transform.rotation);
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0){
            waveNumber++;
            //Spawn a boss every x number of waves
            if (waveNumber % bossRound ==0){
                SpawnBossWave(waveNumber);
            }
            else{
                SpawnEnemyWave(waveNumber);
            }
            int powerupIndex = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[powerupIndex], GenerateSpawnPosition(), 
            powerupPrefabs[powerupIndex].transform.rotation);
        }
    }
    //隨機產生位置
    private Vector3 GenerateSpawnPosition(){
        float spawnPosX = Random.Range(-SpawnRange, SpawnRange);
        float spawnPosZ = Random.Range(-SpawnRange, SpawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
    void SpawnBossWave(int currentRound){
        int miniEnemyToSpawn;
        if (bossRound != 0){
            miniEnemyToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemyToSpawn = 1;
        }
        var boss = Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
        boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemyToSpawn; 
    }
    //產生敵人
    void SpawnEnemyWave(int enemiesToSpawn){
        for (int i = 0; i < enemiesToSpawn; i++){
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[enemyIndex], GenerateSpawnPosition(), 
            enemyPrefabs[enemyIndex].transform.rotation);
        }
    }
    public void SpawnMiniEnemy(int amount){
        for (int i = 0; i < amount; i++){
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(), 
            miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }
}
