using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAIAgent : Agent {
	// Action 0 = turn left
	// Action 1 = turn right
	// Action 2 = go striaght
	public GameObject head;
	public GameObject tailPrefab;
	public float moveSpeed = 0.2f;


	List<Transform> tail = new List<Transform>();
	Vector3 snakeResetPos = new Vector3(0, 0, 0);
	Vector2 dir = Vector2.right;
	int faceAngle = 90;

	// Layermask to exclude all but walls
	int layerMask = 1 << 9;


	void Start () {
		InvokeRepeating("Move", moveSpeed, moveSpeed); 
	}

	public Vector2 Vector2FromAngle(float a){
		a *= Mathf.Deg2Rad;
		return new Vector2 (Mathf.Cos (a), Mathf.Sin (a));
	}

	public void Update(){
		//Debug.Log (Physics2D.Raycast (transform.position, Vector2FromAngle (faceAngle), Mathf.Infinity, layerMask).distance);
	}

	public override void CollectObservations(){
		// Distance to walls from head
		AddVectorObs (Physics2D.Raycast (transform.position, Vector2FromAngle (faceAngle + 90), Mathf.Infinity, layerMask).distance); // Right
		AddVectorObs (Physics2D.Raycast (transform.position, Vector2FromAngle (faceAngle - 90), Mathf.Infinity, layerMask).distance); // Left
		AddVectorObs (Physics2D.Raycast (transform.position, Vector2FromAngle (faceAngle), Mathf.Infinity, layerMask).distance); // Forward
		AddVectorObs (Physics2D.Raycast (transform.position, Vector2FromAngle (faceAngle + 45), Mathf.Infinity, layerMask).distance); // Forward Right
		AddVectorObs (Physics2D.Raycast (transform.position, Vector2FromAngle (faceAngle - 45), Mathf.Infinity, layerMask).distance); // Forward Left
	}

	public override void AgentStep(){
		if (Input.GetKey (KeyCode.RightArrow)) {
				genNewDir("right");
				faceAngle += 90;
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				genNewDir("down");    // '-up' means 'down'
				faceAngle = 180;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				genNewDir("left"); // '-right' means 'left'
				faceAngle -= 90;
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				genNewDir("up");
				faceAngle = 0;
		}
	}


	public override void AgentReset(){
		tail.Clear ();
		head.transform.position = snakeResetPos;
	}

	public void genNewDir(string dirtogo){
		if (dir == Vector2.right){
			if (dirtogo == "up"){
				dir = Vector2.up;
			} else if (dirtogo == "down"){
				dir = Vector2.down;}

		} else if (dir == Vector2.left){
			if (dirtogo == "up"){
				dir = Vector2.up;
			} else if (dirtogo == "down"){
				dir = Vector2.down;}

		} else if (dir == Vector2.up){
			if (dirtogo == "left"){
				dir = Vector2.left;
			} else if (dirtogo == "right"){
				dir = Vector2.right;}

		} else if (dir == Vector2.down){
			if (dirtogo == "left"){
				dir = Vector2.left;
			} else if (dirtogo == "right"){
				dir = Vector2.right;}
		}
	}
}