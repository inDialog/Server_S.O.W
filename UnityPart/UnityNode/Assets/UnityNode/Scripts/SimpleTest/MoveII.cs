using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveII : MonoBehaviour
{

	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;
	private bool grounded = true;
    public Rigidbody _rigidbody;



    void Awake()
	{
		_rigidbody.freezeRotation = true;
		_rigidbody.useGravity = false;
	}
	private void Update()
    {

        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * 2, 0));

    }
    void FixedUpdate()
	{
		Vector3 jump = Vector3.zero;

		if (grounded)
		{
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Vertical"));
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;

			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = _rigidbody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			_rigidbody.AddForce(velocityChange*2, ForceMode.Impulse);

			// Jump
			if (Input.GetButton("Jump"))
			{
				jump = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			}else
            {
				jump = new Vector3(0, -gravity * _rigidbody.mass, 0);

			}

		}

		// We apply gravity manually for more tuning control
		_rigidbody.AddForce(jump);
	
	}

	void OnCollisionStay()
	{
		grounded = true;
	}

	float CalculateJumpVerticalSpeed()
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity)*100;
	}
	void PusleOnLanding()
	{
			_rigidbody.AddForce(new Vector3(0, gravity * _rigidbody.mass, 0), ForceMode.Impulse);

		//return -gravity * (_rigidbody.mass + DistanceToGround());
	}


    float DistanceToGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, transform.up * -1);
        if (Physics.Raycast(ray, out hit))
        {
            //float dist = ; ;
            return hit.distance;
        }
        else return 10;
    }
}
//	public float speed = 10.0f;
//	public float gravity = 10.0f;
//	public float maxVelocityChange = 10.0f;
//	public bool canJump = true;
//	public float jumpHeight = 2.0f;
//	private bool grounded = false;
//	public Rigidbody _rigidbody;
//    public float[] acceleration;
//	private float inputY = 0.0f;
//	private float inputUp = 0.0f;
//	//public List<GameObject> pastSelcted = new List<GameObject>();

//	public int numberOfFallowers;

//	void Awake()
//	{
//		acceleration = new float[3];
//		_rigidbody.freezeRotation = true;
//	}
//	private void Update()
//	{

//		transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * 2, 0));

//	}
//	void FixedUpdate()
//	{
//		print(grounded);

//		Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Vertical"));
//		Vector3 velocity = _rigidbody.velocity;
//		// Jump or fall
//		if (Input.GetButton("Jump"))
//		{
//			acceleration[0] += 0.1f;
//			targetVelocity.y = CalculateJumpVerticalSpeed() * Mathf.Clamp(acceleration[0],1,10);
//			acceleration[1] = targetVelocity.y;
//		}

//		else
//		{
			
//			if (!grounded)
//			{
//				acceleration[1] -= 0.01f;
//			}
//			else
//				acceleration[1] = 0;

//            targetVelocity.y = gravity * GetComponent<Rigidbody>().mass * Mathf.Clamp(acceleration[1], -2, 2);
//			acceleration[0] = 1;

//		}

//		targetVelocity = transform.TransformDirection(targetVelocity);
//		//speed += Time.deltaTime * 0.1f;

//		targetVelocity *= speed;
//		//acceleration[2] += Time.deltaTime;

//		Vector3 velocityChange = (targetVelocity - velocity);

//		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
//		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
//		velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);




//		_rigidbody.AddForce(velocityChange , ForceMode.VelocityChange);
//		_rigidbody.isKinematic = false;

//		grounded = false;


//	}

//	private void OnCollisionStay(Collision collision)
//    {
//		grounded = true;
//    }
   

//	float CalculateJumpVerticalSpeed()
//	{
//		return Mathf.Sqrt(jumpHeight * gravity);
//	}
//    float DistanceToGround()
//    {
//		RaycastHit hit;
//		Ray ray = new Ray(this.transform.position, transform.up* -1);
//		if (Physics.Raycast(ray, out hit))
//		{
//			//float dist = ; ;
//			return hit.distance;
//		}
//		else return 1;
//}

//}
