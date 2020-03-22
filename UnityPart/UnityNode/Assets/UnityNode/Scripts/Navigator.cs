using UnityEngine;

public class Navigator : MonoBehaviour {

	Vector3 target;

    private void Update()
    {
		if (target != Vector3.zero)
		{
			float step = 10 * Time.deltaTime; // calculate distance to move
			transform.position = Vector3.MoveTowards(transform.position, target, step);
		}
	}
    public void NavigateTo (Vector3 position) {

		target = position;
	}


}
