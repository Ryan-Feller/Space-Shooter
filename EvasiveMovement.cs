using UnityEngine;
using System.Collections;

public class EvasiveMovement : MonoBehaviour {

    //wait before moving left/right
    public Vector2 startWait;
    //max number the dodge value can be will be set in Inspector
    public float dodge;
    //set this to range of times to keep dodging
    public Vector2 maneuverTime;
    //set this to range of times to wait before dodging again
    public Vector2 maneuverWait;
    //
    public float smoothing;
    public Boundary boundary;
    public float tilt;

    private float currentSpeed;
    private float targetManeuver;
    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
    }

    IEnumerator Evade()
    {

        yield return new WaitForSeconds(Random.Range(startWait.x,startWait.y));

        //constantly moves toward a destination, waits, stops briefly, then starts again
        while (true)
        {
            //if the enemy ship is on the positive side of our x plane, it will change direction to the negative side, vice versa
            targetManeuver = Random.Range(1,dodge)*-Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x,maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x,maneuverWait.y));
        }
    }

    void FixedUpdate()
    {
        //movetowards has an initial position, a direction to move in, a max speed
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
        rb.position = new Vector3(
                //clamps a value between a minumum and maximum (same as PlayerController)
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        rb.rotation = Quaternion.Euler(0.0f, 180.0f, rb.velocity.x * -tilt);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
