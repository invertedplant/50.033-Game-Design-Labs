using System.Security.Cryptography;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    public float speed = 3.5f;
    private bool collidedWithMario;
    private bool launched = false;
    private Rigidbody2D mushroomBody;
    private bool movingRightState;
    private bool onGround;
    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomBody.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
        launched = true;
        if (rand.NextDouble() > 0.5){
            movingRightState = true;
        } 
        else{
            movingRightState = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(!launched || collidedWithMario) {return;}
        var velocity =  mushroomBody.velocity;
        velocity = movingRightState ? new Vector2(speed, velocity.y) : new Vector2(-speed, velocity.y);
        mushroomBody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!collidedWithMario && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Mushroom hit Mario!");
            collidedWithMario = true;
            mushroomBody.velocity = Vector2.zero;
            return;
        }

        var direction = other.GetContact(0).normal;
        if (direction == Vector2.up) {return;}
        if (other.gameObject.CompareTag("Obstacle"))
        {
            movingRightState = !movingRightState;
        }
        
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
