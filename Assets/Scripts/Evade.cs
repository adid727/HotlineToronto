using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Evade : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float dist;
    private float range;

    

    void Update()
    {
        Vector2 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        range = Vector2.Distance(transform.position, target.position);

        if (range < dist)
        { 
            transform.position = Vector2.MoveTowards(transform.position, target.position, -1 * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Win();
        }
    }

    void Win()
    {
        SceneManager.LoadScene("Win Scene");
    }
}
