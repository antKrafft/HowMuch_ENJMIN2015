using UnityEngine;
using System.Collections;

public class TPMovement : MonoBehaviour {

	public float speed;
	public float rotateSpeed = 90;
	public int angleMax;
	public float zSpeed;
	public GameObject droite, gauche;

	private CharacterController cc;
	private Animator an;

	private bool touchWallG = false;
	private bool touchWallD = false;
	private int sightCount = 0;

	//différentes caméras de jeu
	public Camera introCam;
	public Camera endCam;
	public Camera closeCam;
	public Camera pursuitCam;
	public Camera reverseCam;
	public Camera howMuchCam;
	public Camera sightCam;

	public Light spotChicks;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		cc = this.GetComponent<CharacterController> ();
		an = this.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		float angleDroite = Quaternion.Angle (transform.rotation, droite.transform.rotation);
		float angleGauche = Quaternion.Angle (transform.rotation, gauche.transform.rotation);

		//Rotation du poulet
		if(Input.GetKey(KeyCode.D) && (angleDroite > angleMax))
		{
			transform.Rotate(Vector3.up, Mathf.Clamp(rotateSpeed * Time.deltaTime, 0f, 360f));
		}
		if(Input.GetKey(KeyCode.Q) && (angleGauche > angleMax))
		{
			transform.Rotate(Vector3.up, -Mathf.Clamp(rotateSpeed * Time.deltaTime, 0f, 360f));
		}

		//déplacement automatique du poulet sur l'axe X (Space.World)
		transform.Translate(new Vector3(-speed,0,0) * Time.deltaTime, Space.World);

		//déplacement du poulet sur l'axe Z (Space.World) en fonction de sa rotation
		if ((angleDroite > angleGauche) && (touchWallG  == false)) {
			touchWallD = false;
			cc.Move(new Vector3 (0, 0, -zSpeed) * Time.deltaTime);
		} else if ((angleGauche > angleDroite) && (touchWallD  == false)) {
			touchWallG = false;
			cc.Move(new Vector3 (0, 0, zSpeed) * Time.deltaTime);
		}

	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		float angleDroite = Quaternion.Angle (transform.rotation, droite.transform.rotation);
		float angleGauche = Quaternion.Angle (transform.rotation, gauche.transform.rotation);

		if (angleDroite > angleGauche) {
			touchWallG = true;
		} else if (angleGauche > angleDroite) {
			touchWallD = true;
		}
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject.layer == 8){
			spotChicks.active = true;
			howMuchCam.active = false;
			reverseCam.active = true;
			endCam.active = true;
			speed = speed / 3;
			zSpeed = zSpeed / 3;
			rotateSpeed = rotateSpeed / 5;
			an.speed = an.speed / 3;
		}
		if (other.gameObject.layer == 9){
			introCam.active = false;
			sightCam.active = false;
			pursuitCam.active = true;
		}
		if (other.gameObject.layer == 10){
			endCam.active = false;
			reverseCam.active = false;
			closeCam.active = true;
		}
		if (other.gameObject.layer == 11){
			if (sightCount == 1){
				sightCam.fieldOfView = 13;
				sightCount++;
			}
			else{
				pursuitCam.active = false;
				sightCam.active = true;
				sightCount++;
			}
		}
		if (other.gameObject.layer == 13){
			pursuitCam.active = false;
			howMuchCam.active = true;
		}
	}
	
}
