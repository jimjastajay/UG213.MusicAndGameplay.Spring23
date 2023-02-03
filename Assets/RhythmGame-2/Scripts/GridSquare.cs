using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GridSquare : MonoBehaviour {

    public delegate void OnLetterPressDelegate(KeyCode c);
    public static event OnLetterPressDelegate OnLetterPress;
    Vector3 startSize;
    SpriteRenderer spriteRenderer;
    Color transparent = new Color(0.5f, 0.5f, 0.5f, 0.35f);
    Color currentColor = Color.white;

    public TMP_Text text;
    KeyCode keyCode;

    bool beatActive = false;

    #region Setup
    private void Awake() {
        startSize = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupBox(KeyCode c) {
        text.text = c.ToString();
        keyCode = c;
        spriteRenderer.color = transparent;
    }
    #endregion

    #region Animations
    public void PlaySquare() {
        beatActive = true;
        StartCoroutine(PlaySquareRoutine());
    }
    IEnumerator PlaySquareRoutine() {
        float t = 0.0f;
        currentColor = Color.white;
        while (t < 1.0f) {
            transform.localScale = Vector3.Lerp(startSize * 1.5f, startSize, t);
            t += Time.deltaTime;
            yield return null;
        }
        beatActive = false;
        transform.localScale = startSize;
    }


    void PlayPressAnimation() {
        if (beatActive)
            StartCoroutine(PressAnimationRoutine());
    }
    IEnumerator PressAnimationRoutine() {
        float t = 0.0f;
        currentColor = Color.green;
        OnLetterPress?.Invoke(keyCode);
        while (t < 1.0f) {
            spriteRenderer.color = Color.Lerp(currentColor, transparent, t);
            t += Time.deltaTime;
            yield return null;
        }
        currentColor = transparent;
        spriteRenderer.color = transparent;
    }

    #endregion

    #region Input
    void CheckInput() {

        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(kcode) && kcode == keyCode) {
                PlayPressAnimation();
            }
        }

    }
    #endregion

    private void Update() {
        CheckInput();
    }
}
