using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 70;
    public float upSpeed = 40;
    public float maxSpeed = 10;
    public float maxAirSpeed = 8;
    public Transform enemyLocation;
    /* public TMP_Text scoreText;
    private int score = 0;
    private bool countScoreState = false; removed for week 2*/
    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    // week 2 starts here
    private Animator marioAnimator;
    private AudioSource marioAudio;

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
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (Input.GetButtonUp("Horizontal"))
        {
            marioBody.velocity = Vector2.zero;
        }

        if (Input.GetButton("Jump") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            /* countScoreState = true; //check if Gomba is underneath, removed for week 2 */
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
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (Mathf.Abs(marioBody.velocity.x) > 0.5)
                marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (Mathf.Abs(marioBody.velocity.x) > 0.5)
                marioAnimator.SetTrigger("onSkid");
        }
        // when jumping, and Gomba is near Mario and we haven't registered our score
        /*         if (!onGroundState && countScoreState) removed for week 2
                {
                    if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
                    {
                        countScoreState = false;
                        score++;
                        Debug.Log(score);
                    }
                } */
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround", onGroundState);

    }

    /*     void OnTriggerEnter2D(Collider2D other) removed for week 2
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Collided with Gomba!");
                marioBody.velocity = Vector2.zero;
                // this.enabled = false;
                Time.timeScale = 0.0f;
            }
        } */

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGroundState = true; // back on ground
            /*             countScoreState = false; // reset score state
                        scoreText.text = "Score: " + score.ToString(); removed for week 2 */
        };

        if (col.gameObject.CompareTag("Obstacle") && Mathf.Abs(marioBody.velocity.y) < 0.01f)
        {
            onGroundState = true; // back on ground
            /*             countScoreState = false; // reset score state
                        scoreText.text = "Score: " + score.ToString(); removed for week 2 */
        }
    }

    void PlayJumpSound()
    {
        marioAudio.PlayOneShot(marioAudio.clip);
    }
}
