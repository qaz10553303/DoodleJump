using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject enemyPool;
    private Dictionary<float, GameObject> targetDistDict;
    private List<float> distList;
    private GameObject target;
    private int targetOrder;
    private float bulletSpeed=10;
    private AudioSource enemyHitByBulletAud;


    // Start is called before the first frame update
    void Start()
    {
        enemyHitByBulletAud = GameObject.Find("EnemyHitByBulletAud").GetComponent<AudioSource>();
        enemyPool = GameObject.Find("EnemyPool");
        distList = new List<float>();
        targetDistDict = new Dictionary<float, GameObject>();
        Destroy(gameObject, 5f);
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Enemy1"|| collision.tag == "Enemy2"|| collision.tag == "Enemy3")
        {
            Destroy(collision.gameObject);
            GameManager.bonus += 500;
            enemyHitByBulletAud.Play();
        }
    }

    public void SetTarget()
    {
        if (enemyPool.transform.childCount>0)
        {
            targetDistDict.Clear();
            distList.Clear();
            for (int i = 0; i < enemyPool.transform.childCount; i++)//add targets to dictionary and list
            {
                float targetDist = Vector2.Distance(enemyPool.transform.GetChild(i).position, transform.position);
                targetDistDict.Add(targetDist, enemyPool.transform.GetChild(i).gameObject);
                distList.Add(targetDist);
            }
            distList.Sort();//sort the list from min to max
            target = targetDistDict[distList[targetOrder]];//find and set the nearest target
            if (Vector2.Distance(target.transform.position, transform.position) < 8)
            {
                if(target.transform.position.y > transform.position.y+1)
                {
                    FlyToEnemy();
                }
                else
                {
                    targetOrder += 1;
                    SetTarget();
                }
            }
            else
            {
                FlyToSky();
            }
        }
        else
        {
            FlyToSky();
        }
    }

    public void FlyToEnemy()
    {
        Vector2 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(targetDir*bulletSpeed,ForceMode2D.Impulse);
        Debug.Log(targetDir);
    }

    public void FlyToSky()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0,8f), ForceMode2D.Impulse);
    }

}
