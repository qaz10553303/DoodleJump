using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject simplePlatformPrefab;
    public GameObject movePlatformPrefab;
    public GameObject brokenPlatformPrefab;
    public GameObject superPlatformPrefab;
    public GameObject oneTimePlatformPrefab;
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;
    public GameObject itemHatPrefab;
    public GameObject itemRocketPrefab;

    public GameObject tilePool;
    public GameObject enemyPool;
    public GameObject itemPool;

    public AudioSource JumpOffAud;


    public Text scoreText;

    private GameObject ground;
    private GameObject player;
    private GameObject newTile;
    private float minGenerateHeight = 1f;
    private float maxGenerateHeight = 3f;
    private float nextItemGenerateTimer;

    private Vector2 lastTilePos;
    

    public static int score;
    public static int bonus;
    public static float flyTime;
    public static bool isDead;
    public static bool isJumpOff;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ground = GameObject.FindGameObjectWithTag("Ground");
        newTile = ground;
        lastTilePos = ground.transform.position;
        score = 0;
        bonus = 0;
        flyTime = 0;
        isDead = false;
        isJumpOff = false;
        nextItemGenerateTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        flyTime -= Time.deltaTime;
        nextItemGenerateTimer -= Time.deltaTime;
        UpdateScore();
        DestroyTiles();
        DestroyItems();
        if (!isDead)
        {
            CheckPlayerDeath();
        }

        if (tilePool.transform.childCount < 50)
        {
            GenerateTile();
        }
        
    }

    void GenerateTile()//60%simple, 10%broken,10%super,10%oneTime,10%move
    {
        float newX = Random.Range(-2f, 2f);
        float newY = Random.Range(lastTilePos.y+minGenerateHeight, lastTilePos.y + maxGenerateHeight);

        int type = Random.Range(1, 11);
        if (type <= 6)//simple
        {
            newTile = Instantiate(simplePlatformPrefab, new Vector2(newX, newY), Quaternion.identity, tilePool.transform);
            GenerateEnemy(lastTilePos);
        }
        else if (type == 7)//broken
        {
            Instantiate(brokenPlatformPrefab, new Vector2(newX, newY), Quaternion.identity, tilePool.transform);
        }
        else if (type == 8)//super
        {
            newTile = Instantiate(superPlatformPrefab, new Vector2(newX, newY), Quaternion.identity, tilePool.transform);
            GenerateEnemy(lastTilePos);
        }
        else if (type == 9)//oneTime
        {
            newTile = Instantiate(oneTimePlatformPrefab, new Vector2(newX, newY), Quaternion.identity, tilePool.transform);
            GenerateEnemy(lastTilePos);
        }
        else if (type == 10)//move
        {
            newTile = Instantiate(movePlatformPrefab, new Vector2(newX, newY), Quaternion.identity, tilePool.transform);
        }
        lastTilePos = newTile.transform.position;
        if (nextItemGenerateTimer < 0)
        {
            GenerateItem(newX, newY);
            nextItemGenerateTimer = 20f;
        }
    }

    void GenerateItem(float x,float y)
    {
        if (Random.value < 0.5)
        {
            GameObject item = Instantiate(itemHatPrefab, new Vector2(x, y + 0.5f), Quaternion.identity, itemPool.transform);
        }
        else
        {
            GameObject item = Instantiate(itemRocketPrefab, new Vector2(x, y + 0.5f), Quaternion.identity, itemPool.transform);
        }
    }

    void GenerateEnemy(Vector2 lastTilePos)//10% chance to spawn enemy1&2, 5% chance to spawn enemy3
    {
        if (Random.value < 0.1f)
        {
            Instantiate(enemy1Prefab, new Vector2(lastTilePos.x + Random.Range(-0.5f, 0.5f), lastTilePos.y + Random.Range(0f, 0.5f)), Quaternion.identity, enemyPool.transform);
        }

        else if (Random.value < 0.1f)
        {
            Instantiate(enemy2Prefab, new Vector2(lastTilePos.x + Random.Range(-0.5f,0.5f), lastTilePos.y + Random.Range(0f, 0.5f)), Quaternion.identity, enemyPool.transform);
        }

        else if (Random.value < 0.05f)
        {
            Instantiate(enemy3Prefab, new Vector2(lastTilePos.x, lastTilePos.y + 0.5f), Quaternion.identity, enemyPool.transform);
        }
    }


    void UpdateScore()
    {
        if(Mathf.RoundToInt(player.transform.position.y * 100 - ground.transform.position.y+1 * 100) > score)
        {
            score = Mathf.RoundToInt(player.transform.position.y*100 - ground.transform.position.y+1*100);
        }
        int finalScore = score + bonus;
        scoreText.text = "Score: " + finalScore;
    }

    void DestroyTiles()
    {
        for (int i = 0; i < tilePool.transform.childCount; i++)
        {
            if (tilePool.transform.GetChild(i).position.y < Camera.main.transform.position.y - 5)
            {
                Destroy(tilePool.transform.GetChild(i).gameObject);
                break;
            }
        }
    }

    void DestroyItems()
    {
        for (int i = 0; i < itemPool.transform.childCount; i++)
        {
            if (itemPool.transform.GetChild(i).position.y < Camera.main.transform.position.y - 5)
            {
                Destroy(itemPool.transform.GetChild(i).gameObject);
                break;
            }
        }
    }

    void CheckPlayerDeath()
    {
        if(player.transform.position.y < Camera.main.transform.position.y - 6)
        {
            JumpOffAud.Play();
            isJumpOff = true;
            player.GetComponent<PlayerController>().PlayerDie();
        }
    }
}
