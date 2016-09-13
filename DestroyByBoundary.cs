using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

    //our boundary is a trigger. OnTriggerExit(Collider) triggers an event when something leaves an area
    void OnTriggerExit(Collider other)
    {
        //destroy anything that exits the area
        Destroy(other.gameObject);
    }

}
