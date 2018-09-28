using UnityEngine;

public class TimeUpdater : MonoBehaviour {

    public UnityEngine.UI.Text TimeLimit;
    private float time=60f;
    int checkpoint = 5;
    public GameManager gameManager;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {

        TimeLimit.text = "Time Limit : " + (int) time;

		if(time <= 0)
        {
            gameManager.play = false;
        }
	}

    private void FixedUpdate()
    {

        if (gameManager.play) time -= Time.deltaTime;
    }

    private void increaseTime()
    {
        if(gameManager.level > checkpoint)
        {
            time += 10f;
            checkpoint += 5;
        }
    }
}
