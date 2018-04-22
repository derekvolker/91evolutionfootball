using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour {

	void Start () {
		int blood_animation = Random.Range(1,3);
		switch(blood_animation){
		case 1:
			this.GetComponent<Animator>().Play("blood_a");
			break;
		case 2:
			this.GetComponent<Animator>().Play("blood_b");
			break;
		case 3:
			this.GetComponent<Animator>().Play("blood_c");
			break;
		}

			
	}
	
}
