using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public void onClick()
    {
        SceneManager.LoadScene("Main");
    }
	
}
