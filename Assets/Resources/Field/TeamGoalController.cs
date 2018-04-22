using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamGoalController : MonoBehaviour {

	public int team_goal_identifier;
	GameObject match_controller;

	void Start(){
		match_controller = GameObject.FindGameObjectWithTag("match_controller");
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "ball"){
			match_controller.SendMessage("Goal", team_goal_identifier);
			Debug.Log("Team "+team_goal_identifier+" took a Goal");
		}
	}

}
