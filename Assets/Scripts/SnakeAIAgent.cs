using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAIAgent : Agent {
	// Action 0 = left
	// Action 1 = right
	// Action 2 = up
	// Action 3 = down
	public GameObject head;
	public GameObject tailPrefab;
	public float moveSpeed = 0.2f;
	private float score = 0.0f;
	int faceAngle = 90;
	bool ate = false;

	List<Transform> tail = new List<Transform>();
	Vector3 snakeResetPos = new Vector3(0, 0, 0);
	Vector2 dir = Vector2.right;

	int layerMask_onlyWalls = 1 << 9; // Exclude all but walls
	int layerMask_onlyFood = 1 << 10;
	int layerMask_onlyBody = 1 << 11;

	string logFilePath = @"C:\Users\Ari\Desktop\SnakeAI\log.txt";


	public override void InitializeAgent(){
		base.InitializeAgent();
		InitializeLogFile();
		Output("Initialized File");
		Output("Tail - " + tail.ToString());
		InvokeRepeating ("Move", 0.2f, moveSpeed);
	}

	public override void CollectObservations(){
		// Distance to walls from head
		Output("__________");
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, 90f), Mathf.Infinity, layerMask_onlyWalls).distance); // Right
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, -90f), Mathf.Infinity, layerMask_onlyWalls).distance); // Left
		AddVectorObs (Physics2D.Raycast (transform.position, dir, Mathf.Infinity, layerMask_onlyWalls).distance); // Forward
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, 45f), Mathf.Infinity, layerMask_onlyWalls).distance); // Forward Right
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, -45f), Mathf.Infinity, layerMask_onlyWalls).distance); // Forward Left
		// Distance to food from head
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, 90f), Mathf.Infinity, layerMask_onlyFood).distance); // Right
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, -90f), Mathf.Infinity, layerMask_onlyFood).distance); // Left
		AddVectorObs (Physics2D.Raycast (transform.position, dir, Mathf.Infinity, layerMask_onlyFood).distance); // Forward
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, 45f), Mathf.Infinity, layerMask_onlyFood).distance); // Forward Right
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, 20f), Mathf.Infinity, layerMask_onlyFood).distance); // Forward Right Semi 
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, -20f), Mathf.Infinity, layerMask_onlyFood).distance); // Forward Left Semi
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, -45f), Mathf.Infinity, layerMask_onlyFood).distance); // Forward Left
		// Distance to body from head
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, 90f), Mathf.Infinity, layerMask_onlyBody).distance); // Right
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, -90f), Mathf.Infinity, layerMask_onlyBody).distance); // Left
		AddVectorObs (Physics2D.Raycast (transform.position, dir, Mathf.Infinity, layerMask_onlyBody).distance); // Forward
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, 45f), Mathf.Infinity, layerMask_onlyBody).distance); // Forward Right
		AddVectorObs (Physics2D.Raycast (transform.position, RotateDeg(dir, -45f), Mathf.Infinity, layerMask_onlyBody).distance); // Forward Left
		// Current score
		AddVectorObs (score);
		Output("Raycasts found and added");		
	}

	public override void AgentAction(float[] vectorAction, string textAction){
		Output("vectorAction - " + vectorAction.ToString());
		int action = Mathf.FloorToInt(vectorAction[0]);
		Output("Action - " + action.ToString());
		Output("textAction - " + textAction);
		if (action == 1) {GenNewDir("right");} 
		if (action == 3) {GenNewDir("down");}
		if (action == 0) {GenNewDir("left");} 
		if (action == 2) {GenNewDir("up");}
	}


	public override void AgentReset(){
		tail.Clear ();
		head.transform.position = snakeResetPos;
	}

	void InitializeLogFile(){
		System.IO.File.WriteAllText(logFilePath, System.DateTime.Now.ToString());
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (logFilePath, true)) {file.WriteLine("   ")}
	}

	void Output(string message){
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(logFilePath, true)){
            file.WriteLine("DEBUG LOG - " + message);
        }
	}

	public static Vector2 RotateDeg(Vector2 baseVector, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
        float tx = baseVector.x;
        float ty = baseVector.y;
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    void Move() {
		// Save current position (gap will be here)
		Vector2 v = transform.position;

		// Move head into new direction (now there is a gap)
		transform.Translate (dir);

		// Ate something? Then insert new Element into gap
		if (ate) {
			// Load Prefab into the world
			GameObject g = (GameObject)Instantiate (tailPrefab, v, Quaternion.identity);
			score += 1.0f;

			// Keep track of it in our tail list
			tail.Insert (0, g.transform);

			// Reset the flag
			ate = false;
		} else if (tail.Count > 0) {	// Do we have a Tail?
				// Move last Tail Element to where the Head was
			tail.Last().position = v;

				// Add to front of list, remove from the back
				tail.Insert (0, tail.Last ());
				tail.RemoveAt (tail.Count - 1);
		}
	}
	

	public void GenNewDir(string dirtogo){
		if (dir == Vector2.right){
			if (dirtogo == "up"){
				dir = Vector2.up;
				faceAngle = 0;
			} else if (dirtogo == "down"){
				dir = Vector2.down;
				faceAngle = 180;}

		} else if (dir == Vector2.left){
			if (dirtogo == "up"){
				dir = Vector2.up;
				faceAngle = 0;
			} else if (dirtogo == "down"){
				dir = Vector2.down;
				faceAngle = 180;}

		} else if (dir == Vector2.up){
			if (dirtogo == "left"){
				dir = Vector2.left;
				faceAngle = 270;
			} else if (dirtogo == "right"){
				dir = Vector2.right;
				faceAngle = 90;}

		} else if (dir == Vector2.down){
			if (dirtogo == "left"){
				dir = Vector2.left;
				faceAngle = 270;
			} else if (dirtogo == "right"){
				dir = Vector2.right;
				faceAngle = 90;}
		}
	}
}
