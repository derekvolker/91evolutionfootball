using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team2AIController : MonoBehaviour {

	public GameObject FighterA, FighterB, FighterC, FighterD, FighterE, ball, team_1_goal;
	//middlefront, middletop, middlebottom, backtop, backbottom
	float fighter_a_limit_top = 0.46f;
	float fighter_a_limit_right = 3.9f;
	float fighter_a_limit_bottom = -1.18f;
	float fighter_a_limit_left = 0;

	float fighter_b_limit_top = 2.12f;
	float fighter_b_limit_right = 3.9f;
	float fighter_b_limit_bottom = 0.46f;
	float fighter_b_limit_left = 0;

	float fighter_c_limit_top = -1.18f;
	float fighter_c_limit_right = 3.9f;
	float fighter_c_limit_bottom = -2.84f;
	float fighter_c_limit_left = 0;

	float fighter_d_limit_top = 2.12f;
	float fighter_d_limit_right = 7.8f;
	float fighter_d_limit_bottom = -0.36f;
	float fighter_d_limit_left = 3.9f;

	float fighter_e_limit_top = -0.36f;
	float fighter_e_limit_right = 7.8f;
	float fighter_e_limit_bottom = -2.84f;
	float fighter_e_limit_left = 3.9f;

	//define limits
	private float field_top_limit = 2.12f;
	private float field_bottom_limit = -2.84f;
	private float field_left_limit = -7.8f;
	private float field_right_limit = 7.8f;

	public bool ball_possession = false;

	public void SetBallPossession(){
		ball_possession = true;
	}
	public void UnsetBallPossession(){
		ball_possession = false;
		//Horrible, but didn't have time to do something better
		FighterA.SendMessage("UnsetBallPossession");
		FighterB.SendMessage("UnsetBallPossession");
		FighterC.SendMessage("UnsetBallPossession");
		FighterD.SendMessage("UnsetBallPossession");
		FighterE.SendMessage("UnsetBallPossession");
	}

	public void ChaseObjective(GameObject Fighter, float top_limit, float right_limit, float bottom_limit, float left_limit, GameObject objective){
		//1 in 5 chance to kick ball if in range
		int random_chance_of_stuff = Random.Range(1,5);
		if (random_chance_of_stuff == 1){
			//Kicks
			if (Vector3.Distance(Fighter.transform.position, ball.transform.position) <= 0.6f){
				Fighter.SendMessage("Kick");

			}
		}
		//1 in 10 chance to attack if in range
		random_chance_of_stuff = Random.Range(1,10);

		if (random_chance_of_stuff == 1){
			GameObject[] enemy_fighters = GameObject.FindGameObjectsWithTag("team_1_unselected");
			GameObject enemy_selected = GameObject.FindGameObjectWithTag("team_1_selected");
			float shortest_distance = Vector3.Distance(Fighter.transform.position, enemy_selected.transform.position);
			foreach(GameObject enemy_fighter in enemy_fighters){
				float distance = Vector3.Distance(Fighter.transform.position, enemy_fighter.transform.position);
				if (distance < shortest_distance){
					shortest_distance = distance;
				}
			}
			if (shortest_distance <= 0.6f){
				Fighter.SendMessage("Attack");
			}
		}
		Vector3 fighter_a_new_position = Fighter.transform.position;
		float x_movement = 0;
		float y_movement = 0;
		if (objective.transform.position.x > Fighter.transform.position.x){
			//should go right
			if (Fighter.transform.position.x < right_limit){
				//under limits
				x_movement = 1;
			}
		} else {
			//should go left
			if (Fighter.transform.position.x > left_limit){
				//under limits
				x_movement = -1;
			}
		}
		if (objective.transform.position.y > Fighter.transform.position.y){
			//should go up
			if (Fighter.transform.position.y < top_limit){
				//under limits
				y_movement = 1;
			}
		} else {
			//should go down
			if (Fighter.transform.position.y > bottom_limit){
				//under limits
				y_movement = -1;
			}
		}
		if (x_movement != 0 || y_movement != 0){
			Fighter.SendMessage("Move", new float[] {x_movement, y_movement});
		}
	}

	public void OffensiveStrategy(){
		//Should Chase the ball between the limits
		ChaseObjective(FighterA, fighter_a_limit_top, fighter_a_limit_right, fighter_a_limit_bottom, field_left_limit, team_1_goal);
		ChaseObjective(FighterB, fighter_b_limit_top, fighter_b_limit_right, fighter_b_limit_bottom, field_left_limit, team_1_goal);
		ChaseObjective(FighterC, fighter_c_limit_top, fighter_c_limit_right, fighter_c_limit_bottom, field_left_limit, team_1_goal);
		ChaseObjective(FighterD, fighter_d_limit_top, fighter_d_limit_right, fighter_d_limit_bottom, fighter_d_limit_left, team_1_goal);
		ChaseObjective(FighterE, fighter_e_limit_top, fighter_e_limit_right, fighter_e_limit_bottom, fighter_e_limit_left, team_1_goal);
	}

	public void DefensiveStrategy(){
		//Should Chase the ball between the limits
		ChaseObjective(FighterA, fighter_a_limit_top, fighter_a_limit_right, fighter_a_limit_bottom, field_left_limit, ball);
		ChaseObjective(FighterB, fighter_b_limit_top, fighter_b_limit_right, fighter_b_limit_bottom, field_left_limit, ball);
		ChaseObjective(FighterC, fighter_c_limit_top, fighter_c_limit_right, fighter_c_limit_bottom, field_left_limit, ball);
		ChaseObjective(FighterD, fighter_d_limit_top, fighter_d_limit_right, fighter_d_limit_bottom, fighter_d_limit_left, ball);
		ChaseObjective(FighterE, fighter_e_limit_top, fighter_e_limit_right, fighter_e_limit_bottom, fighter_e_limit_left, ball);
	}

	public void FreezeTeam(){
		FighterA.SendMessage("Freeze");
		FighterB.SendMessage("Freeze");
		FighterC.SendMessage("Freeze");
		FighterD.SendMessage("Freeze");
		FighterE.SendMessage("Freeze");
	}

	public void UnfreezeTeam(){
		FighterA.SendMessage("Unfreeze");
		FighterB.SendMessage("Unfreeze");
		FighterC.SendMessage("Unfreeze");
		FighterD.SendMessage("Unfreeze");
		FighterE.SendMessage("Unfreeze");
	}

	public void ResetTeamPosition(){
		FighterA.transform.position = new Vector3(1.6f,-0.36f,-0.36f);
		FighterB.transform.position = new Vector3(1.36f,1.13f,1.13f);
		FighterC.transform.position = new Vector3(1.35f,-2.03f,-2.03f);
		FighterD.transform.position = new Vector3(4.61f,0.59f,0.59f);
		FighterE.transform.position = new Vector3(4.61f,-1.31f,-1.31f);
	}

	void Start () {
		FighterA = GameObject.Find("Fighter T2 A");
		FighterB = GameObject.Find("Fighter T2 B");
		FighterC = GameObject.Find("Fighter T2 C");
		FighterD = GameObject.Find("Fighter T2 D");
		FighterE = GameObject.Find("Fighter T2 E");
		ball = GameObject.FindGameObjectWithTag("ball");
		team_1_goal = GameObject.FindGameObjectWithTag("team_1_goal");
	}

	void Update () {
		if (ball_possession){
			OffensiveStrategy();
		} else {
			DefensiveStrategy();
		}
	}
}
