using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CCControler : MonoBehaviour
{
    //create custom characters

    /*
     find a way yo create an array that hold a series of meshes, then rotate through them and store it in a global class
    the global class will store the int values, and set it to the player during game start
    ez.
     */

    //hold menu buttons

    public Image[] textImageBackgrounds;
    int imageCount = 0;
    //hold reference renderers
    public MeshRenderer[] meshesBody;
    int meshCount = 0;


    //Car Data
    CarData carData;


    //hold tires
    public GameObject[] tires;
    public MeshRenderer[] tireMesh;
    int tireCount = 0;
    public GameObject carBody;

    public string ccPartNameController = "body";

    //car color data

    public GameObject colorParent;
    public MeshFilter color;
    int carColorCount = 0;
    public Mesh testMesh;

    //hold data
    int carCCBody;
    int carCCTires;
    int carCCColor;

    public void SaveCarCC()
    {
        //hard save
        carCCBody = meshCount; //body
        carCCTires = tireCount; //tires
        carCCColor = carColorCount; //body color

        //playerPrefs save
        PlayerPrefs.SetInt("CarBody", carCCBody);
        PlayerPrefs.SetInt("CarTires", carCCTires);
        PlayerPrefs.SetInt("CarColor", carCCColor);
        Debug.Log(" SAVE: Tire number is " + PlayerPrefs.GetInt("CarTires") + ", and body number is " + PlayerPrefs.GetInt("CarBody"));
    }
    public void LoadCarCC()
    {
        //set data to save data

        meshCount = carCCBody;
        tireCount = carCCTires;
        carColorCount = carCCColor;

        //load body

        int i = 0;
        foreach (MeshRenderer mesh in meshesBody)
        {
            //disable all meshes
            meshesBody[i].enabled = false;
            i++;
        }
        
        //enable the currently selected mesh

        meshesBody[meshCount].enabled = true;

        //load tires

        i = 0;
        foreach (GameObject tire in tires)
        {
            //disable all meshes
            tires[i].SetActive(false);
            i++;
        }
        
        //enable the currently selected mesh
       
        tires[tireCount].SetActive(true);

        //load color

        LoadColor();

    }
    public void Start()
    {
        meshCount = PlayerPrefs.GetInt("CarBody");
        carColorCount = PlayerPrefs.GetInt("CarColor");
        tireCount = PlayerPrefs.GetInt("CarTires");

        

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //move cc menu down
            ccControler(1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ccControler(-1);
            
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ControlCCPanel(ccPartNameController, 1); //sellect right
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ControlCCPanel(ccPartNameController, -1); //sellect left
        }
    }

    void RotateCarBodyMesh(int intval)
    {

        //remove current car tires 

        GameObject carBody = GameObject.Find("CarBody");
        MeshRenderer ren;



        RemoveWheels();

        int i = 0;
        foreach (MeshRenderer mesh in meshesBody)
        {
            //disable all meshes
            meshesBody[i].enabled = false;
            i++;
        }
        
        //enable the currently selected mesh
        
        meshCount+=intval;
        
        if (meshCount >= meshesBody.Length)
        {
            meshCount = 0;
        }
        if(meshCount < 0)
        {
            meshCount = meshesBody.Length - 1; //-1 so it matches the index range that starts at 0
        }
        meshesBody[meshCount].enabled = true;

        LoadWheels();

        void LoadWheels()
        {
            for (int k = 0; k < carBody.transform.childCount; k++) //goes through each child object of carBody, AKA each car body
            {

                //search the carBody children and find the active car

                ren = carBody.transform.GetChild(k).gameObject.GetComponentInChildren<MeshRenderer>();//set ren = to the current child(i)
                if (ren.enabled == true) //if the renderer is enabled(meaning its the correct car) 
                {

                    //if the car is active, get the meshFilter
                    color = ren.GetComponentInParent<MeshFilter>();
                    //find the mesh holder on the car
                    carData = ren.GetComponentInParent<CarData>(); //this is the active car

                    carData.carTires[0].SetActive(true);

                    break;

                }

                //Change the mesh filter

            }
        }
        void RemoveWheels()
        {
            for (int k = 0; k < carBody.transform.childCount; k++) //goes through each child object of carBody, AKA each car body
            {

                //search the carBody children and find the active car

                ren = carBody.transform.GetChild(k).gameObject.GetComponentInChildren<MeshRenderer>();//set ren = to the current child(i)
                if (ren.enabled == true) //if the renderer is enabled(meaning its the correct car) 
                {

                    //if the car is active, get the meshFilter
                    color = ren.GetComponentInParent<MeshFilter>();
                    //find the mesh holder on the car
                    carData = ren.GetComponentInParent<CarData>(); //this is the active car

                    for (int j = 0; j < carData.carTires.Length; j++)
                    {
                        Debug.Log(j);
                        carData.carTires[j].SetActive(false);
                    }

                    break;

                }

                //Change the mesh filter

            }
        }

    }

    void RotateCarBodyColor(int intVal)
    {

        GameObject carBody = GameObject.Find("CarBody");

        MeshRenderer ren;

        //CarData carData;


        for (int i = 0; i < carBody.transform.childCount; i++) //goes through each child object of carBody, AKA each car body
        {

            //search the carBody children and find the active car

            ren = carBody.transform.GetChild(i).gameObject.GetComponentInChildren<MeshRenderer>();//set ren = to the current child(i)
            if (ren.enabled == true) //if the renderer is enabled(meaning its the correct car) 
            {

                //if the car is active, get the meshFilter
                color = ren.GetComponentInParent<MeshFilter>();
                //find the mesh holder on the car
                carData = ren.GetComponentInParent<CarData>();

                carColorCount += intVal;

                if (carColorCount >= carData.carColors.Length)
                {
                    carColorCount = 0;
                }
                if (carColorCount < 0)
                {
                    carColorCount = carData.carColors.Length - 1; //-1 so it matches the index range that starts at 0
                }
                Debug.Log(carColorCount);
                Debug.Log("car index is "+carData.carColors.Length);
                color.mesh = carData.carColors[carColorCount];
                //set the mesh = to the carColorCount which is equal to the parameter 'intval'
                //Debug.Log("Car color is " + carData.carColors[carColorCount].name);
                //Debug.Log(carData.gameObject.name);
                break;
                
            }

            //Change the mesh filter
            
        }
    }

    void RotateCarTires(int intVal)
    {

        //find car

        GameObject carBody = GameObject.Find("CarBody");

        MeshRenderer ren;

        //CarData carData;

        
        int j = 0;
        for (int k = 0; k < carBody.transform.childCount; k++) //goes through each child object of carBody, AKA each car body
        {

            //search the carBody children and find the active car

            ren = carBody.transform.GetChild(k).gameObject.GetComponentInChildren<MeshRenderer>();//set ren = to the current child(i)
            if (ren.enabled == true) //if the renderer is enabled(meaning its the correct car) 
            {

                //if the car is active, get the meshFilter
                color = ren.GetComponentInParent<MeshFilter>();
                //find the mesh holder on the car
                carData = ren.GetComponentInParent<CarData>();


                foreach (GameObject tire in carData.carTires)
                {
                    //disable all meshes
                    carData.carTires[j].SetActive(false);
                    j++;
                }
                tireCount += intVal;
                if (tireCount >= carData.carTires.Length)
                {
                    tireCount = 0;
                }
                else if (tireCount < 0)
                {
                    tireCount = carData.carTires.Length - 1; //-1 so it matches the index range that starts at 0

                }
                carData.carTires[tireCount].SetActive(true);

                break;

            }

            //Change the mesh filter

        }


        
       
        
        //enable the currently selected mesh


       
        
    }

    public void ControlCCPanel(string ccCarPartName, int intVal) //references car part to change, 1 to move forward through the list, -1 to move backwards
    {
        if(ccCarPartName == "body")
        {
            RotateCarBodyMesh(intVal);
            //control the body
        }
        if (ccCarPartName == "bodyColor")
        {
            //control the body Color
            RotateCarBodyColor(intVal);
        }
        if (ccCarPartName == "tires")
        {
            RotateCarTires(intVal);
            //control the tires
        }
        if(ccCarPartName == "null")
        {
            RotateCarBodyMesh(0);
        }

    }

    void ccControler(int intVal)
    {
      

        /*//change menu selection
        int i = 0;
        foreach (Image image in textImageBackgrounds)
        {
            //disable all meshes
            textImageBackgrounds[i].enabled = false;
            i++;
        }*/
         
        //enable the currently selected mesh

        //imageCount+=intVal;
       /* if (imageCount >= textImageBackgrounds.Length)
        {
            imageCount = 0;
        }
        if(imageCount< 0)
        {
            imageCount = textImageBackgrounds.Length - 1;
        }

        textImageBackgrounds[imageCount].enabled = true;*/

        
    }
    void LoadColor()
    {
        GameObject carBody = GameObject.Find("CarBody");

        MeshRenderer ren;

        CarData carData;


        for (int i = 0; i < carBody.transform.childCount; i++) //goes through each child object of carBody, AKA each car body
        {

            //search the carBody children and find the active car

            ren = carBody.transform.GetChild(i).gameObject.GetComponentInChildren<MeshRenderer>();//set ren = to the current child(i)
            if (ren.enabled == true) //if the renderer is enabled(meaning its the correct car) 
            {

                //if the car is active, get the meshFilter
                color = ren.GetComponentInParent<MeshFilter>();
                //find the mesh holder on the car
                carData = ren.GetComponentInParent<CarData>();

                carColorCount = carCCColor;

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

}
