using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public Animator animator;
    private Rigidbody2D rb;
    public GameObject foot;
    Animator player;
    AudioSource audio;


    bool Moving;
    //z and y movement
    [SerializeField]
    private Stat health;

    private float z;
    private float x;
    public float Speed;
    private float enemyKilled = 0;
    //public float health;

    private void Awake()
    {
        health.Initialize();
    }

    // Use this for initialization
    void Start () {

        Speed = 30;
        Moving = false;
        player = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        //health take damage
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            health.CurrentVal += 10;
        }

        if (health.CurrentVal == 0)
        {
            Dead();
        }
        if (enemyKilled == 19)
        {
            Invoke("Win", 4);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //Moving = Input.GetButtonDown("Jump");

        //if (Moving)
        //{
        //    animator.SetBool("isMoving", Moving);

        //}

        //if ((Input.GetKey(KeyCode.S)))
        //{
        //    Moving = false;
        //    animator.SetBool("isMoving", false);
        //}

       
        //Rotate to the mouse position
        var objectPos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - objectPos;


        if (transform.localScale.x != 2f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));

            //WASD movement to mouse position
            z = (Input.GetAxis("Vertical") * Time.deltaTime * 1.0f) * Speed;
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 1.0f * Speed;
            transform.Translate(x, z, 0, Space.World);


            //Animation for footstep
            if (z > 0 || z < 0 || x > 0 || x < 0)
            {
                animator.SetFloat("Forward", 1);
            }
            else
            {
                animator.SetFloat("Forward", 0);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Furniture")
        {
            Speed = 0;
            // how much the character should be knocked back
            float magnitude = 0.01f;
            // calculate force vector
            var force = transform.position - collision.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();

            gameObject.GetComponent<Rigidbody2D>().AddForce(force * magnitude);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            enemyKilled++;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Furniture")
        {
            Speed = 30;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
        }
    }

    void Dead()
    {
        transform.localScale = new Vector3(2F, 2f, 2f);
        this.GetComponent<BoxCollider2D>().enabled = false;
        foot.SetActive(false);
        player.SetTrigger("Dead");
        Debug.Log("Dead");
        Invoke("GameOver", 4);
    }

    void Win()
    {
        SceneManager.LoadScene("Win Scene");
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver Scene");
    }

    
    void TakeDamage()
    {
        audio.Play();
        health.CurrentVal -= 10;
    }

}
