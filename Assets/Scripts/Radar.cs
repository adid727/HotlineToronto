using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {

    public GameObject enemy;
    public bool Chase = false;
    public GameObject playerAudio;

    [SerializeField]
    private Stat health;

    // Use this for initialization
    void Start () {

        enemy.GetComponent<Enemy>();
        playerAudio.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            InvokeRepeating("TakeDamage", 0.2f, 1f); 
            Chase = true;
            Debug.Log("Chasing");
        } 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CancelInvoke();
            Chase = false;
        }
    }

    void TakeDamage()
    {
        playerAudio.GetComponent<AudioSource>().Play();
        health.CurrentVal -= 10;
        
    }


}
