/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (CollisionController))]
public class PlayerController : MonoBehaviour {

	public float G;
	public float mass;
	public float jumpheight = 16;
	public float timeToJumpHeight = .5f;
	public float termvel = 30;
	public float moveSpeed = 6;
	public float sprintSpeed;
	public float jetpackforce;
	public float jetpackfueltotal;
	public float jetpackcost;
	public float jetpackrefillground;
	public float jetpackrefillair;
	public Camera maincam;
	public Camera minimapcam;
	public ParticleSystem jetparticles;
	public GameObject GrapplingHook;
	public Text airtimetext;
	public Image jetfuelimage;
	public Image jetfuelimage2;

	public bool talking;

	private float jetpackfuel;
	private float airtime = 0.0f;
	private float highairtime = 0.0f;
	private JetpackFuelUI jetpackim;
	GameObject[] planets;
	//Rigidbody2D mybody;

	Vector2 totforce;
	Vector2 exitvel;
	bool juststarted;
	public bool jetpackexists;
	private GameControllerScript gamecontrollerscript;

	//private Vector2 cam_mins;
	//private Vector2 cam_maxs;
	//public float camerafollowx;
	//public float camerafollowy;

	float accelerationTimeAirborne = .3f;
	float accelerationTimeGrounded = .2f;

	float jumpSpeed;
	float gravity;
	public Vector3 velocity;
	//Vector3 oldvel;
	float velocityXSmoothing;
	//float lastdeltatime;
	bool justjumped;

	CollisionController controller;
	//Quaternion rotdiff = Quaternion.identity;
	public float camerasize = 5;
	public float indoorcamerasize = 5;
	public float camerasizemultiplier = 1;
	public float cameraZoomSpeed = .5f;
	float cameraSmoothing;
	float talkzoomcounter;
	public float talkzoomtime;
	private TwineParser twinecontroller;
	public float maxcorrectdist;
	bool onplanet;
	float planspin;
	//float maxtime;
	//float xloc;

	// Use this for initialization
	public void Start () {
		//gamecontrollerscript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		//maxtime = 0;
		GetComponent<SpriteRenderer> ().receiveShadows = true;
		GetComponent<SpriteRenderer> ().castShadows = true;

		controller = GetComponent<CollisionController> ();
		twinecontroller = GameObject.FindGameObjectWithTag("DialogController").GetComponent<TwineParser>();

		gravity = -2 * jumpheight / Mathf.Pow (timeToJumpHeight, 2);
		jumpSpeed = Mathf.Abs (gravity) * timeToJumpHeight;
		//print ("Gravity: " + gravity + " || JumpSpeed: " + jumpSpeed);
		jetpackim = jetfuelimage.GetComponent<JetpackFuelUI>();
		//jetparticles = GetComponent<ParticleSystem>();
		jetpackfuel = jetpackfueltotal;
		planets = GameObject.FindGameObjectsWithTag("Planet");
		//mybody = GetComponent<Rigidbody2D>();
		//mycollider = GetComponent<CircleCollider2D>();
		gamecontrollerscript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		if (gamecontrollerscript.inside || !jetpackexists) {
			jetfuelimage.enabled = false;
			jetfuelimage2.enabled = false;
		} else {
			jetfuelimage.enabled = true;
			jetfuelimage2.enabled = true;
		}
		//xloc = transform.position.x;
		//Application.targetFrameRate = 3;
		//if (gamecontrollerscript.inside) {
		//	maincam.transform.position = transform.position + Vector3.forward * maincam.transform.position.z + Vector3.up * 5; //Find better wall following method than vector3.up * 5
		//} else {
		maincam.transform.position = transform.position + Vector3.forward * maincam.transform.position.z;
		//}
		minimapcam.transform.position = transform.position + Vector3.forward * minimapcam.transform.position.z;

		//print (gamecontrollerscript);
		talking = false;
		//oldvel = Vector3.zero;
		//lastdeltatime = 1f;
		transform.up = Vector3.up;
		velocity = Vector3.zero;
		maincam.transform.rotation = transform.rotation;
		/*if (!gamecontrollerscript.inside) {
			accelerationTimeAirborne = 100f;
		}*
		//Debug.Log ("YUUUUSSS");
		controller.collisions.belowcounter = 99;
		controller.collisions.Reset ();
		talkzoomtime = 2;
		talkzoomcounter = 0;
		onplanet = false;
		//mybody.gravityScale = gamecontrollerscript.gravity;
		//cam_mins = new Vector2(Screen.width * camerafollowx/2, Screen.height*camerafollowy/2);
		//cam_maxs = new Vector2(Screen.width *(1-camerafollowx/2), Screen.height*(1-camerafollowy/2));
	}
	public void StartAgain(){
		juststarted = true;
		//Debug.Log ("YEEEESSS");
	}

	void FixedUpdate(){
		if (juststarted){
			juststarted = false;
			//Debug.Log ("YAAAASSS");
			Start ();
		}
	}
	// Update is called once per frame
	void Update () {
		/*if (juststarted){
			juststarted = false;
			//Debug.Log ("YAAAASSS");
			Start ();
		}*
		if (!juststarted) {
			//maxtime = Mathf.Max (Time.deltaTime, maxtime);
			//print (maxtime);
			//velocity = rotdiff * velocity;
			jetpackim.Value = (int)jetpackfuel;
			airtime += Time.deltaTime;
			if (airtime > highairtime) {
				highairtime = airtime;
			}
			airtimetext.text = "Airtime: " + Mathf.Round (airtime * 10.0f) / 10.0f + " || HighScore: " + Mathf.Round (highairtime * 10.0f) / 10.0f;


			if (controller.collisions.above || controller.collisions.below) {
				float groundvel;
				/*if (controller.collisions.groundhit && controller.collisions.groundhit.transform.GetComponent<planetmovescript> ()) {
					groundvel = (Quaternion.Inverse (transform.rotation) * controller.collisions.groundhit.transform.GetComponent<planetmovescript> ().MoveVector).y;
					Debug.Log ("groundvel: " + groundvel);
				} else {*
					groundvel = 0;
				//}
				velocity.y = groundvel;
			}
			/*if (controller.collisions.left || controller.collisions.right) {
			velocity.x = 0;
		}*
			Vector2 input = Vector2.zero;
			if (talking) {
				if (twinecontroller.speaker && gamecontrollerscript.inside) {
					Vector2 disttospeaker = Quaternion.Inverse(transform.rotation) * (Vector2)(twinecontroller.speaker.transform.position - transform.position);
					//Debug.Log (disttospeaker.magnitude);
					if (disttospeaker.magnitude > maxcorrectdist) {
						input = new Vector2 (Mathf.Sign (disttospeaker.x), 0);
					}
				}
			} else {
				input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			}

			if (controller.collisions.below) {
				airtime = 0f;
				jetpackfuel += jetpackrefillground;
			} else {
				jetpackfuel += jetpackrefillair;
			}
			if (controller.collisions.belowcounter < 5) {
				if (!justjumped && Input.GetKey (KeyCode.Space) && !talking && input.y >= 0) { // 
					justjumped = true;
					controller.collisions.belowcounter = 99;
					//Debug.Log (controller.collisions.groundhit.transform.gameObject.tag);
					//Figure out how to detect when on a jumppad (for some reason when they are children of planets, the planet is detected instead of the jumppad)
					velocity.y = 0;
					print (controller.collisions.groundhit.transform.name);
					if (controller.collisions.groundhit && controller.collisions.groundhit.transform.tag == "JumpPad") {
						//try{
						velocity.y += controller.collisions.groundhit.transform.GetComponent<JumpPadSettings> ().jumppadstrength;
						Debug.Log ("JETJUMP USED");
						//}catch{
						//	velocity.y += jumpSpeed;


						//}
					} else {
						velocity.y += jumpSpeed;
						//Debug.Log ("velocity.y: " + velocity.y.ToString());
					}
				}
			} else {
				justjumped = false;
			}

			float Speed;

			if (Input.GetAxis ("Sprint") > 0) {
				Speed = sprintSpeed;
			} else {
				Speed = moveSpeed;
			}


			//velocity.x += (targetVelocityX - velocity.x) * Time.deltaTime;
			//////// HORIZONTAL MOVEMENT /////////////////
			if ((gamecontrollerscript.inside || controller.collisions.below)) {
				float targetVelocityX;
				//if (!talking) {
				targetVelocityX = input.x * Speed;
				//} else {
				//	targetVelocityX = 0;
				//}
				if (controller.collisions.groundhit) {
					//////////// ACCOUNTING FOR PLANET SPIN /////////////
					if (controller.collisions.groundhit.transform.GetComponent<SpinScript> ()) {
						Vector3 prerotpos = transform.position;
						transform.RotateAround (controller.collisions.groundhit.transform.position, Vector3.forward, controller.collisions.groundhit.transform.GetComponent<SpinScript> ().spinvel * Time.deltaTime);
						//print ((transform.position - prerotpos).x);
						planspin = (Quaternion.Inverse(transform.rotation) * (prerotpos - transform.position)).x / Time.deltaTime; //eventually turn this into circle circumference instead of exact distance
						//Debug.DrawRay(transform.position, Quaternion.Inverse(transform.rotation) * (prerotpos - transform.position)*5f, Color.cyan);
						if (!onplanet) {
							velocity.x += planspin;
							onplanet = true;
						}
					}
				}
				velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
				//print (targetVelocityX);//print ("VelocitySmoothing: " + velocityXSmoothing.ToString ());
			} else {
				if (onplanet) {
					velocity.x -= planspin;
					onplanet = false;
				}
				/*velocity.x -= planspin;
				planspin = 0;*
				//print ("In Air");
			}

			if (gamecontrollerscript.inside) {
				velocity.y += gravity * Time.deltaTime;
			}

			if (jetpackexists && gamecontrollerscript.jetpackallowed) {
				Vector2 jetpackdir = Input.GetAxisRaw ("Horizontal") * Vector2.right + Input.GetAxisRaw ("Vertical") * Vector2.up;
				//print (jetpackdir);
				if (jetpackdir != Vector2.zero && jetpackfuel > jetpackcost && Input.GetAxisRaw ("Jet") != 0) {
					jetpackfuel -= jetpackcost;
					jetparticles.transform.eulerAngles = new Vector3(90,0,0);
					jetparticles.transform.Rotate (0, Mathf.Rad2Deg * Mathf.Atan2 (jetpackdir.y, jetpackdir.x) - 90 + transform.eulerAngles.z, 0);
					//jetparticles.transform.Rotate (0, 0, Mathf.Rad2Deg * Mathf.Atan2 (jetpackdir.y, jetpackdir.x) + 90 );
					jetparticles.Emit (1);
					velocity += (Vector3)(jetpackdir.normalized * jetpackforce);
				}
			}
			jetpackfuel = Mathf.Min (jetpackfuel, jetpackfueltotal);
			//Debug.Log (gamecontrollerscript.inside);
			List<Color> ambience = new List<Color>();
			List<float> dist = new List<float> ();
			List<float> masses = new List<float> ();
			if (!gamecontrollerscript.inside) {
				totforce = new Vector2 (0, 0);
				for (int i = 0; i < planets.Length; i++) {
					GameObject planet = planets [i];
					Vector2 offset = planet.transform.position - transform.position;
					float pmass = planet.GetComponent<Rigidbody2D> ().mass;

					//My game is 2D, so  I set the offset on the Z axis to 0
					//offset.z = 0;

					//Offset Squared:
					float magsqr = offset.sqrMagnitude;
					ambience.Add (planet.GetComponent<MeshRenderer> ().material.color);
					dist.Add (Mathf.Max(offset.magnitude-planet.transform.lossyScale.x, 0));
					masses.Add (pmass);
					//(Done by Quaternion Inverse Below) MAKE SURE TO SWITCH THE VELOCITY INTO THE FORM OF LOCAL VELOCITY AS SUPPOSED TO GLOBAL VELOCITY.
					//Check distance is more than 0 to prevent division by 0
					if (magsqr > 0.0001f) {
						totforce = (G * offset.normalized / magsqr) * pmass * mass + totforce;
					}
				}
				velocity += Quaternion.Inverse (transform.rotation) * (Vector3)totforce;
			}
			//Calculate ambient lighting
			Color lighting = Color.black;
			for (int i = 0; i < planets.Length; i++) {
				lighting += Mathf.Pow(masses[i], 1f/2f)*ambience [i]/(Mathf.Pow(dist[i],2.5f) + 100);
			}
			float lightdiv = lighting.maxColorComponent;
			lighting = lighting / Mathf.Max (lightdiv, 1);
			RenderSettings.ambientLight = lighting;

			if (velocity.magnitude > termvel) { //should I have termvel?
				print("REACHED TERMINAL VELOCITY");
				velocity = velocity.normalized * termvel;
			}

			Quaternion negoldrot = Quaternion.Inverse (transform.rotation);
			velocity = controller.Move (velocity, totforce, gamecontrollerscript.inside, negoldrot);//oldvel = Quaternion.Inverse(transform.rotation) * 

			if (talking) {
				talkzoomcounter = 0;
			}
			if(talkzoomcounter < talkzoomtime){
				talkzoomcounter += Time.deltaTime;
				float targetCameraSize = indoorcamerasize;
				maincam.orthographicSize = Mathf.SmoothDamp (maincam.orthographicSize, targetCameraSize, ref cameraSmoothing, cameraZoomSpeed);
			}else if (!gamecontrollerscript.inside) {
				float targetCameraSize = camerasize + camerasizemultiplier * velocity.magnitude;
				maincam.orthographicSize = Mathf.SmoothDamp (maincam.orthographicSize, targetCameraSize, ref cameraSmoothing, cameraZoomSpeed);
			} else {
				maincam.orthographicSize = indoorcamerasize;
			}
			if (!gamecontrollerscript.inside) {
				float sign = Vector3.Cross (maincam.transform.up, transform.up).z;
				float anglediff = Mathf.Sign (sign) * Vector2.Angle (maincam.transform.up, transform.up);
				//Debug.Log ("anglediff: " + anglediff);
				maincam.transform.Rotate (new Vector3 (0, 0, anglediff));
			}
			Vector3 cameraoffset = (gamecontrollerscript.inside)? Vector3.up : Vector3.zero;
			Vector2 posdiff = (Vector2) (transform.position + cameraoffset - maincam.transform.position); //eventually find a better way to limit the camera movement than the Vector3.up thing.
			maincam.transform.position += (Vector3)posdiff; 
			minimapcam.transform.position += (Vector3)posdiff;
		}
	}
	public void Place(Vector3 newposition){
		newposition.z = transform.position.z;
		transform.position = newposition;
	}
}*/