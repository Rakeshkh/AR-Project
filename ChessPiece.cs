using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour {

	bool followl  = false;
	bool followr = false;
	public Transform[] points;
	float timer = 0.0f;
	float timemax = 2.0f;
	bool placeable = false;
	float placetimer = 0.0f;

	Transform findclosest () {
		var closest = points [0];
		for (int i = 0; i < points.Length; i++) {
			if (Vector3.Distance (points [i].position, gameObject.transform.position) < Vector3.Distance (closest.position, gameObject.transform.position)) {
				closest = points [i];
			}
		}
		return closest;
	}

	// Use this for initialization
	void Start () {
		points = GameObject.FindGameObjectWithTag ("points").GetComponentsInChildren<Transform>();
		var closest = findclosest ();
		Debug.Log (closest.position);
		gameObject.transform.position = closest.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (!placeable) {
			placetimer += Time.deltaTime;
		}

		if (followl) {
			timer += Time.deltaTime;
			if (timer >= timemax) {
				placeable = true;
			}
			GameObject tmp = GameObject.FindGameObjectWithTag ("lefttip");
			if (tmp) {
				this.transform.position = new Vector3 (tmp.transform.position.x, tmp.transform.position.y - 5.0f, tmp.transform.position.z);
			}
		} else if (followr) {
			GameObject tmp = GameObject.FindGameObjectWithTag ("righttip");
			if (tmp) {
				this.transform.position = tmp.transform.position;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		Debug.Log ("hit");
		string s = c.gameObject.tag;
		if (s == "handmoverl") {
			if (placetimer >= 2.0f) {
				followr = false;
				followl = true;
				placetimer = 0.0f;
			}
		}else if (s == "handmoverr") {
			if (placetimer >= 2.0f) {
				followl = false;
				followr = true;
				placetimer = 0.0f;
			}
		}
		if (s == "board" && placeable) {
				timer = 0.0f;
				followl = false;
				followr = false;
				placeable = false;
				var closest = findclosest ();
				this.transform.position = closest.position;
		}
		//Debug.Log ("collided" + s);

	}
}
