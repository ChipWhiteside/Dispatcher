using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public Sprite tabActive;
    public Sprite tabInactive;
    public Canvas shopCanvas;

    public GameObject gameMan;
    public GameMan gameManScript;
    public GameObject allTab;
    public GameObject truckTab;
    public GameObject shopTab;

    public GameObject[] truckCards;
    public GameObject[] shopCards;

    void Start()
    {
        //shopCanvas.worldCamera = DBManager.gameplayCamera;
        gameMan = GameObject.FindGameObjectsWithTag("GameManager")[0];
        gameManScript = gameMan.GetComponent<GameMan>();
        truckCards = GameObject.FindGameObjectsWithTag("TruckCard");
        shopCards = GameObject.FindGameObjectsWithTag("ShopCard");
    }

    void Update()
    {

    }

    public void SetAllTabActive()
    {
        allTab.GetComponent<Image>().sprite = tabActive;
        truckTab.GetComponent<Image>().sprite = tabInactive;
        shopTab.GetComponent<Image>().sprite = tabInactive;

        foreach (GameObject card in truckCards)
        {
            card.SetActive(true);
        }
        foreach (GameObject card in shopCards)
        {
            card.SetActive(true);
        }
    }

    public void SetTruckTabActive()
    {
        truckTab.GetComponent<Image>().sprite = tabActive;
        allTab.GetComponent<Image>().sprite = tabInactive;
        shopTab.GetComponent<Image>().sprite = tabInactive;
        foreach (GameObject card in truckCards)
        {
            card.SetActive(true);
        }
        foreach (GameObject card in shopCards)
        {
            card.SetActive(false);
        }
    }

    public void SetShopTabActive()
    {
        shopTab.GetComponent<Image>().sprite = tabActive;
        allTab.GetComponent<Image>().sprite = tabInactive;
        truckTab.GetComponent<Image>().sprite = tabInactive;
        foreach (GameObject card in truckCards)
        {
            card.SetActive(false);
        }
        foreach (GameObject card in shopCards)
        {
            card.SetActive(true);

        }
    }

    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync("Shop");
    }

    public void PurchaseTruck()
    {
        if (DBManager.money >= 200)
        {
            DBManager.money -= 200;
            gameManScript.AddNewTruck();
            UnloadScene();
        }
        else
        {
            gameManScript.DisplayPopupDialogue(false, "Oops!", "Not enough money to buy a truck! You need $200!");
            UnloadScene();
        }
    }

    public void UpgradeTruckSpeed()
    {
        if (DBManager.money >= 250)
        {
            DBManager.money -= 250;
            gameManScript.UpgradeTruckSpeed();
            UnloadScene();
        }
        else
        {
            gameManScript.DisplayPopupDialogue(false, "Oops!", "Not enough money to upgrade your trucks! You need $250!");
            UnloadScene();
        }
    }

    public void UpgradeTruckCapacity()
    {
        if (DBManager.money >= 250)
        {
            DBManager.money -= 250;
            gameManScript.UpgradeTruckCarryingCapacity();
            UnloadScene();
        }
        else
        {
            gameManScript.DisplayPopupDialogue(false, "Oops!", "Not enough money to upgrade your trucks! You need $250!");
            UnloadScene();
        }
    }

    public void PurchaseShop()
    {
        if (DBManager.money >= 500)
        {
            //DBManager.money -= 500;
            //gameManScript.AddNewShop();
            UnloadScene();
        }
        else
        {
            gameManScript.DisplayPopupDialogue(false, "Oops!", "Not enough money to buy a shop! You need $50!");
            UnloadScene();
        }
    }

    public void UpgradeShopGarage()
    {
        if (DBManager.money >= 350)
        {
            //DBManager.money -= 350;
            //gameManScript.UpgradeShopSize();
            UnloadScene();
        }
        else
        {
            gameManScript.DisplayPopupDialogue(false, "Oops!", "Not enough money to upgrade your shop's garage! You need $350!");
            UnloadScene();
        }
    }

    public void ButtonsInteractable()
    {
        gameManScript.SetButtonsInteractable();
    }
}
