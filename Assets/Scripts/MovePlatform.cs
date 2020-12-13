using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 2 || transform.position.x < -2)
        {
            speed = -speed;
        }
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        
    }
}
