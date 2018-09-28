using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour {

    public GameObject QuestionPanel;
    public GameObject QuestionButton;
    public GameManager gameManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Order(bool Click)
    {
        gameManager.touchDestination = new Vector2(9999, 9999);
        gameManager.directionChosen = false;
        if(Click == true)
        {
            gameManager.canControl = true;
            QuestionPanel.SetActive(false);
            QuestionButton.SetActive(true);
            gameManager.Sprite.SetActive(true);
            gameManager.Water.SetActive(true);
        }
        else
        {
            gameManager.Sprite.SetActive(false);
            QuestionButton.SetActive(false);
            QuestionPanel.SetActive(true);
            gameManager.Water.SetActive(false);
            gameManager.canControl = false;
        }
    }
}
