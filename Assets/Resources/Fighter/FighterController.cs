using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour {
	//Define a lot of parameters
	public bool can_move = true;
	public bool current_fighter = false;
	public bool in_cooldown = false;
	public bool with_ball = false;
	public bool moving = false;

	public string current_animation = "iddle";
	public string skin;
	public string team;

	private float life_max = 10;
	private float life_current = 10;
	private float damage_current = 5;
	private float damage_default = 1;
	private float movement_modifier = 0.02f;
	private float normal_speed = 0.02f;
	private float ball_speed = 0.01f;

	public float last_direction_x = 0;
	public float last_direction_y = 0;

	private float attack_cooldown = 0.5f;
	private float hit_cooldown = 1;
	private float fall_cooldown = 2;
	private float rise_cooldown = 1;
	private float kick_cooldown = 0.25f;
	public float current_cooldown = 0;

	//define limits
	private float top_limit = 2.12f;
	private float bottom_limit = -2.84f;
	private float left_limit = -7.8f;
	private float right_limit = 7.8f;

	public Animator my_animator;
	public GameObject selected_marker;
	public GameObject ball;
	public GameObject fighter_attack_base;
	public GameObject blood_base;
	public GameObject kick_base;
	Camera main_camera;


	//Define default keys
	//TODO: Get by configuration or something
	KeyCode primary_up = KeyCode.W;
	KeyCode secondary_up = KeyCode.UpArrow;
	KeyCode primary_down = KeyCode.S;
	KeyCode secondary_down = KeyCode.DownArrow;
	KeyCode primary_left = KeyCode.A;
	KeyCode secondary_left = KeyCode.LeftArrow;
	KeyCode primary_right = KeyCode.D;
	KeyCode secondary_right = KeyCode.RightArrow;
	KeyCode primary_pass = KeyCode.J;
	KeyCode secondary_pass = KeyCode.J;
	KeyCode primary_kick = KeyCode.K;
	KeyCode secondary_kick = KeyCode.K;
	KeyCode primary_attack = KeyCode.L;
	KeyCode secondary_attack = KeyCode.L;
	KeyCode primary_change = KeyCode.I;
	KeyCode secondary_change = KeyCode.I;

	//Only will be called if it is the current fighter
	//TODO: Think in a better way of doing this
	void HandleInput(){
		//Check for arrow movement
		if (Input.GetKey(primary_up) || Input.GetKey(secondary_up)){
			Move(new float[] {0, 1});
		}
		if (Input.GetKey(primary_down) || Input.GetKey(secondary_down)){
			Move(new float[] {0, -1});
		}
		if (Input.GetKey(primary_left) || Input.GetKey(secondary_left)){
			Move(new float[] {-1, 0});
		}
		if (Input.GetKey(primary_right) || Input.GetKey(secondary_right)){
			Move(new float[] {1, 0});
		}
		if (Input.GetKeyDown(primary_pass) || Input.GetKeyDown(secondary_pass)){
			Pass();
		}
		if (Input.GetKeyDown(primary_kick) || Input.GetKeyDown(secondary_kick)){
			Kick();
		}
		if (Input.GetKeyDown(primary_attack) || Input.GetKeyDown(secondary_attack)){
			Attack();
		}
		if (Input.GetKeyDown(primary_change) || Input.GetKeyDown(secondary_change)){
			Change();
		}
	}

	void ChangeAnimation(){
		switch (current_animation) {
		case "iddle":
			my_animator.Play("fighter_"+skin+"_iddle");
		break;
		case "run":
			my_animator.Play("fighter_"+skin+"_run");
		break;
		case "hit":
			my_animator.Play("fighter_"+skin+"_hit");
			break;
		case "attack":
			my_animator.Play("fighter_"+skin+"_attack");
		break;
		case "rise":
			my_animator.Play("fighter_"+skin+"_rise");
		break;
		case "fall":
			my_animator.Play("fighter_"+skin+"_fall");
		break;
		case "kick":
			my_animator.Play("fighter_"+skin+"_kick");
		break;
		}

	}

	void Move(float[] parameters){
		//parameter 0 is x, 1 is y
		if (can_move){
			moving = true;
			float x = parameters[0];
			float y = parameters[1];
			last_direction_x = x;
			last_direction_y = y;
			Vector3 new_position = transform.position;
			Vector3 ball_new_position = ball.transform.position;
			Vector3 camera_new_position = main_camera.transform.position;
			//TODO: Ugly code
			if (x == 1){
				new_position.x += movement_modifier;
				if (new_position.x > right_limit){
					new_position.x = right_limit;
				}
				//The distance calculation is a horrible workaround because the ball tends to go wild
				if (with_ball && Vector3.Distance(transform.position, ball.transform.position)<0.16f){
					ball_new_position.x += movement_modifier;
					camera_new_position.x += movement_modifier;
					if (ball_new_position.x > right_limit){
						ball_new_position.x = right_limit;
						camera_new_position.x = right_limit;
					};
				}
			}
			if (x == -1){
				new_position.x -= movement_modifier;
				if (new_position.x < left_limit){
					new_position.x = left_limit;
				}
				if (with_ball && Vector3.Distance(transform.position, ball.transform.position)<0.16f){
					ball_new_position.x -= movement_modifier;
					camera_new_position.x -= movement_modifier;
					if (ball_new_position.x < left_limit){
						ball_new_position.x = left_limit;
						camera_new_position.x = left_limit;
					};
				}
			}
			if (y == 1){
				new_position.y += movement_modifier;
				new_position.z += movement_modifier;
				if (new_position.y > top_limit){
					new_position.y = top_limit;
					new_position.z = top_limit;
				}
				if (with_ball && Vector3.Distance(transform.position, ball.transform.position)<0.16f){
					ball_new_position.y += movement_modifier;
					camera_new_position.y += movement_modifier;
					if (ball_new_position.y > top_limit){
						ball_new_position.y = top_limit;
						camera_new_position.y = top_limit;
					};
				}
			}
			if (y == -1){
				new_position.y -= movement_modifier;
				new_position.z -= movement_modifier;
				if (new_position.y < bottom_limit){
					new_position.y = bottom_limit;
					new_position.z = bottom_limit;
				}
				if (with_ball && Vector3.Distance(transform.position, ball.transform.position)<0.16f){
					ball_new_position.y -= movement_modifier;
					camera_new_position.y -= movement_modifier;
					if (ball_new_position.y < bottom_limit){
						ball_new_position.y = bottom_limit;
						camera_new_position.y = bottom_limit;
					};
				}
			}
			transform.position = new_position;
			if (with_ball){
				ball.transform.position = ball_new_position;
				main_camera.transform.position = camera_new_position;

			}
			if (x != 0){
				//If goes to left mirror
				//If goes to right unmirror
				Vector3 new_scale = transform.localScale;
				new_scale.x = x;
				transform.localScale = new_scale;
			}
			if (current_animation != "run"){
				current_animation = "run";
				ChangeAnimation();				
			}
		}
	}

	void Attack(){
		if (can_move){
			current_animation = "attack";
			ChangeAnimation();
			current_cooldown = attack_cooldown+Time.time;
			can_move = false;
			in_cooldown = true;
			//distortion to hit in front
			Vector3 attack_position = transform.position;
			if (last_direction_x == 1){
				attack_position.x += 0.08f;
			} else {
				attack_position.x -= 0.08f;
			}
			GameObject new_attack = Instantiate(fighter_attack_base, attack_position, transform.rotation) as GameObject;
			new_attack.SendMessage("SetDamageAmount", damage_current);
			if (team == "1"){
				new_attack.SendMessage("SetTargetTeam", "2");
			} else {
				new_attack.SendMessage("SetTargetTeam", "1");
			}

		}
	}

	void Kick(){
		if (can_move){
			current_animation = "kick";
			ChangeAnimation();			
			current_cooldown = kick_cooldown+Time.time;
			can_move = false;
			in_cooldown = true;

			GameObject target_goal = EnemyTeamGoal();
			with_ball = false;
			movement_modifier = normal_speed;
			float x_direction = 0;
			float y_direction = 0;
			float y_difference = target_goal.transform.position.y - transform.position.y;
			if (Mathf.Abs(y_difference) > 0.5f){
				if (y_difference > 0){
					y_direction = 1;
				} else {
					y_direction = -1;
				}
			}
			if (target_goal.transform.position.x > transform.position.x){
				x_direction = 1;
			} else {
				x_direction = -1;
			}

			Vector3 kick_position = transform.position;
			if (last_direction_x == 1){
				kick_position.x += 0.08f;
			} else {
				kick_position.x -= 0.08f;
			}
			GameObject new_kick = Instantiate(kick_base, kick_position, transform.rotation) as GameObject;
			new_kick.SendMessage("SetDirection", new float[] {x_direction,y_direction});
		}
	}

	void Change(){
		//TODO:Implement
	}

	private GameObject EnemyTeamGoal(){
		GameObject team_goal;
		if (team == "1"){
			team_goal = GameObject.FindGameObjectWithTag("team_2_goal");
		} else{
			team_goal = GameObject.FindGameObjectWithTag("team_1_goal");
		}
		return team_goal;
	}
		
	private GameObject ClosestTeamMember(){
		//TODO:Reimplement to actually return closest team member
		GameObject[] team_members = GameObject.FindGameObjectsWithTag("team_"+team+"_unselected");
		float shortest_distance = 100;//Workaround
		float distance = 0;
		GameObject closest_team_member = null;
		foreach (GameObject team_member in team_members){
			distance = Vector3.Distance(transform.position, team_member.transform.position);
			if (distance < shortest_distance){
				shortest_distance = distance;
				closest_team_member = team_member;
			}
		}
		return closest_team_member;
	}

	void Pass(){
		if (can_move && with_ball){
			GameObject target_team_member = ClosestTeamMember();
			with_ball = false;
			movement_modifier = normal_speed;
			float x_direction = 0;
			float y_direction = 0;
			float y_difference = target_team_member.transform.position.y - transform.position.y;
			if (Mathf.Abs(y_difference) > 0.5f){
				if (y_difference > 0){
					y_direction = 1;
				} else {
					y_direction = -1;
				}
			}
			if (target_team_member.transform.position.x > transform.position.x){
				x_direction = 1;
			} else {
				x_direction = -1;
			}

			ball.SendMessage("Passed", new float[] {x_direction,y_direction});
			target_team_member.SendMessage("TurnIntoCurrentFighter");
		}
	}

	void TurnIntoCurrentFighter(){
		GameObject current_fighter = GameObject.FindGameObjectWithTag("team_"+team+"_selected");
		current_fighter.SendMessage("UnsetSelectedFighter");
		transform.tag = "team_"+team+"_selected";
		//change da bool
		if (team == "1"){
			this.current_fighter = true;
		}

	}

	public void UnsetSelectedFighter(){
		transform.tag = "team_"+team+"_unselected";
		current_fighter = false;
	}



	void ReceiveDamage(float damage_amount){
		Vector3 blood_position = transform.position;
		if (last_direction_x == -1){
			blood_position.x += 0.08f;
		} else {
			blood_position.x -= 0.08f;
		}
		Instantiate(blood_base, blood_position, transform.rotation);
		life_current -= damage_amount;
		if (life_current <= 0){
			current_animation = "fall";
			current_cooldown = fall_cooldown+Time.time;			
		} else {
			current_animation = "hit";
			current_cooldown = hit_cooldown+Time.time;
		}
		ChangeAnimation();
		can_move = false;
		in_cooldown = true;
	}

	void ResetCooldown(){
		if (Time.time > current_cooldown){
			if (current_animation == "fall"){
				current_animation = "rise";
				ChangeAnimation();
				life_current = life_max;
				current_cooldown = rise_cooldown+Time.time;				
			} else {
				can_move = true;
				in_cooldown = false;
				current_animation = "iddle";
				ChangeAnimation();
			}
		}
	}

	void UnsetBallPossession(){
		with_ball = false;
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "ball"){
			with_ball = true;
			movement_modifier = ball_speed;
			ball.SendMessage("PlayerGotHold", new string[] {team, transform.name});
			TurnIntoCurrentFighter();
		}
	}

	public void Freeze(){
		can_move = false;
	}

	public void Unfreeze(){
		can_move = true;
	}

	void Start () {
		my_animator = transform.GetComponent<Animator> ();
		main_camera = Camera.main;
		selected_marker = GameObject.FindGameObjectWithTag("selected_marker");
		ball = GameObject.FindGameObjectWithTag("ball");
		ChangeAnimation();
	}

	void Update () {
		moving = false;
		if (in_cooldown){
			ResetCooldown();	
		}
		if (current_fighter){
			HandleInput();
		}
		if (current_fighter){
			//Selected marker Z must be behind the fighter
			selected_marker.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+1);
		}
		if (current_animation == "run" && moving == false){
			current_animation = "iddle";
			ChangeAnimation();
		}
	}


}
