using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
    //added this script to the explosion prefabs to destroy them after 2 seconds

    public float lifetime;

	// Use this for initialization
	void Start () {

        //destroy game object after set amount of time
        Destroy(gameObject, lifetime);
	}

}
