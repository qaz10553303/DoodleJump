using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isDefeated=false;
    private bool isLanding = false;
    private float speed = 0.5f;
    private AudioSource superJumpAud;
    private AudioSource enemyDefeatAud;


    // Start is called before the first frame update
    void Start()
    {
        superJumpAud = GameObject.Find("SuperJumpAud").GetComponent<AudioSource>();
        enemyDefeatAud = GameObject.Find("EnemyDefeatAud").GetComponent<AudioSource>();
        if (transform.position.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX=true;
            speed = -speed;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < Camera.main.transform.position.y - 5)
        {
            Destroy(gameObject);
        }


        if (tag == "Enemy2" && !isDefeated && !GameManager.isDead)
        {
            if (transform.position.x > 2 || transform.position.x < -2)
            {
                speed = -speed;
            }
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }

        if (tag == "Enemy3"&&transform.position.y-Camera.main.transform.position.y<8&&!isDefeated && !GameManager.isDead)
        {
            Enemy3Behavior();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag == "Enemy3" && collision.tag == "SimplePlatform" || collision.tag == "SuperPlatform" || collision.tag == "OneTimePlatform" || collision.tag == "MovePlatform")
        {
            isLanding = true;
        }
        if (GameManager.flyTime > 0&& collision.attachedRigidbody.transform.tag=="Player"&&!isDefeated)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 1f;
            Vector2 angle = new Vector2(Random.value < 0.5 ? -1f : 1f, 6f);
            Debug.Log(angle);
            GetComponent<Rigidbody2D>().AddForce(angle, ForceMode2D.Impulse);
            isDefeated = true;
            GameManager.bonus += 500;
            enemyDefeatAud.Play();
        }
        else if (collision.tag == "Body"&&!isDefeated)
        {
            collision.transform.parent.GetComponent<PlayerController>().PlayerDie();
        }
        else if(collision.tag == "Feet"&& collision.transform.parent.GetComponent<Rigidbody2D>().velocity.y<0)
        {
            GetComponent<Rigidbody2D>().gravityScale = 2f;
            collision.transform.parent.GetComponent<PlayerController>().Jump();
            superJumpAud.Play();
            isDefeated = true;
            GameManager.bonus += 500;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tag == "Enemy3" && collision.tag == "SimplePlatform" || collision.tag == "SuperPlatform" || collision.tag == "OneTimePlatform" || collision.tag == "MovePlatform")
        {
            isLanding = false;
        }
    }

    void Enemy3Behavior()
    {
        if (!isLanding)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        }
        else if (isLanding)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0f;
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }
    }

}
