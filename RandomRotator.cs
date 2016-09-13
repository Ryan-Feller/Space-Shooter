using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {

    //max value for tumbling
    public float tumble;

	// Use this for initialization
	void Start () {

        //more elegant way up declaring this
        Rigidbody asteroidBody = GetComponent<Rigidbody>();

        //this random insideunitsphere thing does a random vector3 value
        asteroidBody.angularVelocity = Random.insideUnitSphere * tumble;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
