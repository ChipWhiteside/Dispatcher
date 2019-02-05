using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour {

    public Sprite check;
    public Sprite noCheck;

    public GameObject gameMan;
    public GameMan gameManScript;

    public GameObject musicCheck;
    public GameObject SFXCheck;
    public Button musicSliderOn;
    public Button musicSliderOff;
    public Button SFXSliderOn;
    public Button SFXSliderOff;
    public Slider volumeSlider;

    bool music;
    bool sfx;
    int volume;

    // Use this for initialization
	void Start () {
        gameMan = GameObject.FindGameObjectsWithTag("GameManager")[0];
        gameManScript = gameMan.GetComponent<GameMan>();

        //sets volume synced with dbmanager data
        volume = DBManager.volume;

        //sets volume bar to dbman's volume value
        volumeSlider.value = volume;
    }

    // Update is called once per frame
    void Update () {
        music = (DBManager.music == 1);
        sfx = (DBManager.sfx == 1);

        //keeps music check synced with dbman's music value
        if (music)
        {
            musicCheck.GetComponent<Image>().sprite = check;
            musicSliderOn.gameObject.SetActive(true);
            musicSliderOff.gameObject.SetActive(false);
        }
        else
        {
            musicCheck.GetComponent<Image>().sprite = noCheck;
            musicSliderOn.gameObject.SetActive(false);
            musicSliderOff.gameObject.SetActive(true);
        }
        //keeps sfx check synced with dbman's sfx value
        if (sfx)
        {
            SFXCheck.GetComponent<Image>().sprite = check;
            SFXSliderOn.gameObject.SetActive(true);
            SFXSliderOff.gameObject.SetActive(false);
        }
        else
        {
            SFXCheck.GetComponent<Image>().sprite = noCheck;
            SFXSliderOn.gameObject.SetActive(false);
            SFXSliderOff.gameObject.SetActive(true);
        }
    }

    public void CloseSettings() {
        //saves the current settings
        music = musicSliderOn.IsActive();
        sfx = SFXSliderOn.IsActive();

        //update db settings table
        SetDBSettings();

        gameManScript.SetButtonsInteractable();

        //unloads the settings scene
        SceneManager.UnloadSceneAsync("Settings");
    }

    /*
     * Values to set:
     * Sound
     * Sfx
     * Volume    
     */
    public void SetDBSettings()
    {
        StartCoroutine(SetSettings());
    }

    IEnumerator SetSettings()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("music", DBManager.music);
        form.AddField("sfx", DBManager.sfx);
        form.AddField("volume", DBManager.volume);
        
        WWW www = new WWW("http://localhost:8888/dispatcherphps/setsettings.php", form);
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

    public void SoundOn()
    {
        DBManager.music = 1;
    }

    public void SoundOff()
    {
        DBManager.music = 0;
    }

    public void SfxOn()
    {
        DBManager.sfx = 1;
    }

    public void SfxOff()
    {
        DBManager.sfx = 0;
    }

    public void VolUpdated()
    {
        DBManager.volume = (int) volumeSlider.value;
    }

    public void LogOut()
    {
        DBManager.LogOut();
        SceneManager.LoadScene("Login");
    }

    public void QuitPressed()
    {
        CloseSettings();
        gameManScript.DisplayQuitPopupDialogue();
    }
}
