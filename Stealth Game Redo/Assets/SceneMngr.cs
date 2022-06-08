using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMngr : MonoBehaviour
{
    public GameObject endGameScreenUI;

    public void Start() {
        endGameScreenUI.SetActive(false);
    }

    public static void reloadLevel() {
        SceneManager.LoadScene(1);
    }

    public void CompleteLevel() {
        Debug.Log("show end level");
        endGameScreenUI.SetActive(true); // enable UI
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            CompleteLevel();
        }    
    }
}