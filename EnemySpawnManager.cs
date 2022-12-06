using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    //SPAWN TABLE
    //Enemy control
    public float spawnInterval;
    //controll invoking time
    void Start()
    {
        randomizeSpawnWeights();
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(RandomizeWeightsRotation());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            randomizeSpawnWeights(); //test method
            Debug.Log("Randomizing weights"); 
        }
    }

    //table information
    public GameObject[] spawnTablePos;
    public int[] carPositionWeight;
    public GameObject[] cars;
    public int[] carSpawnWeight;
   
    //car totals
    private int carTotal;
    private int carRandomNumber;


    private int posTotal;
    private int posRandomNumber;

    IEnumerator SpawnEnemyCoroutine()
    {
        carTotal = 0;
        posTotal = 0;

        //initialize data
        var spawnIndex = Random.Range(0, spawnTablePos.Length); //is a random int between 0 and spawnTable length
        var spawnPos = spawnTablePos[spawnIndex].transform.position; //is the position of the item in the spawnTablePos[] 
        var spawnRotation = spawnTablePos[spawnIndex].transform.rotation; //is the rotation in that same table

        foreach(var item in carPositionWeight)
        {
            posTotal += item;
        }
        posRandomNumber = Random.Range(0, posTotal);
        for (int i = 0; i < spawnTablePos.Length; i++)
        {
            if (posRandomNumber <= carPositionWeight[i])
            {
                //change spawn weights
                spawnIndex = i;
                spawnPos = spawnTablePos[spawnIndex].transform.position; //is the position of the item in the spawnTablePos[] 
                spawnRotation = spawnTablePos[spawnIndex].transform.rotation; //is the rotation in that same table
                break;
            }
            else
            {
                posRandomNumber -=carPositionWeight[i];
            }
        }

        foreach (var item in carSpawnWeight)
        {

            carTotal += item;
        }
        carRandomNumber = Random.Range(0, carTotal);
        for (int i = 0; i < cars.Length; i++)
        {
            if (carRandomNumber <= carSpawnWeight[i])
            {
               
                Instantiate(cars[i], spawnPos, spawnRotation); //instantiate car
                
                break;
            }
            else
            {
                carRandomNumber -= carSpawnWeight[i];
            }
        }
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnEnemyCoroutine());
        
    }

    public float RandomizeWeightTime = 5;

    IEnumerator RandomizeWeightsRotation()
    {
        

        yield return new WaitForSeconds(RandomizeWeightTime);
        randomizeSpawnWeights();
        StartCoroutine(RandomizeWeightsRotation());
    }
    void randomizeSpawnWeights()
    {
        //lanes
        int laneCount = 0;
        int newVal = 0;

        


        for (int i = 0; i < spawnTablePos.Length/2; i++) //checks half the spawning nodes(12), and turns it into (6) lanes
        {
            var currentLaneNum = 0; //track what lane is currently used, 0 or 1

            for (int j = 0; j < 2; j++)
            {
                //randomize the lane value's between 0 and 1
                //at least one of the lanes needs to have a value

                int randomLaneValue = Random.Range(0, 2); // when using random.range, ints are Maximally Inclusive, where as floats are not
                carPositionWeight[laneCount] = randomLaneValue;
                laneCount++;
                if(randomLaneValue == 1)
                {
                    //Debug.Log("current lane num is " + currentLaneNum);

                    if (currentLaneNum == 0)
                    {
                        laneCount++;
                    }
                    break;
                }

                currentLaneNum++;
            }
            newVal++;
        }
    }
}
