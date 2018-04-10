using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Snake : MonoBehaviour {
	bool ate = false;
	bool isDied = false;
	public GameObject tailPrefab;
	Vector2 dir = Vector2.right;
	int angle = 90;
	List<Transform> tail = new List<Transform>();

	public Text ForwardText;
	public Text LeftText;
	public Text RightText;

	void Start () {
		// Move the Snake every 300ms
		InvokeRepeating("Move", 0.3f, 0.3f); 
	}

	void Update () {
		if (!isDied) {
			// Move in a new Direction?
			if (Input.GetKey (KeyCode.RightArrow)) {
				genNewDir("right");
				angle += 90;
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				genNewDir("down");    // '-up' means 'down'
				angle = 180;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				genNewDir("left"); // '-right' means 'left'
				angle -= 90;
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				genNewDir("up");
			} else {
				if (Input.GetKey (KeyCode.R)) {
					//clear the tail
					tail.Clear ();

					//reset to origin
					transform.position = new Vector3 (0, 0, 0);

					//make snake alive
					isDied = false;
				}
			}
			genRaycasts ();
		}
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

	public Vector2 Vector2FromAngle(float a){
		a *= Mathf.Deg2Rad;
		return new Vector2 (Mathf.Cos (a), Mathf.Sin (a));
	}

	void genRaycasts(){
		int LayerMask = 1 << 9; // Only cast against wall layer
		RaycastHit2D wallForward = Physics2D.Raycast (transform.position, dir, Mathf.Infinity, LayerMask);
		RaycastHit2D wallRight = Physics2D.Raycast (transform.position, dir.Rotate(-90f), Mathf.Infinity, LayerMask);
		RaycastHit2D wallLeft = Physics2D.Raycast (transform.position, dir.Rotate(90f), Mathf.Infinity, LayerMask);

		ForwardText.text = "wallForward:" + wallForward.distance.ToString ();
		LeftText.text = "wallLeft   :" + wallLeft.distance.ToString ();
		RightText.text = "wallRight  :" + wallRight.distance.ToString ();
	}


	void Move() {
		if (!isDied) {
			// Save current position (gap will be here)
			Vector2 v = transform.position;

			// Move head into new direction (now there is a gap)
			transform.Translate (dir);

			// Ate something? Then insert new Element into gap
			if (ate) {
				// Load Prefab into the world
				GameObject g = (GameObject)Instantiate (tailPrefab,
					              v,
					              Quaternion.identity);

				// Keep track of it in our tail list
				tail.Insert (0, g.transform);

				// Reset the flag
				ate = false;
			} else if (tail.Count > 0) {	// Do we have a Tail?
					// Move last Tail Element to where the Head was
					tail.Last ().position = v;

					// Add to front of list, remove from the back
					tail.Insert (0, tail.Last ());
					tail.RemoveAt (tail.Count - 1);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		// Food?
		if (coll.name.StartsWith("Food")) {
			// Get longer in next Move call
			ate = true;

			// Remove the Food
			Destroy(coll.gameObject);
		} else { 	// Collided with Tail or Border
			isDied = true;
		}
	}
}
 
public static class Vector2Extension {
     
    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
        float tx = v.x;
        float ty = v.y;
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}