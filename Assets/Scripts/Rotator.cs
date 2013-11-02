using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	// no forces : we can use update instead of fixedupdate
	void Update () {
		transform.Rotate(new Vector3(15,30,45)*Time.deltaTime);
	}
}
