using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PowerUpType currentPowerUp = PowerUpType.None;   //決定目前powerup種類
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public float speed = 5.0f;
    public bool hasPowerup;
    public GameObject powerupIndicator;
    private Coroutine powerupCountdown;
    private GameManager gameManager;
    //For powerup_strong
    private float powerupStrength = 15.0f;
    //For powerup_bullet
    public GameObject bulletPrefab;
    private GameObject tmpBullet;
    //For powerup_smash
    private float hangTime = 0.2f;
    private float smashSpeed = 50f;
    private float explosionForce = 50f;
    private float explosionRadius = 10f;
    bool smashing = false;
    float floorY;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);          //旋轉慣性
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);            //powerupIndicator位置
        //玩家墜落。遊戲結束
        if (transform.position.y < -10){
            gameManager.GameOver();
        }
        //觸發Powerup_strong
        if (currentPowerUp == PowerUpType.Bullet){
            powerupIndicator.GetComponent<Renderer>().material.color = new Color(255, 130, 0, 255);     //powerupIndicator顏色
        }
        //觸發Powerup_bullet
        if (currentPowerUp == PowerUpType.Bullet){
            powerupIndicator.GetComponent<Renderer>().material.color = Color.green;             
            if (Input.GetKeyDown(KeyCode.Space)){
                LaunchBullets();
            }
        }
        //觸發Powerup_smash
        if (currentPowerUp == PowerUpType.Smash){
            powerupIndicator.GetComponent<Renderer>().material.color = Color.red;
            if (Input.GetKeyDown(KeyCode.Space) && !smashing){
                smashing = true;
                StartCoroutine(Smash());
            }
        }

    }
    //觸發Powerup判定
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Powerup")){
            hasPowerup = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;      //powerup種類判定
            Destroy(other.gameObject);
            if (powerupCountdown != null){
                StopCoroutine(powerupCountdown);
            }   //避免多個種類鑽石共用時間
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }
    //Powerup計時器
    IEnumerator PowerupCountdownRoutine(){
        yield return new WaitForSeconds(7);         //before we do something
        hasPowerup = false;
        currentPowerUp = PowerUpType.None;
        powerupIndicator.gameObject.SetActive(false);
    }
    //Powerup_strong 功能
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Strong){
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);   //ForceMode.Impulse：施加瞬間力道
        }
    }
    //Powerup_bullet 功能
    void LaunchBullets(){
        foreach (var enemy in FindObjectsOfType<Enemy>()){
            tmpBullet = Instantiate(bulletPrefab, transform.position + 2 * Vector3.up , Quaternion.identity);
            tmpBullet.GetComponent<BulletBehaviour>().Fire(enemy.transform);  
        }
    }
    //Powerup_smash 功能
    IEnumerator Smash(){
        var enemies = FindObjectsOfType<Enemy>();
        floorY = transform.position.y;          //Store the y position before taking off
        float jumpTime = Time.time + hangTime;  //Calculate the amount of time we will go up
        //Time.time:當前update開始之時間，Time.deltaTime:自上一幀以來經過的時間
        //Move up
        while (Time.time < jumpTime){
            //move the player up while still keeping their x velocity.
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }
        //Move down
        while (transform.position.y > floorY){
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        for (int i = 0; i < enemies.Length; i++){
            if (enemies[i] != null){
                //Apply an explosion force that originates from our position.
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 
                explosionRadius, 0.0f, ForceMode.Impulse);
            }
        }
        smashing = false;
    }
}
