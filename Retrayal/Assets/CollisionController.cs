/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class CollisionController : MonoBehaviour {

	public LayerMask collisionMask;

	const float skinwidth = .016f*7;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float maxClimbAngle = 80;
	float maxDescendAngle = 75;

	float horizontalRaySpacing;
	float verticalRaySpacing;
	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	Bounds bounds;
	public CollisionInfo collisions;

	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider2D> ();
		UpdateUnrotatedBounds ();
		CalculateRaySpacing ();
	}

	public Vector3 Move(Vector3 velocity, Vector2 totforce, bool inside, Quaternion negoldrot){
		UpdateUnrotatedBounds ();
		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.velocityOld = velocity;
		Vector3 dpos = velocity * Time.deltaTime;
		Vector3 dvel = totforce * Time.deltaTime;
		//Vector3 rotatedvelocity = transform.rotation * velocity;
		if (velocity.y < 0){
			DescendSlope (ref dpos);
		}
		if (velocity.x != 0) {
			HorizontalCollisions (ref dpos);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref dpos);
		}
		Debug.DrawRay (transform.position, transform.rotation * dpos / Time.deltaTime, Color.yellow);
		transform.Translate (dpos);
		//print (dpos.x / Time.deltaTime);
		if (!inside) {
			RotateTransform (dvel);
		}
		Quaternion newrot = transform.rotation;
		Quaternion rotdiff = newrot * negoldrot;
		velocity = Quaternion.Inverse(rotdiff) * velocity;
		return velocity;
	}

	void HorizontalCollisions(ref Vector3 velocity){
		float dirX = Mathf.Sign (velocity.x);
		float rayLen = Mathf.Abs (velocity.x) + skinwidth;
		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (dirX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += (Vector2)transform.up * (horizontalRaySpacing * i + velocity.y);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, transform.right * dirX, rayLen, collisionMask);
			Debug.DrawRay (rayOrigin, (Vector2)transform.right * dirX * rayLen, Color.red);
			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, transform.up);


				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						//velocity = collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance - skinwidth;
						velocity.x -= distanceToSlopeStart * dirX;
					}
					ClimbSlope (ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * dirX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinwidth) * dirX;
					rayLen = hit.distance;

					if (collisions.climbingSlope) {
						velocity.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}

					collisions.left = dirX == -1;
					collisions.right = dirX == 1;

				}
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity){
		float dirY = Mathf.Sign (velocity.y);
		float rayLen = Mathf.Abs (velocity.y) + skinwidth;
		RaycastHit2D shortestHit = new RaycastHit2D();

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (dirY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += (Vector2)transform.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, transform.up * dirY, rayLen, collisionMask);
			Debug.DrawRay (rayOrigin, (Vector2) transform.up*dirY*rayLen, Color.blue);
			if (hit) {
				shortestHit = hit;
				velocity.y = (hit.distance-skinwidth) * dirY;
				rayLen = hit.distance;

				if (collisions.climbingSlope) {
					velocity.x = velocity.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				collisions.below = dirY == -1;
				collisions.above = dirY == 1;
			}
		}
		/*Vector2 rayOrigin1 = (dirY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
		rayOrigin1 += (Vector2)transform.right * (verticalRaySpacing * verticalRayCount/2 + velocity.x);
		RaycastHit2D hit1 = Physics2D.Raycast (rayOrigin1, transform.up * dirY, rayLen*1.5f, collisionMask);
		Debug.DrawRay (rayOrigin1, (Vector2) transform.up*dirY*rayLen*1.5f, Color.cyan);
		if (hit1) {
			shortestHit = hit1;
		}*

		if (shortestHit){
			collisions.groundhit = shortestHit;
		}

		if (collisions.climbingSlope) {
			float dirX = Mathf.Sign (velocity.x);
			rayLen = Mathf.Abs (velocity.x) + skinwidth;
			Vector2 rayOrigin = ((dirX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + (Vector2)transform.up*velocity.y;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, transform.right * dirX, rayLen, collisionMask);

			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, (Vector2)transform.up);
				if (slopeAngle != collisions.slopeAngle) {
					velocity.x = (hit.distance - skinwidth) * dirX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle){
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			print ("Climbing Slope!");
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity){
		//print ("1");
		float dirX = Mathf.Sign (velocity.x);
		Vector2 rayOrigin = (dirX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -(Vector2)transform.up, Mathf.Infinity, collisionMask);
		if (hit) {
			//print ("2");
			float slopeAngle = Vector2.Angle (hit.normal, (Vector2)transform.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				//print("3");
				//if (Mathf.Sign ((transform.rotation*hit.normal).x) == dirX) { //idk if this is right for when rotated
					//print("4");
					if (hit.distance - skinwidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x)) {
						float moveDistance = Mathf.Abs (velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;

						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						//print ("Descending Slope!");
						collisions.below = true;
					}
				//}
			}
		}
	}

	void RotateTransform(Vector2 totforce){
		if (collisions.groundhit) {
			//transform.up = collisions.groundhit.normal;
			//Debug.Log ("onGround");
			float zsign = Vector3.Cross((Vector2)transform.up, -(Vector2)totforce.normalized).z;
			transform.RotateAround((Vector3)collisions.groundhit.point, Vector3.forward, Mathf.Sign(zsign) * Vector2.Angle((Vector2)transform.up, -(Vector2)totforce.normalized));
		} else {
			transform.up = -totforce.normalized; //consider adding dampening to this to slow the rotation of the player a bit.
		}
	}

	void UpdateUnrotatedBounds (){
		raycastOrigins.zAngle = transform.rotation.eulerAngles.z;
		transform.Rotate (new Vector3 (0, 0, -raycastOrigins.zAngle));
		bounds = collider.bounds;
		bounds.Expand (skinwidth * -2);
		transform.Rotate (new Vector3 (0, 0, raycastOrigins.zAngle));
	}
	
	// Update is called once per frame
	void UpdateRaycastOrigins () {
		//Vector3 pos = transform.position;
		//transform.position = Vector3.zero;
		raycastOrigins.bottomLeft = RotatePointAroundPivot(new Vector3 (bounds.min.x, bounds.min.y, 0), transform.position, transform.rotation);
		raycastOrigins.bottomRight = RotatePointAroundPivot(new Vector3 (bounds.max.x, bounds.min.y, 0), transform.position, transform.rotation);
		raycastOrigins.topLeft = RotatePointAroundPivot(new Vector3 (bounds.min.x, bounds.max.y, 0), transform.position, transform.rotation);
		raycastOrigins.topRight = RotatePointAroundPivot(new Vector3 (bounds.max.x, bounds.max.y, 0), transform.position, transform.rotation);
		//transform.position = pos;
	}

	void CalculateRaySpacing(){
		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
			
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation){
		return rotation * (point - pivot) + pivot;
	}

	struct RaycastOrigins{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;

		public float zAngle;
	}
	public struct CollisionInfo{
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public bool descendingSlope;

		public float slopeAngle, slopeAngleOld;

		public float belowcounter;

		public Vector3 velocityOld;
		public RaycastHit2D groundhit;

		public void Reset(){
			if (below) {
				belowcounter = 0;
			} else {
				belowcounter += 1;
			}
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
			if (belowcounter > 5) { //change this specified number at some point
				groundhit = new RaycastHit2D ();
			}
		}
	}
}
*/