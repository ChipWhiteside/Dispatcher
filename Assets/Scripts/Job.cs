using UnityEngine;

public class Job
{
    //for db id, distance, date, time

    //jobs have: a delivery location, amount of goods, price, truck
    private int jobID;
    private GameObject deliveryLocation;
    public string date; //ex: 2018-01-01
    public string time; //1430
    public string distance;
    private GameObject[] pathPoints;
    private int amountGoods;
    private int amountEarned;
    private int amountCost;
    private GameObject truckAssigned;
    private Vector3 storeStartingFrom;
    private int v1;
    private int v2;
    private double v3;
    private double v4;
    private Truck jobTruck;
    private Transform transform;
    private Vector3 position1;
    private Vector3 position2;

    public Job(int jID, GameObject deliveryLoc, GameObject[] path, int aGoods, int aEarned, int aCost, Vector3 storeStart)
    {
        jobID = jID;
        deliveryLocation = deliveryLoc;
        pathPoints = path;
        amountGoods = aGoods;
        amountEarned = aEarned;
        amountCost = aCost;
        storeStartingFrom = storeStart;
        SetDateTime();
        distance = Vector3.Distance(storeStart, deliveryLoc.transform.position).ToString();
    }

    //public Job(int v1, Vector3 position1, int v2, double v3, double v4, Truck jobTruck, Vector3 position2)
    //{
    //    this.v1 = v1;
    //    this.position1 = position1;
    //    this.v2 = v2;
    //    this.v3 = v3;
    //    this.v4 = v4;
    //    this.jobTruck = jobTruck;
    //    this.position2 = position2;
    //}

    
    public void SetDateTime()
    {
        string datetime = System.DateTime.Now.ToString();
        string sepDate = datetime.Substring(0, 10); // 12/11/2018 - 2018-01-01
        string sepTime = datetime.Substring(11, 5); // 22:39
        date = sepDate.Substring(6) + "-" + sepDate.Substring(0, 2) + "-" + sepDate.Substring(3, 2);
        time = sepTime.Substring(0, 2) + sepTime.Substring(3);
    }

    /*
     * Sets the job ID
     */
    public void SetJobID(int jID)
    {
        jobID = jID;
    }

    /*
     * Returns the job ID
     */
    public int GetJobID()
    {
        return jobID;
    }


    /*
     * Returns the delivery location
     */
    public void SetDeliveryLocation(GameObject newLocation)
    {
        deliveryLocation = newLocation;
    }

    /*
     * Returns the delivery location
     */
    public GameObject GetDeliveryLocation()
    {
        return deliveryLocation;
    }


    /*
     * Sets the amount of goods transported
     */
    public void SetAmountGoods(int newAGoods)
    {
        amountGoods = newAGoods;
    }

    /*
     * Returns the amount of goods transported
     */
    public int GetAmountGoods()
    {
        return amountGoods;
    }


    /*
     * Sets the amount this job earns
     */
    public void SetAmountEarned(int newAEarned)
    {
        amountEarned = newAEarned;
    }

    /*
     * Returns the amount this job earns
     */
    public int GetAmountEarned()
    {
        return amountEarned;
    }


    /*
     * Sets the amount this job costs
     */
    public void SetAmountCost(int newACost)
    {
        amountCost = newACost;
    }

    /*
     * Returns the amount that the job costs
     */
    public int GetAmountCost()
    {
        return amountCost;
    }


    /*
     * Sets the truck assigned to the job
     */
    public void SetTruckAssigned(GameObject newTruck)
    {
        truckAssigned = newTruck;
    }

    /*
     * Returns the truck assigned to the job
     */
    public GameObject GetTruckAssigned()
    {
        return truckAssigned;
    }


    /*
     * Sets the store location where the truck will leave from and return to
     */
    public void SetStoreStartingFrom(Vector3 newStore)
    {
        storeStartingFrom = newStore;
    }

    /*
     * Returns the store location where the truck will leave from and return to
     */
    public Vector3 GetStoreStartingFrom()
    {
        return storeStartingFrom;
    }

    /*
     * Returns the points along the path from the store to the delivery location
     */
    public void SetPathPoints(GameObject[] path)
    {
        pathPoints = path;
    }

    /*
     * Returns the points along the path from the store to the delivery location
     */
    public GameObject[] GetPathPoints()
    {
        return pathPoints;
    }

    public string PrintJob()
    {
        return ("jID: " + jobID + " delLoc: " + deliveryLocation.transform.position + " aGoods: " + amountGoods + " aEarned: " + amountEarned + " aCost: " + amountCost + " storeStart: " + storeStartingFrom + " truckAssigned: " + truckAssigned);
    }
}
