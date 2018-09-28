using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour {

    private float resetDirection = 5f, resetSpeed = 5f;
    private float speedX, speedY;
    private int directionX, directionY;
    public float answer;

    public GameManager Script;
    public GameObject gameManagerObj;
    public Text FishText;
    // Use this for initialization
    void Start ()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        #region speed randomize
        speedX = Random.Range(0.5f, 5f);
        speedY = Random.Range(0.5f, 5f);
        #endregion
        #region Direction Randomize
        directionX = Random.Range(0, 2);
        directionY = Random.Range(0, 2);
        #endregion
        Script = gameManagerObj.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        resetSpeed -= Time.deltaTime;
        resetDirection -= Time.deltaTime;


        if (resetSpeed <= 0)
        {
            speedX = Random.Range(0f, 3f);
            speedY = Random.Range(0f, 3f);
            resetSpeed = Random.Range(0f,5f);
        }
        if (resetDirection <= 0)
        {
            directionX = Random.Range(0, 2);
            directionY = Random.Range(0, 2);
            resetDirection = Random.Range(0, 3);
        }
        fishMove();
        textMove();
        FishText.text = "" + answer;
        
    }

    void fishMove()
    {
        if(directionX == 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speedX, GetComponent<Rigidbody2D>().velocity.y);
            Vector2 tempscale = transform.localEulerAngles;
            tempscale.y = 0f;
            transform.localEulerAngles = tempscale;
            FishText.alignment = TextAnchor.MiddleLeft;
        }
        
        else if(directionX == 1) //move right
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, GetComponent<Rigidbody2D>().velocity.y);
            Vector2 tempscale = transform.localEulerAngles;
            tempscale.y = 180f;
            transform.localEulerAngles = tempscale;
            FishText.alignment = TextAnchor.MiddleRight;
        }

        if (directionY == 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, speedY);
        }
        else if (directionY == 1)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -speedY);
        }
    }

    void textMove()
    {
        if (directionX == 1)
        {            
            Vector2 tempscale = transform.localEulerAngles;
            tempscale.y = 180f;
            FishText.transform.localEulerAngles = tempscale;
        }

        else if (directionX == 0)
        {
            Vector2 tempscale = transform.localEulerAngles;
            tempscale.y = 0f;
            FishText.transform.localEulerAngles = tempscale;
        }

        if (directionY == 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, speedY);
        }
        else if (directionY == 1)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -speedY);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Hook" && answer == Script.ans && GameManager.Mode == 0)
        {
            Debug.Log("benar");
        }
        else if(col.collider.tag == "Hook" && answer != Script.ans && GameManager.Mode == 0)
        {
            Debug.Log("salah");
        }

        if(col.collider.tag == "Hook" && answer == Script.ans && GameManager.Mode == 1)
        {
            Script.level++;
            Script.newQuestion = true;
            Script.scoreValue += 500;
            Script.directionChosen = false;
            Script.calc = false;
            Script.Harpoon.transform.position = new Vector2(0, (float)2);
            Script.touchStartPos = new Vector2(0, 0);
            Script.touchEndPos = new Vector2(9999, 9999);
            Script.touchDestination = new Vector2(0, 0);
            Script.Harpoon.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        else if (col.collider.tag == "Hook" && answer != Script.ans && GameManager.Mode == 1)
        {
            Script.play = false;
        }
    }


}
