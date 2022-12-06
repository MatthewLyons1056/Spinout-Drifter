using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCLoader : MonoBehaviour
{
    /*
     CCLoader(Character Customization Loader) will load the current customization options that the player
    put in place

    -at start the car will put all subobjects in an array
    -

     */
    [SerializeField] private MeshRenderer[] carBodies;
    [SerializeField] private GameObject[] carTires;


    private void Start()
    {
        PlayerPrefs.GetInt("CarBody");
        PlayerPrefs.GetInt("CarTires");

        LoadCC();

    }

    private void OnLevelWasLoaded()
    {
        LoadCC();
    }

    void LoadCC()
    {
        //Debug.Log(" LOAD: Tire number is " + PlayerPrefs.GetInt("CarTires") + ", and body number is " + PlayerPrefs.GetInt("CarBody"));

        var i = 0;
        //load car body
        foreach (MeshRenderer carBody in carBodies)
        {

            
            carBodies[i].enabled = false;
            i++;
        }
        carBodies[PlayerPrefs.GetInt("CarBody")].enabled = true;


        /*//load car tires
        var j = 0;
        foreach (GameObject carTire in carTires)
        {
            
            carTires[j].SetActive(false);
            j++;
        }
        carTires[PlayerPrefs.GetInt("CarTires")].SetActive(true);*/


        LoadColor();
        LoadTires();
    }

    void LoadColor()
    {
        //load color

        GameObject carBodyColor = GameObject.Find("CarBody");

        MeshRenderer ren;

        CarData carData;

        MeshFilter color;

        int carColorCount;

        for (int k = 0; k < carBodyColor.transform.childCount; k++) //goes through each child object of carBody, AKA each car body
        {

            //search the carBody children and find the active car

            ren = carBodyColor.transform.GetChild(k).gameObject.GetComponentInChildren<MeshRenderer>();//set ren = to the current child(i)
            if (ren.enabled == true) //if the renderer is enabled(meaning its the correct car) 
            {

                //if the car is active, get the meshFilter
                color = ren.GetComponentInParent<MeshFilter>();
                //find the mesh holder on the car
                carData = ren.GetComponentInParent<CarData>();

                carColorCount = PlayerPrefs.GetInt("CarColor");



                //Debug.Log("car index is " + carData.carColors.Length);
                color.mesh = carData.carColors[carColorCount];
                //set the mesh = to the carColorCount which is equal to the parameter 'intval'
                //Debug.Log("Car color is " + carData.carColors[carColorCount].name);
                //Debug.Log(carData.gameObject.name);
                break;

            }
            //Change the mesh filter

        }
    }
    void LoadTires()
    {
        GameObject carBody = GameObject.Find("CarBody");
        MeshRenderer ren;
        
        CarData carData;
        int tireCount;
        int j = 0;
        for (int k = 0; k < carBody.transform.childCount; k++) //goes through each child object of carBody, AKA each car body
        {

            //search the carBody children and find the active car

            ren = carBody.transform.GetChild(k).gameObject.GetComponentInChildren<MeshRenderer>();//set ren = to the current child(i)
            if (ren.enabled == true) //if the renderer is enabled(meaning its the correct car) 
            {

                //if the car is active, get the meshFilter
                
                //find the mesh holder on the car
                carData = ren.GetComponentInParent<CarData>();


                foreach (GameObject tire in carData.carTires)
                {
                    //disable all meshes
                    carData.carTires[j].SetActive(false);
                    j++;
                }
                tireCount = PlayerPrefs.GetInt("CarTires");
                //Debug.Log("Loading wheel number: " + tireCount);
                carData.carTires[tireCount].SetActive(true);

                break;

            }

            //Change the mesh filter

        }

    }

}


