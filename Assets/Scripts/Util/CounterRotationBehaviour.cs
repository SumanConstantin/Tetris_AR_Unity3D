using UnityEngine;
using System.Collections;

public class CounterRotationBehaviour : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		Vector3 srcRot = this.transform.parent.transform.rotation.eulerAngles;
		this.transform.Rotate(-srcRot.x, -srcRot.y, -srcRot.z);
	}
}
