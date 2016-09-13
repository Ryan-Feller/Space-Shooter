using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    
    //public so other scripts can change it
    public float speed;
    public float tilt;

    public Boundary boundary;

    //drag our Bolt prefab to this in the inspector
    public GameObject shot;
    //we can use this as a shortcut instead of making a GameObject for shotSpawn and then manually getting its position and rotation
    //we drag our shotSpawn object to this for the inspector
    public Transform shotSpawn;
    public Transform shotSpawn2;
    //how quickly we can fire
    public float fireRate;
    //when we can fire again
    private float nextFire;

    //used for initialization. called once
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //updates once per frame
    void Update()
    {
        //default speed maintained
        if (!Input.GetButton("Fire2")){
            speed = 10;
        }
        //REGULAR SHOT
        //if the button is pressed and it's been long enough
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            fireRate = 0.25f;
            nextFire = Time.time + fireRate;
            //makes the bullet object. we tell unity that it is to be made AS a GameObject
            //naming it clone allows us to reference the object as "clone"
            //GameObject clone = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
            //however, we don't need to do anything with any one shot, so we can just do it like this with no references:
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //play the sound associated with Player 
            GetComponent<AudioSource>().Play();
        }

        //SLOW FOCUS SHOT
        if (Input.GetButton("Fire2") && Time.time > nextFire)
        {
            speed = 5;
            fireRate = 0.1f;
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }


    }

    //executed once per physics step
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //how to move
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //how to move, how fast to do it
        rb.velocity = (movement * speed);

        rb.position = new Vector3
            (
                //clamps a value between a minumum and maximum
                Mathf.Clamp(rb.position.x, boundary.xMin,boundary.xMax), 
                0.0f,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        rb.rotation = Quaternion.Euler(0.0f,0.0f,rb.velocity.x * -tilt);
        

    } 

}

//class to store our values. serializable lets us see the properties in a container in Unity
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}
