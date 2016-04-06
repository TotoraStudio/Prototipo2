using UnityEngine;
using System.Collections;

public class EventHolder : MonoBehaviour {

    PlayerControl p1;

	// Use this for initialization
	void Start () {

        p1 = transform.root.GetComponent<PlayerControl>();
	}
    public void ThrowProjectile() {
        p1.specialAttack = true;
    }

}
