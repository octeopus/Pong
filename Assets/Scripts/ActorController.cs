﻿/*
 * Script for Paddles prefab. Affects Paddle behavior.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    
    private float speed = 10f;
    private bool AISwitch;
    private Rigidbody rb;
    [SerializeField]
    private List<GameObject> currentTargets;
    [SerializeField]
    private GameObject _target;


	// Use this for initialization
	void Start () {
        //initialize player object as not AI first.
        rb = GetComponentInChildren<Rigidbody>();
        currentTargets = new List<GameObject>();
        _target = null;

    }

    void FixedUpdate()
    {
        getCommand();
    }

    private void getCommand()
    {
        //take in inputs if player is not an AI.
        if (!AISwitch) { 

            float input = Input.GetAxisRaw("Vertical");
            //up is 1, down is -1
            rb.velocity = new Vector3(0, input, 0) * speed;
        }
        else
        {
            //consult AI if player is an AI.
            if (_target != null)
            {
                consultAI();
            }
        }
        
    }

    private void consultAI()
    {
        //use ball's position to move paddle
        //look for ball. 
        //if paddle's y < ball's y, input = 1 (move up)
        //if paddle's y > ball's y, input = -1 (move down)
        //add random chance to ignore movement as difficulty.

        

        if(rb.position.y < _target.transform.position.y)
        {
            StartCoroutine(MoveUp()); //move up
        }
        else if (rb.position.y > _target.transform.position.y)
        {
            StartCoroutine(MoveDown()); //move down
        }else
        {
            StartCoroutine(Stop()); // stop
        }
    }


    //=====================Movement Functions==================//

        IEnumerator MoveUp()
    {
        rb.velocity = new Vector3(0, 1) * speed;
        yield return new WaitForFixedUpdate();
        rb.velocity = new Vector3(0, 0) * speed;
    }
    
     IEnumerator MoveDown()
    {
        rb.velocity = new Vector3(0, -1) * speed;
        yield return new WaitForFixedUpdate();
        rb.velocity = new Vector3(0, 0) * speed;
    }

    IEnumerator Stop()
    {
        rb.velocity = new Vector3(0, 0) * speed;
        yield return new WaitForFixedUpdate();
    }



    //====================Collision Functions=================//

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && !currentTargets.Contains(other.gameObject))
        {
            currentTargets.Add(other.gameObject);
            if(currentTargets.Count == 1)
            {
                _target = currentTargets[0]; 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball") && currentTargets.Contains(other.gameObject))
        {
            currentTargets.Remove(other.gameObject);
            if(currentTargets.Count == 0)
            {
                _target = null;
            }
        }
    } 



    //==================AI Functions=================//

    public void setAI(bool AI)
    {
        AISwitch = AI;
    }

    public void removeTarget(GameObject obj)
    {
        if(currentTargets.Count > 0)
        {
            currentTargets.Remove(obj);
        }

    }

}
