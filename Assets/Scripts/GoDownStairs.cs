using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDownStairs : MonoBehaviour {
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = new Vector3(-17.7f, 89.5f, -0.23697f);
        }
    }
}

