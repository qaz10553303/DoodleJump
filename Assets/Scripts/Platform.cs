using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private AudioSource brokenPlatformAud;
    private AudioSource superJumpAud;
    // Start is called before the first frame update
    void Start()
    {
        brokenPlatformAud=GameObject.Find("BrokenPlatformAud").GetComponent<AudioSource>();
        superJumpAud= GameObject.Find("SuperJumpAud").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Feet"&&collision.transform.parent.GetComponent<Rigidbody2D>().velocity.y < 0 )
        {
            if (tag == "SimplePlatform" || tag == "Ground" || tag == "MovePlatform")
            {
                collision.transform.parent.GetComponent<PlayerController>().Jump();
            }
            else if (tag == "OneTimePlatform")
            {
                collision.transform.parent.GetComponent<PlayerController>().Jump();
                GetComponent<Rigidbody2D>().gravityScale = 2f;
            }
            else if (tag == "SuperPlatform"&&GameManager.flyTime<0)
            {
                collision.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.transform.parent.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 15f), ForceMode2D.Impulse);
                superJumpAud.Play();
            }
            else if (tag == "BrokenPlatform")
            {
                GetComponent<Rigidbody2D>().gravityScale = 2f;
                brokenPlatformAud.Play();
            }

        }
    }


}
