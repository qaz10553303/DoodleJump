using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed = 4;
    private float flySpeed = 10;
    public GameObject bulletPrefab;
    private Vector2 firePos;
    public GameObject bulletPool;
    private bool isShoot;
    public Sprite shootSp;
    public Sprite idleSp;
    private float shootEndTimer = 0;
    private bool isFlying;
    public GameObject flyingHat;
    public GameObject rocket;
    private AudioSource aud;
    public AudioSource shootAud;
    public AudioSource flyingHatAud;
    public AudioSource rocketAud;
    public AudioSource dieAud;
    

    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlying&&!GameManager.isDead)
        {
            Shoot();
        }
        if (!isShoot && !GameManager.isDead)
        {
            Move();
            shootEndTimer += Time.deltaTime;
            if (shootEndTimer > 0.2f&&GameManager.flyTime<0.1f)
            {
                GetComponent<SpriteRenderer>().sprite = idleSp;
            }
        }
        if (GameManager.flyTime > 0 && !GameManager.isDead)
        {
            isFlying = true;
            EnterFlyMode();
        }
        if (GameManager.isDead)
        {
            GameObject.Find("Feet").GetComponent<BoxCollider2D>().enabled = false;
            GameObject.Find("Body").GetComponent<BoxCollider2D>().enabled = false;
        }
    }



    void Move()
    {
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(Input.touchCount - 1);//save last touch infomation
            if (myTouch.phase == TouchPhase.Stationary || myTouch.phase == TouchPhase.Moved)//if user continues the touch
            {
                if (myTouch.position.x > Screen.width / 2)//if touch on the right side
                {
                    transform.position = new Vector2(transform.position.x + moveSpeed*Time.deltaTime, transform.position.y);//move to the right
                    GetComponent<SpriteRenderer>().flipX=false;
                    rocket.transform.localPosition = new Vector2(-0.5f, -0.5f);
                }
                else if (myTouch.position.x < Screen.width / 2)//if touch on the left side
                {
                    transform.position = new Vector2(transform.position.x - moveSpeed*Time.deltaTime, transform.position.y);//move to the left
                    GetComponent<SpriteRenderer>().flipX = true;
                    rocket.transform.localPosition = new Vector2(0.5f, -0.5f);
                }

            }
        }
        if (transform.position.x < -2.5f)
        {
            transform.position= new Vector2(2.5f, transform.position.y);
        }
        if (transform.position.x > 2.5f)
        {
            transform.position = new Vector2(-2.5f, transform.position.y);
        }
    }

    void Shoot()
    {
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(Input.touchCount - 1);
            if (myTouch.phase == TouchPhase.Began)
            {
                if (myTouch.position.y > Screen.height / 2)
                {
                    firePos = new Vector2(transform.position.x, transform.position.y + 0.5f);
                    Instantiate(bulletPrefab, firePos, Quaternion.identity, bulletPool.transform);
                    isShoot = true;
                    GetComponent<SpriteRenderer>().sprite = shootSp;
                    shootEndTimer = 0;
                    shootAud.Play();
                }
            }
            if (myTouch.phase == TouchPhase.Ended)
            {
                isShoot = false;
            }
        }
    }


    public void Jump()
    {
        if (!isFlying&&!GameManager.isDead)
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
            aud.Play();
        }

    }

    public void PlayerDie()
    {
        Invoke("ShowResult", 3f);
        if (!GameManager.isJumpOff)
        {
            dieAud.Play();
        }
        GameManager.isDead = true;
    }

    private void ShowResult()
    {
        SceneManager.LoadScene(3);
    }

    public void EnterFlyMode()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = new Vector2(transform.position.x, transform.position.y + flySpeed * Time.deltaTime);
        if (GameManager.flyTime <= 0.1)
        {
            ExitFlyMode();
        }
    }

    void ExitFlyMode()
    {
        isFlying = false;
        GetComponent<SpriteRenderer>().sprite = idleSp;
        flyingHat.SetActive(false);
        rocket.SetActive(false);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 8f), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hat")
        {
            ExitFlyMode();
            GameManager.flyTime = 3;
            Destroy(collision.gameObject);
            rocket.SetActive(false);
            flyingHat.SetActive(true);
            flyingHatAud.Play();
        }
        else if (collision.tag == "Rocket")
        {
            ExitFlyMode();
            flyingHat.SetActive(false);
            rocket.SetActive(true);
            GameManager.flyTime = 5;
            Destroy(collision.gameObject);
            rocketAud.Play();
        }

    }

}
