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
	private Vector3 movement;
	
	private bool blink;
	public float delay;
	private float delayedTime;
	private bool wasHere;
	private int blinkCounter;
	
	private Color textColor;
	private Color countColor;
	
	private List<GameObject> inactivePickups = new List<GameObject>();
	
	//called before rendering a frame
	void Start(){
		count=0;
		blink = false;
		setCountText();
		delayedTime = 0;
		wasHere=false;
		keepTime = time;
		textColor = Color.black;
		countColor = Color.black;
		countText.color = countColor;
		timeText.color = textColor;
		countText.fontSize = 28;
		timeText.fontSize = 28;
		blinkCounter=0;
		
		Vector3 newPos = new Vector3(0,2,0); 
		transform.position = newPos;
	}
	
	void Update(){
		if(time!=-1)
			setTimeText();
		
		if(Time.time < delayedTime){
			rigidbody.mass = 10000000;
			rigidbody.AddForce(-movement*speed*0.5f);
			++blinkCounter;
			timeText.color = Color.red;
			if(blink){
				blink = false;
			}else{
				blink = true;
			}
			wasHere = true;
		}
		
		if(Time.time > delayedTime && wasHere){
			resetGameObjects();
			blink=false;
			timeText.color = Color.black;
			blinkCounter=0;
			StartCoroutine(justWait(1.5f));
		}
	}
	
	//called just before per forming any physics calculation
	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		movement = new Vector3(moveHorizontal,0.0f,moveVertical);
		rigidbody.AddForce(movement*speed);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Pickup" && !wasHere){
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
		if(time == keepTime)
			time++;
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
		if(wasHere)
			StartCoroutine(blinkWaiter());
	}
	
	IEnumerator blinkWaiter(){
		yield return new WaitForSeconds(0.2f*blinkCounter);	
		if(blink){
			timeText.enabled = false;
		}else if(!blink){
			timeText.enabled = true;
		}
	}
	
	IEnumerator justWait(float time){
		rigidbody.isKinematic = true;
		Vector3 newPos = new Vector3(0,2,0); 
		transform.position = newPos;
		this.rigidbody.mass = 1;
		yield return new WaitForSeconds(time);
		rigidbody.isKinematic = false;
		wasHere=false;
	}
}
