using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;


//Part I: Spawning ball drop on click/keyboard press
//Part II: Spawning ball on beat using clock
    
public class BallSpawner : MonoBehaviour {

    [SerializeField]
    Clock clock;
    Args beatArgs;
    public Transform ball;

    bool pleaseSpawn;

    [SerializeField]
    int BPM = 120;

    #region Delegates
    private void OnEnable() {
        clock.Beat += SpawnBall;
    }
    private void OnDisable() {
        clock.Beat -= SpawnBall;
    }
    #endregion

    void SpawnBall(Beat.Args beatArgs) {
        pleaseSpawn = true;
    }
    void SpawnBall() {
        pleaseSpawn = false;
        Debug.Log("Spawning ball");
        Instantiate(ball, transform.position, Quaternion.identity);
    }

    void KeyboardInput() {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    SpawnBall();
    }
    private void Update() {
        //KeyboardInput();
        if (pleaseSpawn) {
            SpawnBall();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            clock.SetBPM(BPM);
        }
    }

}
