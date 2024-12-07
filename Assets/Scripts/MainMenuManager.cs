using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject Main;
    public GameObject Manual;
    public GameObject[] Manual_pages;
    
    int page_counter = 0;
    void Start(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void PlayPressed(){
        SceneManager.LoadScene(1);
    }

    public void QuitPressed(){
        Application.Quit();
    }

    public void ManualPressed(){
        Main.SetActive(false);
        Manual.SetActive(true);
    }

    public void NextPressed(){
        Manual_pages[page_counter].SetActive(false);
        page_counter++;
        if (page_counter < Manual_pages.Length){
            Manual_pages[page_counter].SetActive(true);
        } else {
            Manual_pages[0].SetActive(true);
            Manual.SetActive(false);
            Main.SetActive(true);
            page_counter = 0;
        }
    }
    public void PrevPressed(){
        Manual_pages[page_counter].SetActive(false);
        page_counter--;
        if (page_counter > 0){
            Manual_pages[page_counter].SetActive(true);
        } else {
            Manual_pages[0].SetActive(true);
            Manual.SetActive(false);
            Main.SetActive(true);
            page_counter = 0;
        }
    }
}
