using UnityEngine;
using System.Collections;

public class Intersection : MonoBehaviour
{

    public GameObject[] adjacentIntersections;
    public bool shopIntersection;

    void Start()
    {

    }

    void Update()
    {

    }

    public GameObject FindNextIntersection(GameObject startingPoint, GameObject targetLocation)
    {
        //Check if there are no adjacents
        if (adjacentIntersections.Length < 1)
            return null;

        //Set the default minimum distance to the first intersection in the array
        GameObject closestInter = adjacentIntersections[0];
        float closestInterDist = Vector3.Distance(targetLocation.transform.position, adjacentIntersections[0].transform.position);

        //For each element in the adjacentIntersections array, check if its closer to the target than the current min
        for (int i = 1; i < adjacentIntersections.Length; i++)
        {
            float dist = Vector3.Distance(targetLocation.transform.position, adjacentIntersections[i].transform.position);
            if (dist < closestInterDist)
            {
                closestInter = adjacentIntersections[i];
                closestInterDist = dist;
            }
        }

        if (!shopIntersection)
        {
            //Check if the distance from the current intersection to the target is smaller than all the adjacents, if so return the target
            float interTargetDist = Vector3.Distance(targetLocation.transform.position, transform.position);
            if (interTargetDist < closestInterDist)
                return targetLocation;
        }
        return closestInter;
    }
}
