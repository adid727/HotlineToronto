using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Button startText;
    public Button exitText;
    AudioSource audio;
    // Use this for initialization
    void Start ()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        quitMenu.enabled = false;
        audio = GetComponent<AudioSource>();
    }

    public void ExitPress() 
    {
        audio.Play();
        quitMenu.enabled = true; 
        startText.enabled = false; 
        exitText.enabled = false;
    }

    public void NoPress() 
    {
        quitMenu.enabled = false; 
        startText.enabled = true; 
        exitText.enabled = true;
    }

    public void StartLevel() 
    {
        audio.Play();
        SceneManager.LoadScene("Game"); 
    }

    public void ExitGame() 
    {
        //audio.Play();
        Debug.Log("QUIT!!!");
        Application.Quit(); 
    }
}
