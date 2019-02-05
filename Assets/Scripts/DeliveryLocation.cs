using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLocation : MonoBehaviour {

    public GameObject[] inters = new GameObject[2];
    public GameObject gameMan;
    public GameMan gameManScript;

    // Use this for initialization
    void Start()
    {
        /*
        gameMan = GameObject.FindGameObjectsWithTag("GameManager")[0];
        gameManScript = gameMan.GetComponent<GameMan>();

        GameObject[] inters = GameObject.FindGameObjectsWithTag("Intersection");

        float min1Dist = 0;
        int min1Index = 0;

        float min2Dist = 0;
        int min2Index = 1;
        for (int i = 0; i < inters.Length; i++)
        {
            float closestInterDist = Vector3.Distance(this.transform.position, inters[i].transform.position);
            if ()
            {

            }
        }
    
	*/
    }
	// Update is called once per frame
	void Update () {
		
	}
}
