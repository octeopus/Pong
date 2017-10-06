/*
 * Script for Paddles prefab. Affects Paddle behavior.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    
    private float speed = 10f;
    private bool AISwitch;

	// Use this for initialization
	void Start () {
        //initialize player object as not AI first.
        
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
            GetComponentInChildren<Rigidbody>().velocity = new Vector3(0, input, 0) * speed;
        }
        else
        {
            //consult AI if player is an AI.
        }
        
    }

    public void setAI(bool AI)
    {
        AISwitch = AI;
    }

}
