using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > transform.position.y&&player.GetComponent<Rigidbody2D>().velocity.y>0)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y,transform.position.z);
        }
        else if(player.transform.position.y > transform.position.y &&GameManager.flyTime>0)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
    }
}
