using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAIAgent : Agent {

	public GameObject head;

	private Vector3 snakeStartPos;

	public override List<float> CollectState(){
		List<float> state = new List<float> ();
		return state;
	}

	public override void AgentStep(float[] action){
		
	}

	public override void AgentReset(){
		head.transform.position = snakeStartPos;
	}
}

