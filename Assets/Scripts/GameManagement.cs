using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour {

    private Canvas titleScreen;
    private Canvas selectDifficulty;

    private static int difficulty; 

	// Use this for initialization
	void Awake () {

        //initialize UI elements
        titleScreen = transform.Find("TitleScreenCanvas").GetComponent<Canvas>();
        selectDifficulty = transform.Find("SelectDifficultyCanvas").GetComponent<Canvas>();
        
        initUI();

        //initialize values
        DontDestroyOnLoad(this);


	}

    //============Initialization=============//

    private void initUI()
    {
        titleScreen.enabled = true;
        selectDifficulty.enabled = false;
    }

    //============Accessor===========//

    public int getDifficulty()
    {
        return difficulty;
    }
    

    //============Canvas Events=============//
	
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ArcadeStart()
    {
        selectDifficulty.enabled = true;
        titleScreen.enabled = false;
    }

    public void BackToMain()
    {
        titleScreen.enabled = true;
        selectDifficulty.enabled = false;
    }

    public void EasyModeArcade()
    {
        difficulty = 0;
        SceneManager.LoadScene("PongStage");
        DisableCanvases();
    }
    
    public void NormalModeArcade()
    {
        difficulty = 1;
        SceneManager.LoadScene("PongStage");
        DisableCanvases();
    }

    public void HardModeArcade()
    {
        difficulty = 2;
        SceneManager.LoadScene("PongStage");
        DisableCanvases();
    }

    public void DisableCanvases()
    {
        titleScreen.enabled = false;
        selectDifficulty.enabled = false;
    }
}
