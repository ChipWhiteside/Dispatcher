using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {

    public InputField accountBox;
    public InputField passwordBox;
    public Text accountName;
    public GameObject rememberAccountBox;
    public GameObject checkmark;
    public GameObject loginPanel;
    public GameObject saveFiles;
    public Button registerButton;
    public Button loginButton;
    public GameObject quitPopup;
    public Text errorMessage;

    public Button quitB;
    public Button loginB;
    public Button registerB;

    private bool fading;

    //bool rememberAccount;
    //string lastUsedAccount;

	// Use this for initialization
	void Start () {
        //rememberAccount = false;
        checkmark.SetActive(false);
        rememberAccountBox.GetComponent<Toggle>().isOn = false;
        errorMessage.text = "";
        fading = false;
    }
	
	// Update is called once per frame
	void Update () {
        //If the toggle returns true, fade in the Image
        if (fading == true)
        {
            //Fully fade in text with the duration of .5
            errorMessage.CrossFadeAlpha(1, 0.0000001f, false);
        }
        //If the toggle is false, fade out to nothing the text with a duration of 5
        if (fading == false)
        {
            errorMessage.CrossFadeAlpha(0, 0.4f, false);
        }
    }

    public void LoginPressed()
    {
        string user = accountBox.text;
        string pass = passwordBox.text;
       
        print("User: " + user + " | Pass: " + pass);
       
        if (ValidateCredentials(user, pass) == 0)
        { //if account and password are valid and registered
            SetLastUsername(user);
            GameObject.Find("Account_Name").GetComponent<Text>().text = "Welcome, " + user;
            LoadSaves();
        }
    }


    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", accountBox.text);
        form.AddField("password", passwordBox.text);

        WWW www = new WWW("http://localhost:8888/dispatcherphps/register.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("User account created successfully.");
            DBManager.username = accountBox.text;
            LoadGame();
        }
        else
        {
            Debug.Log("User account creation failed. Error #" + www.text);
            DisplayErrorMessage("Uh oh! " + www.text.Substring(3));
        }
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", accountBox.text);
        form.AddField("password", passwordBox.text);

        WWW www = new WWW("http://localhost:8888/dispatcherphps/login.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            DBManager.username = accountBox.text;
            /*          
            DBManager.money =int.Parse(www.text.Split('\t')[1]);
            DBManager.shops = int.Parse(www.text.Split('\t')[2]);
            DBManager.numTrucks = int.Parse(www.text.Split('\t')[3]);

            print("money: " + DBManager.money + " shops: " + DBManager.shops + " numtrucks: " + int.Parse(www.text.Split('\t')[3]));
            */
            accountName.text = "Hello! " + DBManager.username;
            LoadGame();
        }
        else
        {
            Debug.Log("User login failed: Error #" + www.text);
            DisplayErrorMessage("Uh oh! " + www.text.Substring(3));
        }
    }

    public void VeryifyInputs() 
    {
        registerButton.interactable = (accountBox.text.Length >= 8 && passwordBox.text.Length >= 8);
        loginButton.interactable = (accountBox.text.Length >= 8 && passwordBox.text.Length >= 8);
    }


    IEnumerator TestRequest()
    {
        WWW request = new WWW("http://localhost:8888/dispatcherphps/##########.php");
        yield return request;
        string[] requestResult = request.text.Split('\t');
        foreach(string s in requestResult)
        {
            Debug.Log(s);
        }
    }



    /*
     * Checks the validity of the username and password when the login button is clicked;
     */
    private int ValidateCredentials(string u, string p) {
        if (u == "")
        { //if the account field is empty return false
            return 1;
        }
        else if (p == "")
        { //if the password field is empty return false
            return 2;
        }
        else {
            //check if the username exists
        }
        print("Validated!");
        return 0;
    }

    /*
     * Loads the "SaveFile" scene
     */
    public void LoadSaves()
    {
        loginPanel.SetActive(false);
        saveFiles.SetActive(true);
        print("Loaded saves");
        //SceneManager.LoadScene("SaveFiles" , LoadSceneMode.Additive);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    /*
     * Handles when the user clickes the "remember username" button
     */
    public void RememberAccountClicked() {
        bool isActive = rememberAccountBox.GetComponent<Toggle>().isOn;
        //rememberAccount = isActive;
        checkmark.SetActive(isActive); //activates or deactivates the checkmark based on the toggle's bool value
    }

    /*
     * Sets the lastUsername to the string that was last used to log into the game if the "remember account" button was clicked.
     */
    public void SetLastUsername(string newUsername) {
        //lastUsedAccount = newUsername;
    }

    /*
     * Loads the settings
     */
    public void OpenSettings() {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void OpenQuitPopup()
    {
        SetButtonsUninteractable();
        quitPopup.gameObject.SetActive(true);
    }

    public void CloseQuitPopup()
    {
        SetButtonsInteractable();
        quitPopup.gameObject.SetActive(false);
    }

    public void Quit()
    {
        CloseQuitPopup();
        Application.Quit();
    }

    public void SetButtonsUninteractable()
    {
        quitB.interactable = false;
        loginB.interactable = false;
        registerB.interactable = false;
    }

    public void SetButtonsInteractable()
    {
        quitB.interactable = true;
        VeryifyInputs();
    }

    public void DisplayErrorMessage(string error)
    {
        errorMessage.text = error;
        //set the alpha to 0
        DisplayError();
    }

    public void DisplayError() 
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fading = true; //fade in
        yield return new WaitForSeconds(5); 
        fading = false; //fade out
    }

    public void StartFadingOut()
    {

    }

}
