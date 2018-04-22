using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

	//Define default keys
	//TODO: Get by configuration or something
	KeyCode primary_up = KeyCode.W;
	KeyCode secondary_up = KeyCode.UpArrow;
	KeyCode primary_down = KeyCode.S;
	KeyCode secondary_down = KeyCode.DownArrow;
	KeyCode primary_enter = KeyCode.Return;
	KeyCode secondary_enter = KeyCode.Space;

	public bool button_fading = true;
	public bool in_main_screen = true;
	public bool in_credits = false;
	public int current_button = 1;
	GameObject start_button, credits_button, exit_button, credits;


	void ShowCredits(){
		Color credits_color = credits.GetComponent<Image>().color;
		if (credits_color.a >= 1){
			
		} else {
			credits_color.a += 0.1f;
		}
		credits.GetComponent<Image>().color = credits_color;
	}

	void HideCredits(){
		Color credits_color = credits.GetComponent<Image>().color;
		if (credits_color.a <= 0){

		} else {
			credits_color.a -= 0.1f;
		}
		credits.GetComponent<Image>().color = credits_color;		
	}

	public void MakesButtonBlink(){
		GameObject target_button = null;
		Color full_color = new Color(1,1,1,1);
		switch(current_button){
		case 1:
			target_button = start_button;
			credits_button.GetComponent<Image>().color = full_color;
			exit_button.GetComponent<Image>().color = full_color;
			break;
		case 2:
			target_button = credits_button;
			start_button.GetComponent<Image>().color = full_color;
			exit_button.GetComponent<Image>().color = full_color;
			break;
		case 3:
			target_button = exit_button;
			start_button.GetComponent<Image>().color = full_color;
			credits_button.GetComponent<Image>().color = full_color;
			break;
		}
		Color new_color = target_button.GetComponent<Image>().color;
		if (button_fading){
			if (new_color.a <= 0){
				button_fading = false;
			} else {
				new_color.a -= 0.1f;	
			}
		} else {
			if (new_color.a >= 1){
				button_fading = true;
			} else {
				new_color.a += 0.1f;	
			}
		}
		target_button.GetComponent<Image>().color = new_color;
	}

	public void HideButtons(){
		Color hide_stuff = start_button.GetComponent<Image>().color;
		hide_stuff.a = 0;
		start_button.GetComponent<Image>().color = hide_stuff;
		credits_button.GetComponent<Image>().color = hide_stuff;
		exit_button.GetComponent<Image>().color = hide_stuff;
	}

	public void HandleMenu(){
		if (Input.GetKeyDown(primary_up) || Input.GetKeyDown(secondary_up)){
			if (current_button == 1){
				current_button = 3;
			} else {
				current_button -= 1;
			}
		}
		if (Input.GetKeyDown(primary_down) || Input.GetKeyDown(secondary_down)){
			if (current_button == 3){
				current_button = 1;
			} else {
				current_button += 1;
			}
		}
		if (Input.GetKeyDown(primary_enter) || Input.GetKeyDown(secondary_enter)){
			if (in_main_screen){
				switch(current_button){
				case 1:
					Application.LoadLevel("Match");
					break;
				case 2:
					in_main_screen = false;
					in_credits = true;
					HideButtons();
					break;
				case 3:
					Application.Quit();
					break;
				}				
			} else {
				in_main_screen = true;
				in_credits = false;				
			}

		}

	}

	void Start () {
		start_button = GameObject.Find("start_button");
		credits_button = GameObject.Find("credits_button");
		exit_button = GameObject.Find("exit_button");
		credits = GameObject.Find("credits");
		Color hide_stuff = credits.GetComponent<Image>().color;
		hide_stuff.a = 0;
		credits.GetComponent<Image>().color = hide_stuff;
	}

	void Update () {
		if(in_main_screen){
			MakesButtonBlink();
			HideCredits();
		}
		if (in_credits){
			ShowCredits();
		}
		HandleMenu();
	}
}

