using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMan : MonoBehaviour
{
    public Camera mainC;
    public GameObject truckModel;
    public GameObject[] testLocations;
    public Text moneyTextBox;
    public Text trucksTextBox; 
    public Text tspeedTextBox;
    public Text tcarryingCapTextBox;
    public GameObject popupDialogue;
    public Text popupText;
    public Text popupNote;
    public GameObject popupImage;
    public Sprite badPopupSprite;
    public Sprite goodPopupSprite;
    public GameObject quitPopup;

    public InputField truckNameBox;
    public GameObject nameTruckDialogue;
    public Button nameTruckButton;

    public Button SettingsB;
    public Button ShopB;
    public Button NewTruckB;
    public Button DeliveriesB;
    public Button NewJobB;

    private int jobID;
    private int numJobs;
    private GameObject[] ownedShops = new GameObject[3];

    //array to hold the unlocked trucks
    GameObject[] trucksArray = new GameObject[100];

    //queue to hold the trucks that are ready to be deployed
    Queue<GameObject> readyTrucksQueue = new Queue<GameObject>();

    //array to hold all the active shops
    GameObject[] shops = new GameObject[100];

    //list to hold all the current jobs
    LinkedList<Job> jobList = new LinkedList<Job>();

    //list to hold the completed jobs
    LinkedList<Job> completedJobs = new LinkedList<Job>();


    //queue of current jobs
    //array of completed jobs
    //array of failed jobs?


    //Text to display the result on
    //private Text statusText;

    void Start()
    {
        DBManager.gameplayCamera = mainC;

        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(mainC);

        //fill jobList and completedJobs from database
        numJobs = jobList.Count + completedJobs.Count;

        //fill array with all the active shops
        shops = GameObject.FindGameObjectsWithTag("Shop");
        ownedShops[0] = shops[0]; //unlocks the first shop
        //update the UI from db
        GetDBValues();
    }

    void Update()
    {
        moneyTextBox.text = "$" + DBManager.money.ToString();
        trucksTextBox.text = readyTrucksQueue.Count.ToString() + "/" + DBManager.numTrucks.ToString();
        tspeedTextBox.text = DBManager.truckSpeed.ToString() + "mph";
        tcarryingCapTextBox.text = (DBManager.truckCarryingCapacity * 10).ToString() + "lbs.";
    }

    public void AddTrucks(int numt)
    {
        for (int i = 0; i < numt; i++)
        {
            GameObject newTruck = Instantiate(truckModel); //create the truck from the truck model
            newTruck.GetComponent<Truck>().driverName = "George";
            newTruck.GetComponent<Truck>().SetSpeed(DBManager.truckSpeed);
            print("newTruck: " + newTruck.GetComponent<Truck>().driverName);
            trucksArray[i] = newTruck;
            readyTrucksQueue.Enqueue(newTruck);
        }
    }

    /*
     * Values to get:
     * Money
     * Number of trucks
     * the unlocked stores    
     */
    public void GetDBValues()
    {
        StartCoroutine(GetInfo());
    }

    IEnumerator GetInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);

        WWW www = new WWW("http://localhost:8888/dispatcherphps/getinfo.php", form);
        yield return www;

        if (www.text[0] == '0')
        {

            print("dbmoney: " + int.Parse(www.text.Split('\t')[1]));
            print("dbshops: " + int.Parse(www.text.Split('\t')[2]));
            print("dbnumtrucks: " + int.Parse(www.text.Split('\t')[3]));


            DBManager.money = int.Parse(www.text.Split('\t')[1]);
            DBManager.shops = int.Parse(www.text.Split('\t')[2]);
            DBManager.numTrucks = int.Parse(www.text.Split('\t')[3]);
            DBManager.truckSpeed = int.Parse(www.text.Split('\t')[4]);
            DBManager.truckCarryingCapacity = int.Parse(www.text.Split('\t')[5]);
            DBManager.music = int.Parse(www.text.Split('\t')[6]);
            DBManager.sfx = int.Parse(www.text.Split('\t')[7]);
            DBManager.volume = int.Parse(www.text.Split('\t')[8]);

            print("DEBUG -- sound: " + DBManager.music + " sfx: " + DBManager.sfx + " volume: " + DBManager.volume);


            AddTrucks(DBManager.numTrucks);
            
            print("money: " + DBManager.money + " shops: " + DBManager.shops + " numtrucks: " + DBManager.numTrucks + " truckspeed: " + DBManager.truckSpeed + " truckcarryingcapacity: " + DBManager.truckCarryingCapacity);
        }
        else
        {
            Debug.Log("Get info failed: Error #" + www.text);
        }
    }

    public void CallUpdatePlayer()
    {
        StartCoroutine(UpdatePlayer());
    }

    IEnumerator UpdatePlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("money", DBManager.money);
        form.AddField("shops", DBManager.shops);
        form.AddField("numtrucks", DBManager.numTrucks);
        form.AddField("truckspeed", DBManager.truckSpeed);
        form.AddField("truckcarryingcapacity", DBManager.truckCarryingCapacity);

        WWW www = new WWW("http://localhost:8888/dispatcherphps/updateplayer.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("User updated successfully.");
        }
        else
        {
            Debug.Log("User update failed. Error #" + www.text);
        }
    }

    public void CallAddDelivery(Job job)
    {
        StartCoroutine(AddDelivery(job));
    }

    IEnumerator AddDelivery(Job job)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("distance", job.distance);
        form.AddField("date", job.date);
        form.AddField("time", int.Parse(job.time));

        print("name: " + DBManager.username + " distance: " + job.distance.Trim() + " date: " + job.date.Trim() + "time: " + int.Parse(job.time));

        WWW www = new WWW("http://localhost:8888/dispatcherphps/adddelivery.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("Delivery added successfully.");
        }
        else
        {
            Debug.Log("Delivery addition failed. Error #" + www.text);
        }
    }

    public void NewJob()
    {
        //pick a random delivery location
        GameObject[] dlocations = GameObject.FindGameObjectsWithTag("DeliveryLocation");
        int index = Random.Range(0, dlocations.Length);
        GameObject deliveryLocation = dlocations[index];

        //find the closest shop for the job
        GameObject jobShop = FindClosest(deliveryLocation);

        //build the path
        GameObject[] path = BuildPath(jobShop, deliveryLocation);

        //set amount earned for the job
        float dist = Vector3.Distance(jobShop.transform.position, deliveryLocation.transform.position);
        int amountEarned = Mathf.RoundToInt(dist + (50 * DBManager.truckCarryingCapacity));
        print("dist: " + dist + " amountEarned: " + amountEarned);

        //create the job
        //int jID, Vector3 delLoc, int aGoods, float aEarned, float aCost, Truck tAssigned, Vector3 storeStart
        Job newJob = new Job(++numJobs, deliveryLocation, path, 1, amountEarned, 1, jobShop.transform.position);

        print("Date: " + newJob.date + " Time: " + newJob.time);
        //add job to the job list
        jobList.AddLast(newJob);

        JobAccepted(newJob);
    }

    public GameObject[] BuildPath(GameObject jobShop, GameObject deliveryLocation)
    {
        //find shortest path from shop to delivery location
        GameObject[] pathIntersections = new GameObject[50];
        GameObject startingPoint = jobShop.GetComponent<Shop>().shopIntersection;
        pathIntersections[0] = startingPoint;
        int pathlength = 1; //number of intersections in the path
        for (int i = 1; i < pathIntersections.Length; i++)
        {
            GameObject nextIntersection = pathIntersections[i - 1].GetComponent<Intersection>().FindNextIntersection(jobShop, deliveryLocation);

            if (nextIntersection == deliveryLocation)
            {
                //path is complete
                pathIntersections[i] = deliveryLocation;
                pathlength++;

                int aIndex = i;
                for (int a = i-1; a >= 0; a--)
                {
                    pathIntersections[++aIndex] = pathIntersections[a];
                }


                break;
            }
            else
            {
                pathIntersections[i] = nextIntersection;
                pathlength++;
            }


            if (i == pathIntersections.Length - 1)
            {
                Debug.Log("Error #8: Path length reached 50");
                break;
            }
        }

        //PrintPath(pathIntersections);
        return pathIntersections;
    }

    public void JobAccepted(Job job)
    {
        AssignJob(job);
    }

    public void AssignJob(Job job)
    {
        if (readyTrucksQueue.Count > 0) {
            //find next availible truck
            GameObject nextTruck = readyTrucksQueue.Dequeue();
            if (nextTruck == null)
            {
                Debug.Log("Error #7: No availible truck");
                //queue job
            }

            job.SetTruckAssigned(nextTruck);
            nextTruck.GetComponent<Truck>().LaunchTruck(job);

            CallAddDelivery(job);
            print("Player updated");

            //start timer
        }
        else
        {
            DisplayPopupDialogue(false, "Oops!", "All your trucks are already on deliveries!");
        }
    }

    public GameObject FindNextTruck()
    {
        return readyTrucksQueue.Dequeue();
    }

    public GameObject FindClosest(GameObject delLoca)
    {
        if (shops.Length < 1) //checks for empty shops array
            return null;

        LinkedList<GameObject> closestShops = new LinkedList<GameObject>();
        closestShops.AddFirst(shops[0]); //adds the first shop to the first of the list

        GameObject min = shops[0]; //sets the first shop in the array to the closest automatically
        int minindex = 0;
        float minDist = Vector3.Distance(delLoca.transform.position, shops[0].transform.position);
        for (int i = 1; i < shops.Length; i++)
        {
            float dist = Vector3.Distance(delLoca.transform.position, shops[i].transform.position);
            if (dist < minDist)
            {
                min = shops[i];
                closestShops.AddFirst(shops[i]);
                minindex = i;
                minDist = dist;
            }
            else
            {
                closestShops.AddAfter(closestShops.First, shops[i]);
            }
        }
        return min;
    }

    public void VeryifyTruckInput()
    {
        nameTruckButton.interactable = (truckNameBox.text.Length >= 0);
    }
    /*
    public void AddTruckClicked()
    {
        if (DBManager.money >= 200)
            nameTruckDialogue.SetActive(true);
        else
            DisplayPopupDialogue(false, "Oops!", "You don't have enough money to buy a truck! You need $200, you have $" + DBManager.money);
    }
    */
    public void AddNewTruck()
    {
        nameTruckDialogue.SetActive(false);
        //Truck newTruckScript = new Truck(); //creates a new truck script
        //newTruckScript.gameMan = this.gameObject; //sets the trucks game manager to the game manager this script is attached to

        GameObject newTruck = Object.Instantiate(truckModel); //create the truck from the truck model
        newTruck.GetComponent<Truck>().driverName = "George";
        print("newTruck: " + newTruck.GetComponent<Truck>().driverName);
        trucksArray[DBManager.numTrucks] = newTruck;
        DBManager.numTrucks++;
        readyTrucksQueue.Enqueue(newTruck);
        CallUpdatePlayer();
        DisplayPopupDialogue(true, "Yay!", "You bought a new truck! You own " + DBManager.numTrucks + " trucks now!");
    }

    public void UpgradeTruckSpeed()
    {
        if (DBManager.truckSpeed != 12) {
            DBManager.truckSpeed = 12;
            for (int i = 0; i < DBManager.numTrucks; i++)
            {
                trucksArray[i].GetComponent<Truck>().SetSpeed(DBManager.truckSpeed);
            }
            CallUpdatePlayer();
            DisplayPopupDialogue(true, "Yay!", "You upgraded your trucks! Their speed has increased by 50%!");
        }
        else
            DisplayPopupDialogue(false, "Oops!", "Your trucks are already upgraded!");
    }

    public void UpgradeTruckCarryingCapacity()
    {
        DBManager.truckCarryingCapacity++;
        for (int i = 0; i < DBManager.numTrucks; i++)
        {
            trucksArray[i].GetComponent<Truck>().SetSpeed(DBManager.truckSpeed);
        }
        CallUpdatePlayer();
        DisplayPopupDialogue(true, "Yay!", "You upgraded your trucks! They can carry " + (DBManager.truckCarryingCapacity * 10) + "lbs of cargo on deliveries now!");
}

    //public int GetNumTrucks()
    //{
    //    //get from db
    //    return numTrucks;
    //}

    //public int GetNumStores()
    //{
        ////get from db
        //return numStores;
    //}

    public void JobComplete(Job completeJob, GameObject deliveryTruck)
    {
        print("FINISHED!");

        //moves the truck underground
        deliveryTruck.transform.position = truckModel.transform.position;
        deliveryTruck.GetComponent<Truck>().DeliveryFinished();

        //add truck back go ready queue
        readyTrucksQueue.Enqueue(deliveryTruck);

        //move job from current list to completed list
        jobList.Remove(completeJob);
        completedJobs.AddLast(completeJob);

        //pay the player
        DBManager.money += completeJob.GetAmountEarned();

        //updates the db
        CallUpdatePlayer();

    }

    public void PrintPath(GameObject[] paths)
    {
        for(int i = 0; i < FindPathLength(paths); i++)
        {
            print("Index_" + i + ": " + paths[i].transform.position);
        }
    }

    public int FindPathLength(GameObject[] path)
    {
        int counter = 0;
        for (int i = 0; i < path.Length; i++)
        {
            if (path[i] == null)
                return i;
            //if (path[i] != null)
              //  counter++;
        }
        return counter;
    }

    /*
     * Loads the settings
     */
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
    }

    public void DisplayPopupDialogue(bool good, string note, string errormessage)
    {
        SetButtonsUninteractable();
        popupDialogue.SetActive(true);
        if (good)
            popupImage.GetComponent<Image>().sprite = goodPopupSprite;
        else
            popupImage.GetComponent<Image>().sprite = badPopupSprite;
        popupNote.text = note;
        popupText.text = errormessage;
    }

    public void ClosePopupDialogue()
    {
        SetButtonsInteractable();
        popupDialogue.SetActive(false);
    }

    public void DisplayQuitPopupDialogue()
    {
        SetButtonsUninteractable();
        quitPopup.SetActive(true);
    }

    public void CloseQuitPopupDialogue()
    {
        SetButtonsInteractable();
        quitPopup.SetActive(false);
    }

    public void SetButtonsUninteractable()
    {
        SettingsB.interactable = false;
        ShopB.interactable = false;
        NewTruckB.interactable = false;
        DeliveriesB.interactable = false;
        NewJobB.interactable = false;
    }

    public void SetButtonsInteractable()
    {
        SettingsB.interactable = true;
        ShopB.interactable = true;
        NewTruckB.interactable = true;
        DeliveriesB.interactable = true;
        NewJobB.interactable = true;
    }

    public void QuitGame()
    {
        CallUpdatePlayer();
        Application.Quit();
    }
}