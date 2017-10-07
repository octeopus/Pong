/*
 * Script for Ball Prefabs in game. Affects Ball Behavior
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    private float speed = 15f;
    [SerializeField]
    private Rigidbody ball;
    private bool isActive; //isActive is used to see if the ball can reflect off something.
    private GameController _gc;

	// Use this for initialization

    public void init(GameController gc)
    {
        _gc = gc;
        ball = GetComponent<Rigidbody>();
        int coin = Random.Range(0, 2);
        
        if(coin == 0) { ball.velocity = Vector3.right * speed; }
        else { ball.velocity = Vector3.left * speed; }
        
        isActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (isActive) {

            if (collision.collider.CompareTag("Paddle"))
            {
                float y = getRelativePosFromCenter(ball.position, collision.transform.position, collision.collider.bounds.size.y);

                ball.velocity += new Vector3(0, y, 0).normalized;
                //StartCoroutine(OnHit());
            }
            if (collision.collider.CompareTag("Wall"))
            {
                ball.velocity = new Vector3(ball.velocity.x, -ball.velocity.y); //if it hits the ceiling or floor, reverse y. 
                StartCoroutine(OnHit());
            }
            
        }

        if (collision.collider.CompareTag("Score"))
        {
            /*
            ball.velocity = new Vector3(-ball.velocity.x, ball.velocity.y); //if it hits vertical walls, reverse x (for now)
            StartCoroutine(OnHit());
            */
            if (ball.position.x < collision.transform.position.x) //check if ball is to left of wall
            {
                _gc.increaseScore(this.gameObject, false);
            }
            if (ball.position.x > collision.transform.position.x)
            {
                _gc.increaseScore(this.gameObject, true);
            }

        }


    }
    IEnumerator OnHit()
    {
        isActive = false;
        yield return new WaitForSeconds(1f);
        isActive = true; 

    }   
    

    private float getRelativePosFromCenter(Vector3 ballPosition, Vector3 paddlePosition, float racketLength)
    {
        
            return (ballPosition.y - paddlePosition.y) * racketLength;

    }
    

}
