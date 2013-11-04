using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public GameObject player;
	//public AudioListener listener;
	private Vector3 offset; //offset to player object

	void Start () {
		offset = transform.position;
	}
	
	//procedural animation and gathering last known code
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
