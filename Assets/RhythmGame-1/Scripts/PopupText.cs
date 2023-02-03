using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour {

    LetterSpawner letterSpawner;

    public enum HitOrMiss { Perfect, Good, Okay, Miss, Late }
    [SerializeField]
    Color[] successColors = new Color[5];
    KeyCode selectedKeyCode;

    TMP_Text text;

    [SerializeField]
    int perfectWindow = 100;
    [SerializeField]
    int goodWindow = 250;
    [SerializeField]
    int okayWindow = 500;

    float expandTime = 1.0f;
    int targetTimeMS;
    bool isCheckingInput = true;

    [SerializeField]
    SpriteRenderer innerCircle;
    [SerializeField]
    SpriteRenderer outerCircle;

    #region Setup
    private void Awake() {
        text = GetComponent<TMP_Text>();
    }
    private void Start() {
        innerCircle.transform.localScale = Vector3.zero;
    }
    #endregion

    #region Letter Management

    public void SpawnLetter(KeyCode c, float m, AkSegmentInfo segInfo, LetterSpawner ls) {
        letterSpawner = ls;
        selectedKeyCode = c;
        text.text = c.ToString();
        int currentTime = letterSpawner.GetMusicTimeInMS();
        targetTimeMS = Mathf.FloorToInt(currentTime + (ls.BeatDuration * 1000 * m));
        expandTime = (targetTimeMS - currentTime) * 0.001f;
        Debug.Log($"Current time: {currentTime} || Target time: {targetTimeMS}", gameObject);
        StartCoroutine(ExpandCircleRoutine());
    }
    IEnumerator ExpandCircleRoutine() {
        float t = 0.0f;
        while (t < expandTime) {
            innerCircle.transform.localScale = Vector3.Lerp(Vector3.zero, outerCircle.transform.localScale, t / expandTime);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        if (isCheckingInput) {
            isCheckingInput = false;
            FadeOut(HitOrMiss.Miss);
        }
    }

    #endregion

    #region Input

    void CheckInput() {

        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(kcode) && kcode == selectedKeyCode) {
                HitOrMiss hm = HitOrMiss.Miss;

                int currentTime = letterSpawner.GetMusicTimeInMS();
                int offBy = targetTimeMS - currentTime;

                if (offBy >= 0) {
                    if (offBy <= perfectWindow)
                        hm = HitOrMiss.Perfect;
                    else if (offBy <= goodWindow)
                        hm = HitOrMiss.Good;
                    else
                        hm = HitOrMiss.Okay;
                }
                else {
                    if (offBy > -perfectWindow)
                        hm = HitOrMiss.Late;
                }
                Debug.Log($"Target Time: {targetTimeMS} || Input time: {currentTime} ||  OffBy: {offBy} || Hit or Miss: {hm}", gameObject);

                isCheckingInput = false;
                FadeOut(hm);
            }
        }

    }

    #endregion


    #region Animations
    public void FadeOut(HitOrMiss hm) {
        StartCoroutine(FadeOutRoutine(hm));
    }
    IEnumerator FadeOutRoutine(HitOrMiss hm) {

        text.color = successColors[(int)hm];
        innerCircle.color = successColors[(int)hm];
        float t = 0.0f;
        while (t < 1.0f) {
            t += Time.deltaTime;
            text.color = Color.Lerp(successColors[(int)hm], Color.clear, t);
            innerCircle.color = Color.Lerp(successColors[(int)hm], Color.clear, t);
            yield return null;
        }
        Destroy(gameObject);
    }
    #endregion

    private void Update() {
        if (isCheckingInput)
            CheckInput();
    }
}
