using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SoundToyOneManager : MonoBehaviour {


    private void Start() {
        Debug.Log($"Starting Sound Toy One. Current time: {DateTime.UtcNow}");
    }

    void ReloadScene() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void Update() {
        ReloadScene();
    }
}
