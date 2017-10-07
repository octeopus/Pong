/*
 * Script for Ball Prefabs in game. Affects Ball Behavior
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    private float _speed = 10f;
    private float _accel = 0f;

    [SerializeField]
    private Rigidbody _ball;
    private bool _isActive; //isActive is used to see if the ball can reflect off something.
    private GameController _gc;

	// Use this for initialization

    public void init(GameController gc)
    {
        _gc = gc;
        _ball = GetComponent<Rigidbody>();
        int coin = Random.Range(0, 2);
        
        if(coin == 0) { _ball.velocity = Vector3.right * (_speed); }
        else { _ball.velocity = Vector3.left * (_speed); }
        
        _isActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (_isActive) {

            if (collision.collider.CompareTag("Paddle"))
            {
                float y = getRelativePosFromCenter(_ball.position, collision.transform.position, collision.collider.bounds.size.y);

                _ball.velocity += new Vector3(0, y, 0).normalized;
                StartCoroutine(OnHit());
            }
            if (collision.collider.CompareTag("Wall"))
            {
                _ball.velocity = new Vector3(_ball.velocity.x, -_ball.velocity.y); //if it hits the ceiling or floor, reverse y. 
                StartCoroutine(OnHit());
            }
            Accelerate();
        }

        if (collision.collider.CompareTag("Score"))
        {
            /*
            ball.velocity = new Vector3(-ball.velocity.x, ball.velocity.y); //if it hits vertical walls, reverse x (for now)
            StartCoroutine(OnHit());
            */
            if (_ball.position.x < collision.transform.position.x) //check if ball is to left of wall
            {
                _gc.increaseScore(this.gameObject, false);
            }
            if (_ball.position.x > collision.transform.position.x)
            {
                _gc.increaseScore(this.gameObject, true);
            }

            ResetAccel();
        }


    }
    IEnumerator OnHit()
    {
        _isActive = false;
        yield return new WaitForSeconds(0.5f);
        _isActive = true; 

    }   
    


    //=================Acceleration===================//

    private void Accelerate()
    {
        //increases velocity of ball by accel. 
        _accel += 0.02f;

        float cX = _ball.velocity.x < 0 ? _ball.velocity.x - _accel : _ball.velocity.x + _accel;

        //regulate speed
        if (cX > 0 && cX > 15f) cX = 15f;
        if (cX < 0 && cX < -15f) cX = -15f;


        float cY = 0f;

        if(_ball.velocity.y == 0.0)
        {
            cY = 0f;
        }else if (_ball.velocity.y < 0.0)
        {
            cY = _ball.velocity.y - _accel;
            if (cY < -15f) cY = -15f;
        }else if (_ball.velocity.y > 0.0)
        {
            cY = _ball.velocity.y + _accel;
            if (cY > 15f) cY = 15f;
        }
        

        Debug.Log(cX);

        _ball.velocity = new Vector3(cX, cY);
    }

    private void ResetAccel()
    {
        _accel = 0f;

    }
    

    private float getRelativePosFromCenter(Vector3 ballPosition, Vector3 paddlePosition, float racketLength)
    {
        
            return (ballPosition.y - paddlePosition.y) * racketLength;

    }
    

}
