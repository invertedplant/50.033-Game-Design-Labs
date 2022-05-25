using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 70;
    public float upSpeed = 22;
    public float maxSpeed = 10;
    public float maxAirSpeed = 12;
    public Transform enemyLocation;
    public TMP_Text scoreText;
    private int score = 0;
    private bool countScoreState = false;
    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // Even before you start, can be called many times
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (Input.GetButtonUp("Horizontal"))
        {
            marioBody.velocity = Vector2.zero;
        }

        if (Input.GetButtonDown("Jump") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            countScoreState = true; //check if Gomba is underneath
        }

        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.x < maxSpeed && onGroundState == true)
                marioBody.AddForce(movement * speed);
            else if (Mathf.Abs(marioBody.velocity.x) < maxAirSpeed && onGroundState == false)
                marioBody.AddForce(movement * speed);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
        }
        // when jumping, and Gomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                score++;
                Debug.Log(score);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");
            marioBody.velocity = Vector2.zero;
            // this.enabled = false;
            Time.timeScale = 0.0f;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGroundState = true; // back on ground
            countScoreState = false; // reset score state
            scoreText.text = "Score: " + score.ToString();
        };
    }
}
