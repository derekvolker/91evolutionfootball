using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour {
	private int time_to_live = 1;
	private float[] direction;


	// Use this for initialization
	void Start () {

	}

	public void SetDirection(float[] direction){
		this.direction = direction;
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
		if (collider.tag == "ball"){
			collider.transform.SendMessage("Kicked", direction);
		}
	}
}
