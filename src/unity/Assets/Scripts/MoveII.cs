using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveII : MonoBehaviour
{

	 float speed1 = 6f;
	 float speed2 =1f;
	 float speed3 = 5f;

	 float speed;

	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;
	private bool grounded = true;
    public Rigidbody _rigidbody;
	float jumpTime;


    void Awake()
	{
		_rigidbody.freezeRotation = true;
		_rigidbody.useGravity = false;
	}
	//private void Update()
	//   {


	//   }
	void FixedUpdate()
	{
		float _speed = Time.deltaTime*70;
		Vector3 jump = Vector3.zero;
		transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * _speed, 0));


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
		_rigidbody.AddForce(velocityChange, ForceMode.Impulse);

		// Jump
		if (Input.GetButton("Jump"))
		{
			jump = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
		}
		else if (!grounded)
		{
			if (_rigidbody.velocity.y < 0.1f)
				jumpTime += Time.deltaTime / 2.5f;
			else jumpTime = 0.5f;
			jumpTime = Mathf.Clamp(jumpTime, 0.1f, _rigidbody.mass);
			if (transform.position.y > -1)
				jump = new Vector3(0, -gravity * jumpTime, 0);
			else
                if(_rigidbody.velocity != Vector3.zero)
				_rigidbody.AddForce(new Vector3(0, -_rigidbody.velocity.y*10, 0));
			


			//print(jumpTime);

		}
		else
		{
			_rigidbody.AddForce(new Vector3(0, -gravity, 0));

		}

		_rigidbody.AddForce(jump);
		if (grounded)
			speed = speed2;
		else
			if (!Input.GetButton("Jump"))
			speed = speed3;
		else
			speed = speed1;
		grounded = false;

		// We apply gravity manually for more tuning control
	}

    private void OnTriggerStay(Collider other)
    {
		grounded = true;
	}
	private void OnTriggerEnter(Collider other)
	{
		grounded = true;
	}
	private void OnCollisionStay(Collision collision)
    {

	}

    float CalculateJumpVerticalSpeed()
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(1000 * jumpHeight * gravity);
	}




}



