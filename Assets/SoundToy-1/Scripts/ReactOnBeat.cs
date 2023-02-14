using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;

public class ReactOnBeat : MonoBehaviour {

    [SerializeField]
    Clock clock;

    SpriteRenderer spriteRenderer;

    [SerializeField]
    Beat.TickValue tickValue;

    bool pleaseChange = false;

    #region Delegate

    private void OnEnable() {
        clock.Beat += OnBeat;
        clock.Eighth += OnBeat;
    }
    private void OnDisable() {
        clock.Beat -= OnBeat;
        clock.Eighth -= OnBeat;
    }

    void OnBeat(Beat.Args beatArgs) {
        if(tickValue == beatArgs.BeatVal) {
            ReactAction();
        }
    }

    #endregion

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void ReactAction() {
        Debug.Log("Hey!!");
        pleaseChange = true;
    }

    void ChangeColor() {
        pleaseChange = false;
        spriteRenderer.color = new Color(Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    private void Update() {
        if (pleaseChange) {
            ChangeColor();
        }
    }

    //[SerializeField]
    //Clock clock;

    //SpriteRenderer spriteRenderer;

    //[SerializeField]
    //Beat.TickValue tickValue;

    //bool pleaseChange = false;

    //private void OnEnable() {
    //    clock.Beat += OnBeat;
    //    clock.Eighth += OnBeat;
    //}
    //private void OnDisable() {
    //    clock.Beat -= OnBeat;
    //    clock.Eighth -= OnBeat;
    //}

    //public void OnBeat(Beat.Args beatArgs) {
    //    if (beatArgs.BeatVal == tickValue) {
    //        ReactAction();
    //    }
    //}

    //private void Awake() {
    //    spriteRenderer = GetComponent<SpriteRenderer>();
    //}

    //void ReactAction() {
    //    Debug.Log("Reaction");
    //    pleaseChange = true;
    //}

    //void ChangeColor() {
    //    pleaseChange = false;
    //    spriteRenderer.color =
    //        new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    //}

    //private void Update() {
    //    if (pleaseChange)
    //        ChangeColor();
    //}

}
