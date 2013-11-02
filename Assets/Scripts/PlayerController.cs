using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public float speed;
	public GUIText countText;
	public GUIText timeText;
	private int count;
	public float time;
	private bool blink;
	public int blinkSpeed;	
	public float delay;
	private float delayedTime;
	private int blinkCounter;
	private bool wasHere;
	private List<GameObject> inactivePickups = new List<GameObject>();
	
	//called before rendering a frame
	void Start(){
		count=0;
		blink = false;
		setCountText();
		delayedTime = 0;
		blinkCounter=0;
		wasHere=false;
	}
	
	void Update(){
		if(time!=-1)
			setTimeText();
		
		if(Time.time < delayedTime){
			if(blinkCounter==blinkSpeed){
				blink = false;
				blinkCounter=0;
			}else{
				blink = true;
			}
			blinkCounter++;
			wasHere = true;
		}
		if(Time.time > delayedTime && wasHere){
			resetGameObjects();
			wasHere=false;
			blink=false;
		}
	}
	
	//called just before performing any physics calculation
	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal,0.0f,moveVertical);
		rigidbody.AddForce(movement*speed);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Pickup"){
			//Destroy (other.gameObject);  ...or...
			other.gameObject.SetActive(false);
			inactivePickups.Add(other.gameObject);
			//other.gameObject.active=false;
			++count;
			setCountText();
		}
	}
	
	void setCountText(){
		countText.text="Count: " + count.ToString();
	}
	
	void setTimeText(){
		if(time>0){
			time -= Time.deltaTime;
			timeText.text = "Time: " + ((int) time).ToString();
		}else{
			time = -1;
			timeText.text = "Time's Up!";
			delayedTime = Time.time + delay;
		}

	}
	void resetGameObjects(){
		//Debug.Log(pickups.Length);
		foreach(GameObject pickup in inactivePickups){
			pickup.SetActive(true);
		}
		inactivePickups.Clear();
		time = 15;
	}
	
	void OnGUI(){
		if(blink){
			timeText.enabled = false;
			timeText.pixelOffset += Vector2(-50,-50);
		}else{
			timeText.enabled = true;
		}
	}
}
