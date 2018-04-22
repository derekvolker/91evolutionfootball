using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAttackController : MonoBehaviour {
	private int time_to_live = 1;
	private float damage_amount;
	private string target_team;

	// Use this for initialization
	void Start () {
		
	}

	public void SetDamageAmount(float damage_amount){
		this.damage_amount = damage_amount;
	}
	public void SetTargetTeam(string target_team){
		this.target_team = target_team;
	}
	
	// Update is called once per frame
	void Update () {
		if(time_to_live > 0){
			time_to_live -= 1;
		} else {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "team_"+target_team+"_unselected" || collider.tag == "team_"+target_team+"_selected"){
			collider.transform.SendMessage("ReceiveDamage", damage_amount);
		}
	}
}
