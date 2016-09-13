using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

    public int health;

    void Start()
    {
       
    }

    public void damage(int amount)
    {
        health -= amount;
        Debug.Log("health = " +health);
    }

    void Update()
    {
        if (health == 0)
        {
            
        }
    }


}
