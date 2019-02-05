using System;
using UnityEngine;

public class Truck : MonoBehaviour
{
    /* Things a truck needs:
     * 
     * Speed
     * Carrying capacity
     * Store (that it is currently located at)
     * InTransit bool value (to keep track of which trucks are waiting to be deployed and which are in transit)
     * Job 
     */

    //db needed items:
    //truck
    public int truckID;
    public string truckModel;
    //driver
    public string driverName;


    public GameObject gameMan;

    private float speed;
    private int carryingCapacity;
    private int storeLocation;
    private bool inTransit;
    private GameObject[] truckPath;
    private int current;
    private Job currentJob;

    Vector3 printedLast = Vector3.zero;

    void Start()
    {
        printedLast = Vector3.zero;
    }

    void Update()
    {
        if (inTransit) {
            float dist = Vector3.Distance(transform.position, truckPath[current].transform.position);
            if (dist > .01)
            {
                if (printedLast != truckPath[current].transform.position) {
                    //print("Target_" + current + ": " + truckPath[current].transform.position);
                    printedLast = truckPath[current].transform.position;
                }

                Vector3 pos = Vector3.MoveTowards(transform.position, truckPath[current].transform.position, speed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(pos);
            }
            else
            {
                this.transform.position = truckPath[current].transform.position;

                if (current > 0 && truckPath[current] == truckPath[0])
                {
                    //reached the start again
                    inTransit = false;
                    gameMan.GetComponent<GameMan>().JobComplete(currentJob, this.gameObject);
                }

                current = (current + 1) % truckPath.Length;
                //print("Current: " + current);
            }
        }
    }

    public Truck()
    {
        speed = 7f;
        carryingCapacity = 1;
        storeLocation = 1;
        inTransit = false;
    }

    public void DeliveryFinished()
    {
        //clears the truck path
        for (int i = 0; i < truckPath.Length; i++) {
            truckPath[i] = null;
        }
        currentJob = null;
        current = 0;
    }

    public void AssignJob(Job newJob)
    {
        //do stuff
    }

    /*
     * Returns the speed of the truck
     */
    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    /*
     * Returns the carrying capacity of the truck
     */
    public int GetCarryingCapacity()
    {
        return carryingCapacity;
    }

    /*
     * Returns the store location of the truck
     */
    public int GetStoreLoc()
    {
        return storeLocation;
    }

    /*
     * Returns the boolean that keeps track of whether the truck is currently in transit or not
     */
    public bool GetInTransit()
    {
        return inTransit;
    }

    /*
     * Sets the boolean that keeps track of whether the truck is currently in transit or not
     */
    public void SetInTransit(bool isInTrans)
    {
        inTransit = isInTrans;
    }

    public void LaunchTruck(Job job)
    {
        currentJob = job;
        truckPath = job.GetPathPoints();
        this.transform.position = truckPath[0].transform.position; //sets the truck position to the shops start position
       //transform.position = new Vector3(transform.position.x, 4.2f, transform.position.z); //sets the y value to the same as the intersections
        inTransit = true;
    }
}