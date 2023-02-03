using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Sequencer : MonoBehaviour {

    [SerializeField]
    AK.Wwise.Event musicEvent;
    uint playingID;

    bool beatDurationSet = false;

    [SerializeField, ReadOnly]
    int BPM;
    [SerializeField, ReadOnly]
    float barDuration;
    [SerializeField, ReadOnly]
    float beatDuration;
    [SerializeField, ReadOnly]
    int currentBeat;

    [SerializeField]
    List<Column> columns = new List<Column>();

    [SerializeField]
    MovingLetters movingLetterPrefab;
    public Transform spawnLocation;

    KeyCode[] keyPool = new KeyCode[] {KeyCode.A,KeyCode.B,KeyCode.C, KeyCode.D,KeyCode.E,KeyCode.F, KeyCode.G, KeyCode.H,KeyCode.I, KeyCode.J,
        KeyCode.L,KeyCode.M,KeyCode.N,KeyCode.O,KeyCode.P, KeyCode.Q,KeyCode.R,KeyCode.S,KeyCode.T,KeyCode.U,KeyCode.V,KeyCode.W,KeyCode.X,KeyCode.Y,KeyCode.Z,
    KeyCode.A,KeyCode.B,KeyCode.C, KeyCode.D,KeyCode.E,KeyCode.F, KeyCode.G, KeyCode.H,KeyCode.I, KeyCode.J,};

    List<KeyCode> activeKeys = new List<KeyCode>();

    #region Setup
    private void Awake() {
        playingID = musicEvent.Post(gameObject,
            (uint)(AkCallbackType.AK_MusicSyncAll |
            AkCallbackType.AK_EnableGetMusicPlayPosition |
            AkCallbackType.AK_MIDIEvent), CallbackFunction);
    }

    private void Start() {
        int c = 0;
        int g = 0;
        for (int i = 0; i < 32; i++) {

            int x = Random.Range(0, 32);
            columns[c].gridSquareList[g].SetupBox(keyPool[x]);
            activeKeys.Add(keyPool[x]);
            g += 1;
            if (g > 3) {
                c += 1;
                g = 0;
            }
        }
    }
    #endregion


    #region WWise Events

    void OnPlayColumn() {
        columns[currentBeat].PlayColumn();
        currentBeat += 1;
        if (currentBeat > columns.Count - 1) {
            currentBeat = 0;
        }
    }

    #endregion


    #region Wwise Integration

    void CallbackFunction(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info) {

        AkMusicSyncCallbackInfo musicInfo;
        AkMIDIEventCallbackInfo midiInfo;

        if (in_info is AkMusicSyncCallbackInfo) {
            musicInfo = (AkMusicSyncCallbackInfo)in_info;
            switch (in_type) {

                case AkCallbackType.AK_MusicSyncUserCue:
                    Debug.Log("User Cue");
                    break;

                case AkCallbackType.AK_MusicSyncBeat:
                    Debug.Log("Sync Beat Callback");
                    OnPlayColumn();
                    break;
                case AkCallbackType.AK_MusicSyncBar:
                    Debug.Log("Sync Bar Callback");
                    if (!beatDurationSet) {
                        beatDurationSet = true;
                        BPM = (int)(60 / beatDuration);
                    }
                    barDuration = musicInfo.segmentInfo_fBarDuration;
                    beatDuration = musicInfo.segmentInfo_fBeatDuration;
                    break;
            }

        }

        if (in_info is AkMIDIEventCallbackInfo) {
            midiInfo = (AkMIDIEventCallbackInfo)in_info;
            Debug.Log($"Midi Info Callback }} Note: {midiInfo.byOnOffNote}");

            switch (midiInfo.byType) {
                case AkMIDIEventTypes.NOTE_ON:
                    NoteOnEvents(midiInfo);
                    break;

                case AkMIDIEventTypes.NOTE_OFF:
                    break;

                case AkMIDIEventTypes.PITCH_BEND:
                    break;
            }
        }
    }

    void NoteOnEvents(AkMIDIEventCallbackInfo b) {
        Debug.Log($"Note: {b.byOnOffNote}");
        switch (b.byOnOffNote) {
            case MidiBytes.noteNumberC5:
                SpawnLetter(beatDuration * 12.0f);
                break;
            case MidiBytes.noteNumberE5:
                SpawnLetter(beatDuration * 8.0f);
                break;
        }
    }


    #endregion

    void SpawnLetter(float n) {
        int rand = Random.Range(0, 2);
        MovingLetters ml = Instantiate(movingLetterPrefab,
    new Vector3(spawnLocation.position.x, rand == 0 ? -4.0f : 4.0f, 0.0f),
    Quaternion.identity);
        int r = Random.Range(0, activeKeys.Count);
        ml.SpawnLetter(activeKeys[r], n);
    }


}
