using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public float speed;
	public GUIText countText;
	public GUIText timeText;
	private int count;
	public float time;
	private float keepTime;
	private bool blink;
	public int blinkSpeed;	
	public float delay;
	private float delayedTime;
	private int blinkCounter;
	private bool wasHere;
	private Color textColor;
	private Color countColor;
	private List<GameObject> inactivePickups = new List<GameObject>();
	
	//called before rendering a frame
	void Start(){
		count=0;
		blink = false;
		setCountText();
		delayedTime = 0;
		blinkCounter=0;
		wasHere=false;
		keepTime = time;
		textColor = Color.black;
		countColor = Color.black;
		countText.color = countColor;
		timeText.color = textColor;
		countText.fontSize = 28;
		timeText.fontSize = 28;
	}
	
	void Update(){
		if(time!=-1)
			setTimeText();
		
		if(Time.time < delayedTime){
			timeText.color = Color.red;
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
			timeText.color = Color.black;
		}
	}
	
	//called just before per forming any physics calculation
	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal,0.0f,moveVertical);
		rigidbody.AddForce(movement*speed);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Pickup"){
			other.gameObject.SetActive(false);
			inactivePickups.Add(other.gameObject);
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
		time = keepTime;
		count = 0;
		setCountText();
	}
	
	void OnGUI(){
		if(blink){
			timeText.enabled = false;
		}else{
			timeText.enabled = true;
		}
	}
}
