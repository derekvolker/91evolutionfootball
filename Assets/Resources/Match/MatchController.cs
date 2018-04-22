using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MatchController : MonoBehaviour {


	GameObject floating_message;
	GameObject score;
	GameObject game_time;
	GameObject team1_ai_controller;
	GameObject team2_ai_controller;
	GameObject ball;
	Camera main_camera;


	public float display_time;
	public bool message_shown;
	public int team_1_score = 0;
	public int team_2_score = 0;
	public bool goal_took;
	public float wait_time;
	private int match_time;
	public bool in_sudden_death = false;
	public bool game_is_over = false;

	public void DisplayMessage(string message, float display_time){
		this.display_time = display_time+Time.time;
		message_shown = true;
		string path = "";
		switch (message) {
		case "begin":
			path = "Messages/message_begin";
			break;
		case "defeat":
			path = "Messages/message_defeat";
			break;
		case "goal":
			path = "Messages/message_goal";
			break;
		case "resume":
			path = "Messages/message_resume";
			break;
		case "sudden_death":
			path = "Messages/message_sudden_death";
			break;
		case "victory":
			path = "Messages/message_victory";
			break;
		}
		floating_message.GetComponent<Image>().sprite = Resources.Load <Sprite>(path);
		floating_message.GetComponent<Image>().SetNativeSize();
		Color color = floating_message.GetComponent<Image>().color;
		color.a = 1;
		floating_message.GetComponent<Image>().color = color;
	}

	public void EraseMessage(){
		if (display_time < Time.time){
			Color color = floating_message.GetComponent<Image>().color;
			if (color.a > 0){
				color.a -= 0.05f;
				floating_message.GetComponent<Image>().color = color;
			} else {
				message_shown = false;
			}

		}
	}

	void ResetTeamPositions(){
		
	}

	public void Goal(int team_goal_identifier){
		if (team_goal_identifier == 1){
			team_2_score += 1;
		} else {
			team_1_score += 1;
		}
		if (in_sudden_death){
			UpdateScore();
			EndGame();
		} else {
			DisplayMessage("goal", 1);
			team1_ai_controller.SendMessage("FreezeTeam");
			team2_ai_controller.SendMessage("FreezeTeam");
			goal_took = true;
			wait_time = Time.time + 2;
			UpdateScore();
		}
	}

	public void ResetBallPosition(){
		ball.transform.position = new Vector3(0,-0.36f,-0.36f);
		Vector3 new_camera_position = main_camera.transform.position;
		//Not messing up with the Z that will bug it all
		new_camera_position.x = 0;
		new_camera_position.y = -0.36f;
		main_camera.transform.position = new_camera_position;
	}

	public void HandleAfterGoal(){
		if (wait_time < Time.time){
			goal_took = false;
			team1_ai_controller.SendMessage("ResetTeamPosition");
			team2_ai_controller.SendMessage("ResetTeamPosition");
			ResetBallPosition();
			team1_ai_controller.SendMessage("UnfreezeTeam");
			team2_ai_controller.SendMessage("UnfreezeTeam");
			DisplayMessage("resume", 1);
		}
	}

	public void UpdateScore(){
		score.GetComponent<Text>().text = "TEAM 1 - "+team_1_score+"\nTEAM 2 - "+team_2_score;
	}

	public void UpdateGameTime(){
		float current_time = match_time - Time.time;
		float minutes = Mathf.Floor(current_time/60);
		float seconds = Mathf.Floor(current_time%60);
		if (seconds == 60){
			seconds = 0;
		}
		if (minutes == 0 && seconds == 0){
			EndGame();
		} else {
			game_time.GetComponent<Text>().text = minutes.ToString("00")+":"+seconds.ToString("00");	
		}

	}

	public void EndGame(){
		if (team_1_score == team_2_score){
			DisplayMessage("sudden_death", 1);
			in_sudden_death = true;
		} else {
			game_is_over = true;
			team1_ai_controller.SendMessage("FreezeTeam");
			team2_ai_controller.SendMessage("FreezeTeam");
			if (team_1_score > team_2_score){
				DisplayMessage("victory", 1);
			} else {
				DisplayMessage("defeat", 1);
			}
			wait_time = Time.time+2;
		}
	}

	void Start () {
		floating_message = GameObject.FindGameObjectWithTag("floating_message");
		score = GameObject.FindGameObjectWithTag("score");
		game_time = GameObject.FindGameObjectWithTag("game_time");
		team1_ai_controller = GameObject.Find("Team1AI");
		team2_ai_controller = GameObject.Find("Team2AI");
		DisplayMessage("begin", 1);
		main_camera = Camera.main;
		ball = GameObject.FindGameObjectWithTag("ball");
		team_1_score = 0;
		team_2_score = 0;
		match_time = 120;
	}

	void Update () {
		if (game_is_over == false){
			if (message_shown){
				EraseMessage();
			}
			if (goal_took){
				HandleAfterGoal();
			}
			if (in_sudden_death == false){
				UpdateGameTime();	
			}			
		} else {
			if (wait_time < Time.time){
				Application.LoadLevel("Title");
			}

		}
	}
}
