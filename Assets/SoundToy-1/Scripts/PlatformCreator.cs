using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCreator : MonoBehaviour {


    #region Parameters

    /// <summary>
    /// Selected platform int
    /// </summary>
    int selectedPlatformIndex;

    /// <summary>
    /// The currently selected platform
    /// </summary>
    [SerializeField]
    Platform currentPlatform;

    /// <summary>
    /// List of platforms
    /// </summary>
    public List<Platform> platforms = new List<Platform>();

    bool checkMousePosition;
    Vector3 startPosition = new Vector3();

    [SerializeField]
    SpriteRenderer cursor;

    #endregion


    private void Start() {
        currentPlatform = null;
    }

    /// <summary>
    /// Spawns a platform and sets the position, direction, and length as long as the mouse is pressed;
    /// </summary>
    /// <param name="pos"></param>
    void StartCreatePlatform(Vector3 pos) {

        if (currentPlatform == null) {
            currentPlatform = Instantiate(platforms[selectedPlatformIndex]);
        }

        Vector3 delta = pos - startPosition;
        Vector3 center = (pos + startPosition) / 2.0f;
        currentPlatform.transform.position = new Vector3(center.x, center.y);
        currentPlatform.transform.right = delta;
        Vector3 lScale = currentPlatform.transform.localScale;

        lScale.x = delta.magnitude;
        currentPlatform.transform.localScale = lScale;

    }

    void CursorTracking(Vector2 pos) {
        cursor.transform.position = pos;
        cursor.enabled = !checkMousePosition;
        cursor.color = platforms[selectedPlatformIndex].GetComponent<SpriteRenderer>().color;
    }

    void MouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            checkMousePosition = true;
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0) && checkMousePosition) {
            checkMousePosition = false;
            currentPlatform = null;
        }
    }

    void KeyboardInput() {
        //Tab cycles through different platforms
        if (Input.GetKeyDown(KeyCode.Tab)) {
            selectedPlatformIndex += 1;
            if (selectedPlatformIndex >= platforms.Count) {
                selectedPlatformIndex = 0;
            }
        }
    }

    private void Update() {
        KeyboardInput();
        MouseInput();
        Vector3 mosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CursorTracking(mosPos);
        if (checkMousePosition)
            StartCreatePlatform(mosPos);
    }
}
