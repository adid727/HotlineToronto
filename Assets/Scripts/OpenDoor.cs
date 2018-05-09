using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    private bool openDoor;
    private float angleOfDoor;
    private bool reverse;
    private void Start()
    {
        openDoor = false;
        reverse = false;
        angleOfDoor = transform.rotation.z;
    }
    private void Update()
    {
        if (openDoor)
        {
            float newAngle = transform.rotation.z;
            newAngle -= 10 * Time.deltaTime;

            if (!reverse)
            {
                transform.rotation = new Quaternion(transform.GetChild(0).rotation.x, transform.GetChild(0).rotation.y, newAngle, transform.GetChild(0).rotation.w);
                if ((angleOfDoor - 1.15f) > newAngle)
                {
                    openDoor = false;
                }
            }

            else
            {
                newAngle += 100 * Time.deltaTime;
                transform.rotation = new Quaternion(transform.GetChild(0).rotation.x, transform.GetChild(0).rotation.y, newAngle, transform.GetChild(0).rotation.w);
                if ((angleOfDoor + 1.15f) < newAngle)
                {
                    openDoor = false;
                }
            }
        }
               
        Debug.Log(openDoor);
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
        }

        if(collision.gameObject.tag == "Player")
        {
            openDoor = true;
            if(collision.gameObject.transform.position.y > transform.position.y && transform.GetChild(0).transform.rotation.z == 0)
            {
                reverse = true;
            }
            else
            {
                reverse = false;
            }

            this.GetComponent<BoxCollider2D>().enabled = false;

        }
    }


}
