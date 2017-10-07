/*
 * Script for the game. Oversees the operation of a game through a GameManager gameobject. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    //spawn nodes
    private GameObject nodeA;
    private GameObject nodeB;
    private GameObject ballNode;
    //canvas elements
    private Canvas UI;
    private Text PlayerAScore_text;
    private Text PlayerBScore_text;
    private Text Winner_text;
    //player scores
    private int PlayerAScore;
    private int PlayerBScore;
    //walls
    private GameObject walls;
    private Collider wallA;
    private Collider wallB;

    private List<ActorController> _players;
    


    public GameObject paddle; //declared in-editor.
    public GameObject ball; 

	// Use this for initialization
	void Start () {

        nodeA = transform.Find("NodeA").gameObject;
        nodeB = transform.Find("NodeB").gameObject;
        ballNode = transform.Find("BallNode").gameObject;
        UI = transform.Find("Canvas").GetComponent<Canvas>();
        PlayerAScore_text = UI.transform.Find("PlayerOneScore").GetComponent<Text>();
        PlayerBScore_text = UI.transform.Find("PlayerTwoScore").GetComponent<Text>();
        Winner_text = UI.transform.Find("WinnerText").GetComponent<Text>();

        //initialize walls
        walls = transform.Find("Walls").gameObject;
        wallA = walls.transform.Find("PlayerAWall").GetComponent<Collider>();
        wallB = walls.transform.Find("PlayerBWall").GetComponent<Collider>();
        

        //Initialize Player Scores
        PlayerAScore = 0;
        PlayerBScore = 0;

        //Initialize Player Text
        updateScore();
        Winner_text.text = "Ready...";

        //Initialize Players
        _players = new List<ActorController>();

        StartCoroutine(MatchReady());

    }

    IEnumerator MatchReady()
    {
        //Instantiate Paddles at Node positions.
        _players.Add(spawnPlayer(false, nodeA));
        _players.Add(spawnPlayer(true, nodeB));

        yield return new WaitForSeconds(3f);

        Winner_text.text = ""; //set Winner_text to empty once match has started.
        //Instantiate Ball at Node Position
        spawnBall(ballNode);
    }
	

    
    //======================Spawn Handling=====================//

    private ActorController spawnPlayer(bool isAI, GameObject side)
    {
        GameObject p = Instantiate(paddle, side.transform.position, Quaternion.identity);
        ActorController pScript = p.GetComponent<ActorController>();
        pScript.setAI(isAI);

        return pScript;
    }

    private void spawnBall(GameObject node)
    {
        GameObject p = Instantiate(ball, node.transform.position, Quaternion.identity);
        BallScript bScript = p.GetComponent<BallScript>();
        bScript.init(this);
        
        
    }

    //======================Score Handling====================//

    public void increaseScore(GameObject obj, bool isLeft)
    {
        //if True, player on Left scores. Else, player on Right scores.
        if (isLeft)
        {
            PlayerBScore++;
        }else
        {
            PlayerAScore++;
        }
        foreach(ActorController player in _players)
        {
            player.removeTarget(obj);
        }
        Destroy(obj);
        updateScore();

        //resolve game here
        if(PlayerAScore == 5)
        {
            StartCoroutine(EndGameProcessing("Player One"));
        }else if(PlayerBScore == 5)
        {
            StartCoroutine(EndGameProcessing("Player Two"));
        }
        else
        {
            StartCoroutine(WaitForBallRespawn());
        }
        
    }

    private void updateScore()
    {
        PlayerAScore_text.text = PlayerAScore.ToString();
        PlayerBScore_text.text = PlayerBScore.ToString();
    }

    IEnumerator WaitForBallRespawn()
    {
        yield return new WaitForSeconds(3f);
        spawnBall(ballNode);

    }

    IEnumerator EndGameProcessing(string winner)
    {
        Winner_text.text = winner + " wins!";
        yield return new WaitForSeconds(3f);
        //go back to title screen... if there was one.
    }


}
