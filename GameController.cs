using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    //lock the inspector to drag all 3 asteroid prefab types over to this element
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public float spawnWait;
    public float startWait;
    private bool spawningEnemies;

    //don't forget to drag these here in the inspector
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text getReadyText;

    private bool beatlevel1;
    private bool beatlevel2;
    private bool beatlevel3;
    private int level;
    private bool newlevel;
    private bool gameOver;
    private bool restart;
    private int score;
    public string nextLevelName;
    public int level1score;
    public int level2score;
    public int level3score;
    public int advanceScore;
    public int initialasteroids;
    public int initialenemies;
    public int level1asteroids;
    public int level1enemies;
    public int level2asteroids;
    public int level2enemies;
    public int level3asteroids;
    public int level3enemies;

    IEnumerator Startup()
    {
        yield return new WaitForSeconds(1);
        getReadyText.GetComponent<AudioSource>().Play();
        getReadyText.text = "Get Ready!";
        yield return new WaitForSeconds(0.5f);
        getReadyText.text = "";
        yield return new WaitForSeconds(0.5f);
        getReadyText.GetComponent<AudioSource>().Play();
        getReadyText.text = "Get Ready!";
        yield return new WaitForSeconds(0.5f);
        getReadyText.text = "";
        yield return new WaitForSecondsRealtime(0.5f);
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(spawnEnemies(initialasteroids, initialenemies));
    }

    //making this a coroutine allows us to return wait times
    IEnumerator spawnEnemies(int asteroids, int enemies)
    {
        if (spawningEnemies)
        {
            //gives a buffer of time between each wave
            yield return new WaitForSeconds(startWait);

            while (true)
            {

                List<GameObject> spawnList = new List<GameObject>();

                //normal asteroids
                for (int j = 0; j < asteroids; j++)
                {
                    GameObject asteroidHazard = hazards[UnityEngine.Random.Range(0, 2)];
                    spawnList.Add(asteroidHazard);
                }

                for (int k = 0; k < enemies; k++)
                {
                    //for level 1, "enemy" means enemy
                    if (SceneManager.GetActiveScene().name == "Main")
                    {
                        GameObject enemyHazard = hazards[3];
                        spawnList.Add(enemyHazard);
                    }
                    //for level 2, "enemy" means giant asteroid
                    else if (SceneManager.GetActiveScene().name == "Level2")
                    {
                        GameObject enemyHazard = hazards[UnityEngine.Random.Range(3, hazards.Length)];
                        spawnList.Add(enemyHazard);
                    }


                }

                shuffleList(spawnList);

                foreach (GameObject item in spawnList)
                {
                    Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(item, spawnPosition, spawnRotation);
                    //wait for a few seconds to spawn more waves
                    yield return new WaitForSeconds(spawnWait);
                }

                //level advancement
                if (score > level1score && beatlevel1 == false)
                {
                    beatlevel1 = true;
                    newlevel = true;
                    level = 1;
                    break;
                }
                if (score > level2score && beatlevel2 == false)
                {
                    beatlevel2 = true;
                    newlevel = true;
                    level = 2;
                    break;
                }
                if (score > level3score && beatlevel3 == false)
                {
                    beatlevel3 = true;
                    newlevel = true;
                    level = 3;
                    break;
                }


            }
        }
        else yield return new WaitForSeconds(3.0f);
    }

    //takes the level set in the spawnEnemies routine
    void changeLevel(int level)
    {
        switch (level)
        {
            case 1: StartCoroutine(spawnEnemies(level1asteroids, level1enemies));
                break;
            case 2: StartCoroutine(spawnEnemies(level2asteroids,level2enemies));
                break;
            case 3: StartCoroutine(spawnEnemies(level3asteroids, level3enemies));
                break;
        }
    }

    //this is called first
    void Start()
    {
        beatlevel2 = false;
        beatlevel3 = false;
        beatlevel1 = false;
        newlevel = false;
        gameOver = false;
        restart = false;
        spawningEnemies = true;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        getReadyText.text = "";
        //call a coroutine
        StartCoroutine(Startup());

    }

    //called once per frame
    void Update()
    {
        if (score > advanceScore)
        {
            StartCoroutine(loadNewLevel());
        }

        if (gameOver)
        {
            restartText.text = "Press R to Restart";
            restart = true;
            gameOver = false;
        }

        if (newlevel)
        {
            newlevel = false;
            changeLevel(level);
            
        }

        //handles the key press on death
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

    }

    //shuffles list of game objects. aka the fisher-yates shuffle
    public void shuffleList(List<GameObject> aList)
    {
        GameObject shoof;
        int n = aList.Count;
        System.Random rnd = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            shoof = aList[k];
            aList[n] = aList[n];
            aList[n] = shoof; 
        }
         
    }

    IEnumerator loadNewLevel()
    {
        spawningEnemies = false;
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            restartText.text = "Area Clear" + Environment.NewLine + "Advancing to Next Area";
            yield return new WaitForSecondsRealtime(5.0f);
            SceneManager.LoadScene(nextLevelName);
        }

    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
    }

}
