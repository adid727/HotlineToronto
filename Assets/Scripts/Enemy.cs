using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour {

    public GameObject radar;
    Animator animator;
    public GameObject foot;
    bool attack;
    public Transform[] patrolPoints;
    public float speed;
    Transform currPatrolPoint;
    int currPatrolIndex;
    

    public Transform target;
    public float chaseRange;

    public float attackRange;
    public float damage;

    private float lastAttackTime;
    public float attackDelay;

    public float awareRange;
    public float targetDist;
    private Rigidbody2D rb;
    public Vector3 orientation = Vector3.up;


    bool isDead = false;

    Ray2D LeftRay;
    Ray2D RightLeft;
    
    public GameObject RayLeft;
    public GameObject RayRight;

    

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        
        attack = false;

        currPatrolIndex = 0;
        currPatrolPoint = patrolPoints[currPatrolIndex];
        rb = GetComponent<Rigidbody2D>();
        radar.GetComponent<Radar>();
    }

    // Update is called once per frame
    void Update()
    {

        

        targetDist = Vector3.Distance(transform.position, target.position);


        if(isDead != true){



            AvoidObstacles();
            //if (targetDist > awareRange)
            //{


            Patrol();
            //}
            if (radar.GetComponent<Radar>().Chase == true)
            {
                Chase();
            }

            if (targetDist < awareRange && targetDist > attackRange )
            {
                    //Chase();
            }

            if (targetDist < attackRange)
            {
                Attack();
            }
            if (attack)
            {
                animator.SetBool("Attack", true);
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }

        //if ((Input.GetKeyUp(KeyCode.A)))
        //{
        //    attack = false;
        //    animator.SetBool("Attack", false);
        //}

    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        foot.SetActive(false);
    //        animator.SetTrigger("Dead");
    //        Debug.Log("Dead");
    //    }
        
    //}

    public void Patrol()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, currPatrolPoint.position) < 0.1f)
        {
            if (currPatrolIndex + 1 < patrolPoints.Length)
            {
                currPatrolIndex++;
            }
            else
            {
                currPatrolIndex = 0;
            }

            currPatrolPoint = patrolPoints[currPatrolIndex];
        }

        Vector3 patrolPointDir = currPatrolPoint.position - transform.position;
        float angle = Mathf.Atan2(patrolPointDir.y, patrolPointDir.x) * Mathf.Rad2Deg - 90f;
        
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180f);

        attack = false;
    }

    public void Chase()
    {
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        
        
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180f);

        transform.Translate(Vector3.up * Time.deltaTime * speed);

        attack = true;
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + attackDelay)
        {
            target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            lastAttackTime = Time.time;

        }
        attack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "Furniture")
        {
            // how much the character should be knocked back
            float magnitude = 0.01f;
            // calculate force vector
            var force = transform.position - collision.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();

            gameObject.GetComponent<Rigidbody2D>().AddForce(force * magnitude);
        }

        if (collision.gameObject.tag == "Player")
        {
            Die();
        }
    }

    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Furniture")
        {
            speed = 5;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
        }
    }

    //Death function 
    void Die()
    {
        
        
        radar.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponentInChildren<BoxCollider2D>().enabled = false;
        foot.SetActive(false);
        animator.SetTrigger("Dead");
        Debug.Log("Dead");
        isDead = true;
        radar.GetComponent<Radar>().Chase = false;
    }

    //Avoiding obstacles
    public void AvoidObstacles()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = (player.transform.position - transform.position).normalized;

        //Ray in front of the enemy
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position, 3);
        // Debug.Log(hit.transform.gameObject.tag);
        if (hit)
            if (hit.transform.gameObject.tag == "Furniture")
            {
                Debug.Log("we hit Furniture!");
                Quaternion q = Quaternion.AngleAxis(10, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180f);
                transform.Translate(Vector3.up * Time.deltaTime * speed);
            }

        //Create more rays (left and right) and repeat the process
        //Apply the direction to the transform.rotation?


    }

    



}
