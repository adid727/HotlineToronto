using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gun : MonoBehaviour {

    Animator animator;
    public GameObject foot;
    bool Attack;
    public GameObject muzzle;


    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();
        muzzle.SetActive(false);
        Attack = false;

    }
	
	// Update is called once per frame
	void Update () {

        Attack = Input.GetKey(KeyCode.A);

        if (Attack)
        {
            
            animator.SetBool("isAttacking", true);
            //spRndrer.enabled = true;
            //StartCoroutine(Flash(flashSpeed));
            StartCoroutine(BlinkText());

        }

        if ((Input.GetKeyUp(KeyCode.A)))
        {
            
            Attack = false;
            animator.SetBool("isAttacking", false);
            //spRndrer.enabled = false;
            muzzle.SetActive(false);
            StopCoroutine("BlinkText");
        }
        

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            foot.SetActive(false);
            animator.SetTrigger("Dead");
            Debug.Log("Dead");
        }

    }

    public IEnumerator BlinkText()
    {
        
            //set the Text's text to blank
            muzzle.SetActive(true);
            //turn on for 0.5 seconds
            yield return new WaitForSeconds(0.5f);
            //turn off for the next 0.5 seconds
            muzzle.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        
    }
}
