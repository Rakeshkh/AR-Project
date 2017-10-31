using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmos : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (gameObject.transform.position, 0.5f);
	}
}
