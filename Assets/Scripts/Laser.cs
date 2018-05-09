using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField]
    private Stat health;

    public GameObject laser;
    public float scale = 100;
    public float interval = 1;
    private float rate = 1;
    public GameObject playerAudio;

    // Use this for initialization
    void Start () {

        playerAudio.GetComponent<AudioSource>();
        InvokeRepeating("LaserFlash", 0, rate);

    }
	
	// Update is called once per frame
	void Update () {

		interval = scale * Mathf.PerlinNoise(Time.deltaTime * scale, 1f)  ;
        if (interval < 10)
        {
            rate = 1;
        }
        if (interval > 10 && interval < 20)
        {
            rate = 2;
        }
        if (interval > 20 && interval < 30)
        {
            rate = 3;
        }
        if (interval > 30 && interval < 40)
        {
            rate = 4;
        }
        if (interval > 40 && interval < 50)
        {
            rate = 5;
        }
        if (interval > 50 && interval < 60)
        {
            rate = 6;
        }
        if (interval > 60)
        {
            rate = 7;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            InvokeRepeating("TakeDamage", 0.0f, 1f);
            playerAudio.GetComponent<AudioSource>().Play();

        }

        InvokeRepeating("LaserFlash", 0, rate);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CancelInvoke();
            InvokeRepeating("LaserFlash", 0, rate);
        }

        
    }

    void TakeDamage()
    {
        health.CurrentVal --;
    }

    void LaserFlash()
    {
        if (laser.activeSelf)
            laser.SetActive(false);
        else
            laser.SetActive(true);
    }

}
