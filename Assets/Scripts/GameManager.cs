using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("PlayerDeath")]
    public Animation canvas_anim;
    public Material static_mat;
    public float static_line_closerate = 0.2f;



    public void PlayerDeath(){
        StartCoroutine(StaticClose(static_line_closerate));
        canvas_anim.Play("DeathScreen");
        player.GetComponentInChildren<MouseScript>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void ArtifactScanned(){
        articfacts_scanned++;
        artifact_scanned_text.text = $"Artifacts Scanned : {articfacts_scanned}";
    }

    IEnumerator StaticClose(float rate){
        Debug.Log(static_mat.HasFloat("_static_lines "));
        while (static_mat.GetFloat("_static_lines") > 1){
            static_mat.SetFloat("_static_lines", static_mat.GetFloat("_static_lines") - rate);
            yield return new WaitForSeconds(0.05f);
        }
        static_mat.SetFloat("_static_lines", 1f);
    }
}
