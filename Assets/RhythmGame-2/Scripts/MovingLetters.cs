using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MovingLetters : MonoBehaviour {

    TMP_Text text;
    SpriteRenderer outerCircleSprite;
    KeyCode keyCode;
    Vector3 startLocation;
    Vector3 targetLocation;

    float moveTime;
    bool isActive;

    #region Delegates

    private void OnEnable() {
        GridSquare.OnLetterPress += OnLetterPress;
    }
    private void OnDisable() {
        GridSquare.OnLetterPress -= OnLetterPress;
    }
    void OnLetterPress(KeyCode c) {
        if (keyCode == c && isActive) {
            outerCircleSprite.color = Color.green;
            BeatUpLetter();
        }
    }

    #endregion

    #region Setup

    private void Awake() {
        outerCircleSprite = GetComponentInChildren<SpriteRenderer>();
        text = GetComponent<TMP_Text>();
    }
    public void SpawnLetter(KeyCode c, float r) {
        moveTime = r;
        keyCode = c;
        text.text = c.ToString();
        startLocation = transform.position;
        int rand = Random.Range(0, 2);
        targetLocation = new Vector3(8.0f,
            startLocation.y);
        StartCoroutine(MoveLetterRoutine());
    }
    #endregion

    #region Letter Beating up
    public void BeatUpLetter() {
        StartCoroutine(BeatUpLetterRoutine());
    }
    IEnumerator BeatUpLetterRoutine() {
        float t = 0.0f;
        isActive = false;
        while (t < 1.0f) {
            t += Time.deltaTime;
            text.enabled = false;
            outerCircleSprite.color = Color.Lerp(outerCircleSprite.color, Color.clear, t);
            yield return null;
        }
        Destroy(gameObject);
    }
    #endregion

    IEnumerator MoveLetterRoutine() {
        float t = 0.0f;
        isActive = true;
        while (t < moveTime) {
            transform.position = Vector3.Lerp(startLocation, targetLocation, t / moveTime);
            t += Time.deltaTime;
            yield return null;
        }
        if (isActive) {
            isActive = false;
            outerCircleSprite.color = Color.red;
            transform.position = targetLocation;
            BeatUpLetter();
        }
    }
}
