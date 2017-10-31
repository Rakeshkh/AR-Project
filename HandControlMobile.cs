using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class HandControlMobile : MonoBehaviour {

	Thread receiveThread;

	//UdpClient client;
	RakeshServer rclient;

	//private int port = 1999; 

	string[] sArray = new string[16];

	public GameObject rightHandPrefab,leftHandPrefab;

	private GameObject currentRightHand, currentLeftHand;

	private bool shouldUpdateHands = false;
	private bool shouldDestroyHands = false;

	private bool rightHandExists = false;
	private bool leftHandExists = false;

	private Vector3 position1, position2;
	private Quaternion rotation1, rotation2;


	public void Start()
	{
		Application.targetFrameRate = 60;
		

		receiveThread = new Thread(new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}

	void Update(){

		if (shouldDestroyHands) {
			shouldDestroyHands = false;
			if (transform.childCount > 0) {
				DestroyHands ();
			}
		}

		if (shouldUpdateHands) {
			shouldUpdateHands = false;
			UpdateHands ();
		}
	}

	void UpdateHands(){

		rightHandExists = false;
		leftHandExists = false;
		Quaternion loffset = Quaternion.AngleAxis (90.0f, Vector3.up);
		Quaternion roffset = loffset * Quaternion.AngleAxis (180.0f, Vector3.left);// * Quaternion.AngleAxis (0.0f, Vector3.forward);  


		if (sArray.Length > 1) {
			sArray[1] = sArray [1].TrimStart ('(');
			sArray [3] = sArray [3].TrimEnd (')');
			sArray [4] = sArray [4].TrimStart ('(');
			sArray [7] = sArray [7].TrimEnd (')');
	

			position1 = new Vector3(float.Parse(sArray[1]),float.Parse(sArray[2]),float.Parse(sArray[3]));
			rotation1 = new Quaternion(float.Parse(sArray[4]),float.Parse(sArray[5]),float.Parse(sArray[6]),float.Parse(sArray[7]));


			//rotation1 = new Quaternion(0.0f,0.0f,0.0f,0.0f);
			if (sArray [0] == "l") {
				leftHandExists = true;
				if (currentLeftHand == null) {
					currentLeftHand = Instantiate (leftHandPrefab, this.transform);
				}

				currentLeftHand.transform.transform.localPosition = position1;
				currentLeftHand.transform.transform.localRotation = rotation1 * loffset;

			} else if (sArray [0] == "r") {
				rightHandExists = true;
				if (currentRightHand == null) {
					currentRightHand = Instantiate (rightHandPrefab, this.transform);
				}



				currentRightHand.transform.transform.localPosition = position1;
				currentRightHand.transform.transform.localRotation = rotation1 * roffset;
			}
		}

		if (sArray.Length > 9) {
			sArray[9] = sArray [9].TrimStart ('(');
			sArray [11] = sArray [11].TrimEnd (')');
			sArray [12] = sArray [12].TrimStart ('(');
			sArray [15] = sArray [15].TrimEnd (')');


			position2 = new Vector3(float.Parse(sArray[9]),float.Parse(sArray[10]),float.Parse(sArray[11]));
			rotation2 = new Quaternion(float.Parse(sArray[12]),float.Parse(sArray[13]),float.Parse(sArray[14]),float.Parse(sArray[15]));


			if (sArray [8] == "l") {
				leftHandExists = true;
				if (currentLeftHand == null) {
					currentLeftHand = Instantiate (leftHandPrefab, this.transform);
				}

				currentLeftHand.transform.transform.localPosition = position2;
				currentLeftHand.transform.transform.localRotation = rotation2 * loffset;

			} else if (sArray [8] == "r") {
				rightHandExists = true;
				if (currentRightHand == null) {
					currentRightHand = Instantiate (rightHandPrefab, this.transform);
				}

				currentRightHand.transform.transform.localPosition = position2;
				currentRightHand.transform.transform.localRotation = rotation2 * roffset;
			}
		}

		if (!leftHandExists) {
			Destroy (currentLeftHand);
		}
		if (!rightHandExists) {
			Destroy (currentRightHand);
		}
	}

	void DestroyHands(){

		foreach (Transform child in gameObject.transform) {

			Destroy (child.gameObject);
		}
	}

	// receive thread
	void ReceiveData()
	{
		//client = new UdpClient(port);
		//IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
		rclient = new RakeshServer();

		while (true)
		{
			//byte[] data = client.Receive(ref anyIP);

			//string text = Encoding.UTF8.GetString(data);
			string text = rclient.get("hands");
			Debug.Log (text);

			// split the items by comma
			sArray = text.Split(',');

			if (sArray[0] != "nothing"){

				shouldUpdateHands =  true;

			} else {

				shouldDestroyHands = true;
			}

		}
	}
}