using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance{
        get{
            if (instance == null){
                Debug.LogError("GameManager not found");
            }

            return instance;
        }
    }

    private void Awake(){
        instance = this;

        static_mat.SetFloat("_static_lines", 30f);
    }

    public GameObject player;
    public int articfacts_scanned = 0;

    [Header("UI elements")]
    public Text artifact_scanned_text;
    public GameObject PauseMenu;
    bool isPaused = false;

    [Header("PlayerDeath")]
    public Animation canvas_anim;
    public Material static_mat;
    public float static_line_closerate = 0.2f;
    public AudioSource playerDeath;
    public AudioSource thud_sfx;



    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            onPause();
        }
    }

    public void PlayerDeath(){
        StartCoroutine(StaticClose(static_line_closerate));
        canvas_anim.Play("DeathScreen");
        thud_sfx.Play();
        player.GetComponentInChildren<MouseScript>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void ArtifactScanned(){
        articfacts_scanned++;
        artifact_scanned_text.text = $"Artifacts Scanned : {articfacts_scanned}";
        if (articfacts_scanned >= 5){
            GameWin();
        }
    }

    public void GameWin(){
        Debug.Log("You won");
    }

    public void onPause(){
        isPaused = !isPaused;
        Cursor.lockState = !isPaused ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = isPaused;
        PauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }

    public void MainMenu(){
        StartCoroutine(StaticClose(static_line_closerate));
        canvas_anim.Play("WinScreen");
        player.GetComponentInChildren<MouseScript>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    IEnumerator StaticClose(float rate){
        Debug.Log(static_mat.HasFloat("_static_lines "));
        while (static_mat.GetFloat("_static_lines") > 1){
            static_mat.SetFloat("_static_lines", static_mat.GetFloat("_static_lines") - rate);
            playerDeath.volume += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        static_mat.SetFloat("_static_lines", 1f);
    }
}
