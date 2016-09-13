using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

    //create explosion parameter. drag the prefab for explosion_asteroid into this slot
    public GameObject explosion;
    //do the same for playerexplosion
    public GameObject playerExplosion;
    //instantiate a gameController class. we set it as private so it doesn't show up in the inspector
    private GameController gameController;
    //value we pass to the score in GameController
    public int value;

    void Start()
    {
        //each asteroid will find its own instance of the first (and in our case only) instance of whatever is tagged Game Controller
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        //if it finds one, grab its component and store it as the object we declared above
        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        //just in case something goes wrong
        if(gameController == null)
        {
            Debug.Log("cannot find 'GameController' script");
        }
    }

    //when another object enters the collider, we get a reference to that object as "other"
    void OnTriggerEnter(Collider other)
    {

        //if the other object is tagged with boundary, just exit
        if (other.tag == "Boundary" || other.CompareTag("Enemy"))
        {
            return;
        }

        //enemies can't kill themselves
        if(gameObject.tag == "Enemy" && other.tag == "EnemyBolt")
        {
            return;
        }

        if(gameObject.tag =="Enemy" && other.tag == "GiantAsteroid")
        {
            return;
        }

        //nothing can kill itself
        if(gameObject.tag == other.tag)
        {
            return;
        }

        //player shots cannot kill enemy bolts
        if (gameObject.tag == "EnemyBolt" && other.tag == "Bolt")
        {
            return;
        }

        if(gameObject.tag =="GiantAsteroid" && other.tag == "Bolt")
        {
            gameObject.GetComponent<HealthController>().damage(1);
            Debug.Log("doing 1 damage");
            Destroy(other.gameObject);

            //break into 4 new asteroids
            if (gameObject.GetComponent<HealthController>().health == 0)
            {
                Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Quaternion spawnRotation = Quaternion.identity;
                spawnPosition.x -= 2;
                spawnPosition.z -= Random.Range(0, 1);
                Instantiate(gameController.hazards[Random.Range(0, 2)], spawnPosition, spawnRotation);
                spawnPosition.x += 1.4f;
                spawnPosition.z -= Random.Range(0, 1);
                Instantiate(gameController.hazards[Random.Range(0, 2)], spawnPosition, spawnRotation);
                spawnPosition.x += 1.4f;
                spawnPosition.z -= Random.Range(0, 1);
                Instantiate(gameController.hazards[Random.Range(0, 2)], spawnPosition, spawnRotation);
                spawnPosition.x += 1.4f;
                spawnPosition.z -= Random.Range(0, 1);
                Instantiate(gameController.hazards[Random.Range(0, 2)], spawnPosition, spawnRotation);

                Destroy(gameObject);
            }
            else return;
        }

        //this is jank, but it's necessary because the enemy shots don't have an explosion for anything but hitting the player
        if (explosion != null)
        {
            //instantiate explosion animation on our position and rotation
            Instantiate(explosion, transform.position, transform.rotation);
        }

        //if the ship collides with anything, game over
        if (other.tag == "Ship")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }

        //add score to the game controller we created above. in the inspector, you 
        gameController.AddScore(value);

        //destroy whatever touches this
        Destroy(other.gameObject);
        Debug.Log("Destroying down below");

        //destroy this object too
        Destroy(gameObject);
    }

   
}
