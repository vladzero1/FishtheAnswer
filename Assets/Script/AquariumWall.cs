using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumWall : MonoBehaviour {
    public GameObject ScriptObject;
    public GameManager Script;
    // Use this for initialization
    void Start () {
        Script = ScriptObject.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Hook")
        {
            Script.directionChosen = false;
            Script.calc = false;
            Script.Harpoon.transform.position = new Vector2(0, (float)2);
            Script.touchStartPos = new Vector2(0, 0);
            Script.touchEndPos = new Vector2(9999, 9999);
            Script.touchDestination = new Vector2(0, 0);
            Script.Harpoon.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }
}
