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
	public float boxW;
	public float boxH;
	private Rect rect;
	private string scoreString;
	public GUIStyle customBox;
	private bool doContinue;
	
	public AudioListener listener;
	public AudioClip pickupSound;
	
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
		doContinue = false;
		Vector3 newPos = new Vector3(0,2,0); 
		transform.position = newPos;
	}
	
	void Update(){
		if(time!=-1 && !wasHere)
			setTimeText();
		
		// controls blinking and effectively disables the players movement
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
		
		// is true, if time is up and blinking done
		if(Time.time > delayedTime && wasHere){
			resetGame();
		}
	}
	
	//called just before performing any physics calculation
	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		movement = new Vector3(moveHorizontal,0.0f,moveVertical);
		rigidbody.AddForce(movement*speed);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Pickup" && !wasHere){
			other.gameObject.SetActive(false);
			AudioSource.PlayClipAtPoint(pickupSound, transform.position);
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
			scoreString = "Your Score: " + count.ToString();
			timeText.text = "Time: 0";
			delayedTime = Time.time + delay;
		}

	}
	
	void resetGame(){
		blink=false;
		timeText.color = Color.black;
		blinkCounter=0;
		if(doContinue){
			doContinue = false;
			StartCoroutine(resetWaiter(1.5f));	
		}
		
	}
	
	// resets time and thus starts the counter
	void resetGameObjects(){
		//Debug.Log(pickups.Length);
		foreach(GameObject pickup in inactivePickups){
			pickup.SetActive(true);
		}
		inactivePickups.Clear();
	}
	
	void OnGUI(){
		if(wasHere)
			StartCoroutine(blinkWaiter());
		if(time == -1){
			GUI.Box(new Rect(Screen.width*boxW-100,Screen.height*boxH,400,200), "Time's up!", customBox);
			GUI.Box(new Rect(Screen.width*boxW-100,Screen.height*boxH+100,400,200), scoreString, customBox);
			if(GUI.Button (new Rect (Screen.width*boxW+300,Screen.height*boxH+160,100,80), "Continue"))
				doContinue = true;
		}
	}
	 
	IEnumerator blinkWaiter(){
		yield return new WaitForSeconds(0.3f*blinkCounter);	
		if(blink){
			timeText.enabled = false;
		}else if(!blink){
			timeText.enabled = true;
		}
	}
	
	// waits for game to run again and does some stuff
	IEnumerator resetWaiter(float time){
		rigidbody.isKinematic = true;
		Vector3 newPos = new Vector3(0,2,0); 
		resetGameObjects();
		transform.position = newPos;
		this.rigidbody.mass = 1;
		this.time = keepTime;
		count = 0;
		setCountText();
		yield return new WaitForSeconds(time);
		rigidbody.isKinematic = false;
		wasHere=false;
		
	}
}

//add multiple levels and the ability to laod them
//todo: one function to end the game (after time run out or all pickups gone)
//put all GUI stuff in its own script!
//clean up
