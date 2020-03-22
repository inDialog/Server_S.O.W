using UnityEngine;

public class Navigator : MonoBehaviour {

	public Vector3 target;
    private void Update()
    {
      
            float step = 10 * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

        
        //if (target != Vector3.zero)
        //    rigidbody.AddForce (target,ForceMode.Impulse);

    }
	public void NavigateTo (Vector3 position) {

		target = position;
	}


}
