﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public Vector3 velocity;
    public Vector3 acceleration = Vector3.zero;
    static float desiredSeparion = 2.5f;
    static float neighborDistance = 7;    
    float maxForce = 2;
    float maxSpeed = 1;
    public Flock flock;
    
	// Use this for initialization
	void Start () {
        float angle = Random.Range(0, Mathf.PI * 2);
        velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 sep = Separate(flock.GetBoids());
        Vector3 ali = Align(flock.GetBoids());
        Vector3 coh = Cohesion(flock.GetBoids());

        acceleration = Vector3.zero;
        acceleration += sep * 1.5f;
        acceleration += ali;
        acceleration += coh;

        Vector3 avoid = AvoidObstacles(flock.GetObstacles());
        acceleration += avoid;


        float dt = Time.deltaTime;
        dt *= 5;
        
        Vector3 pos = transform.position;
        pos += velocity * dt + 0.5f * acceleration * dt * dt;
        velocity += acceleration * dt;
        acceleration = Vector3.zero;
        
        if (pos.x > flock.GetComponentInChildren<BoxCollider2D>().bounds.max.x) velocity.x *=-1;
        if (pos.x < flock.GetComponentInChildren<BoxCollider2D>().bounds.min.x) velocity.x *= -1;
        if (pos.y > flock.GetComponentInChildren<BoxCollider2D>().bounds.max.y) velocity.y *= -1;
        if (pos.y < flock.GetComponentInChildren<BoxCollider2D>().bounds.min.y) velocity.y *= -1;
        transform.position = pos;


        float angle = Mathf.Acos(Vector3.Dot(velocity.normalized, Vector3.up));

        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle * (velocity.x > 0 ? -1:1));
	}

    Vector3 Separate(List<GameObject> boids)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject other in boids)
        {
            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < desiredSeparion))
            {
                // Calculate vector pointing away from neighbor
                Vector3 diff = transform.position - other.transform.position;
                diff.Normalize();
                diff /= d;
                steer += diff;
                count++;
            }
        }
        if (count > 0)
        {
            steer /= count;
        }

        if (steer.magnitude > 0)
        {
            steer.Normalize();
            steer *= maxSpeed;
            steer -= velocity;
            if (steer.magnitude > maxForce)
            {
                steer.Normalize();
                steer *= maxForce;
            }
        }
        return steer;
    }

    Vector3 Align(List<GameObject> boids)
    {
        // For every nearby boid in the system, calculate the average velocity
        Vector3 sum = Vector3.zero;
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject other in boids)
        {
            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < neighborDistance/* neighbordist */))
            {
                // To do : Calculate the sum of all the velocities of the boids                
                    sum+= other.GetComponent<Boid>().velocity;
                count++;
            }
        }

        if (count > 0)
        {
            // To do: Implement Rynolds: Steering = Desired - Velocity
            // The magnitude of steer should not be greater than maxForce
                sum/=((float)count);
                // First two lines of code below could be condensed with new PVector setMag() method 
                // Not using this method until Processing.js catches up 
                // sum.setMag(maxspeed); 

                // Implement Reynolds: Steering = Desired - Velocity 
                sum = sum.normalized;
                sum *= (maxSpeed);
                steer = sum - velocity;
                //steer.limit(maxforce);
                if(steer.x > maxForce)
                {
                    steer.x = maxForce;
                }
                if (steer.y > maxForce)
                {
                    steer.y = maxForce;
                }
                if (steer.y > maxForce)
                {
                    steer.y = maxForce;
                }
            return steer;
            }
            else
            {
                return new Vector3();
            }
        

        return steer;
    }

    Vector3 Cohesion(List<GameObject> boids)
    {
        // For the average position (i.e. center) of all nearby boids, calculate steering vector towards that position
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach(GameObject other in boids)
        {
            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d>0) && (d<neighborDistance))
            {
                // To do : Calculate the sum of all the positions of the boids    
                 
                sum += transform.position; // Add position 
                count++;
            }
        }

        if (count > 0)
        {
            Vector3 averagePos = Vector3.zero;
            // To do: Calculate the average position and steer toward the position
            sum /= (count);
            return Seek(sum);  // Steer towards the position 

        }
        
        return Vector3.zero;        
    }



    Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired /= maxSpeed;

        Vector3 steer = desired - velocity;
        if (steer.magnitude > maxForce)
        {
            steer.Normalize();
            steer *= maxForce;
        }

        return steer;
    }

    Vector3 AvoidObstacles(List<GameObject> obstacles)
    {
        Vector3 steer = Vector3.zero;
        float collision_visibilty = 20;
        float obstacle_radius = 5;

        foreach (GameObject obstacle in obstacles)
        {
            Vector3 a = obstacle.transform.position - transform.position;
            Vector3 u = velocity.normalized;
            Vector3 v = u * collision_visibilty;
            Vector3 p = Vector3.Dot(a, u) * u;
            Vector3 b = p - a;

            if ((b.magnitude < obstacle_radius) && (p.magnitude < v.magnitude))
            {
                Vector3 n = new Vector3(a.y, -a.x, 0);
                Vector3 desired = Vector3.zero;
                float dir = Vector3.Dot(n, v);
                steer = n.normalized * maxSpeed * dir / Mathf.Abs(dir);
                if (steer.magnitude > maxForce)
                {
                    steer.Normalize();
                    steer *= maxForce;
                }
            }
        }

        return steer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(4, 4, 0);
    }
}
