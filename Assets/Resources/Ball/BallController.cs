using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	private Camera main_camera;
	private Animator my_animator;
	public GameObject Team1AIController;
	public GameObject Team2AIController;
	private float movement_modifier = 0.02f;
	private float normal_speed = 0.02f;
	private float kick_speed = 0.04f;
	public bool is_with_player = false;
	public bool is_moving = false;

	public float current_direction_x;
	public float current_direction_y;

	//define limits
	private float top_limit = 2.12f;
	private float bottom_limit = -2.84f;
	private float left_limit = -7.8f;
	private float right_limit = 7.8f;

	void Passed(float[] parameters){
		//parameter 0 is x
		//parameter 1 is y
		current_direction_x = parameters[0];
		current_direction_y = parameters[1];
		movement_modifier = normal_speed;
		is_moving = true;
		is_with_player = false;
		my_animator.Play("moving_ball");
	}

	void Kicked(float[] parameters){
		//parameter 0 is x
		//parameter 1 is y
		current_direction_x = parameters[0];
		current_direction_y = parameters[1];
		movement_modifier = kick_speed;
		is_moving = true;
		is_with_player = false;
		my_animator.Play("moving_ball");
	}

	void Move(){
		Vector3 new_position = transform.position;
		Vector3 camera_new_position = main_camera.transform.position;
		if (current_direction_x == 1){
			new_position.x += movement_modifier;
			camera_new_position.x += movement_modifier;
			if (new_position.x > right_limit){
				new_position.x = right_limit;
				camera_new_position.x = right_limit;
				my_animator.Play("iddle_ball");
				is_moving = false;
			}
		}
		if (current_direction_x == -1){
			new_position.x -= movement_modifier;
			camera_new_position.x -= movement_modifier;
			if (new_position.x < left_limit){
				new_position.x = left_limit;
				camera_new_position.x = left_limit;
				my_animator.Play("iddle_ball");
				is_moving = false;
			}
		}
		if (current_direction_y == 1){
			new_position.y += movement_modifier;
			new_position.z += movement_modifier;
			camera_new_position.y += movement_modifier;
			if (new_position.y > top_limit){
				new_position.y = top_limit;
				camera_new_position.y = top_limit;
				my_animator.Play("iddle_ball");
				is_moving = false;
			}
		}
		if (current_direction_y == -1){
			new_position.y -= movement_modifier;
			new_position.z -= movement_modifier;
			camera_new_position.y -= movement_modifier;
			if (new_position.y < bottom_limit){
				new_position.y = bottom_limit;
				camera_new_position.y = bottom_limit;
				my_animator.Play("iddle_ball");
				is_moving = false;
			}
		}
		transform.position = new_position;
		main_camera.transform.position = camera_new_position;
	}

	void PlayerGotHold(string[] parameters){
		//parameter 1 is player team
		//parameter 2 is player identification
		is_moving = false;
		is_with_player = true;
		my_animator.Play("moving_ball");
		if (parameters[0] == "1"){
			Team1AIController.SendMessage("SetBallPossession");
			Team2AIController.SendMessage("UnsetBallPossession");
		} else {
			Team2AIController.SendMessage("SetBallPossession");
			Team1AIController.SendMessage("UnsetBallPossession");
		}
	}

	void Start () {
		main_camera = Camera.main;
		my_animator = GetComponent<Animator>();
		my_animator.Play("iddle_ball");

	}
	
	void Update () {
		if (is_moving){
			Move();
		}
	}
}
