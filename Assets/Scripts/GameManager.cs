using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int wave;
    public GameObject gameStart;
    public GameObject gameEnd;
    public TextMeshProUGUI waveText;
    private SpawnManager spawnManager;
    public bool isGameActive = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        wave = spawnManager.waveNumber;
        waveText.text = "Wave:" + wave;
    }
    public void StartGame(){
        isGameActive = true;
        gameStart.gameObject.SetActive(false);
    }
    public void GameOver(){
        isGameActive = false;
        gameEnd.gameObject.SetActive(true);
    }
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
