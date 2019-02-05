using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    public GameObject shopIntersection;
    public int shopIndex;

    private int numTrucks;
    private int garageSize;
    private GameObject[] shopTrucks = new GameObject[100];
    private Queue<GameObject> readyTrucksQueue = new Queue<GameObject>();

    void Start()
    {
        garageSize = 2;
    }

    void Update()
    {

    }

    public void AddTruck(GameObject Truck)
    {

    }

    public bool TruckReady()
    {
        return readyTrucksQueue.Dequeue() != null;
    }
}
