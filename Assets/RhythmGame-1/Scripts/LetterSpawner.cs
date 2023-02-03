using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class LetterSpawner : MonoBehaviour {

    [SerializeField]
    AK.Wwise.Event musicEvent;
    uint playingID;

    [SerializeField]
    PopupText popupTextPrefab;

    [SerializeField]
    float beatDuration;

    [SerializeField]
    float barDuration;
    bool durationSet = false;

    [SerializeField]
    KeyCode[] selectedKeys = new KeyCode[] {KeyCode.A,KeyCode.B,KeyCode.C, KeyCode.D,KeyCode.E,KeyCode.F, KeyCode.G, KeyCode.H,KeyCode.I, KeyCode.J, KeyCode.K,
        KeyCode.L,KeyCode.M,KeyCode.N,KeyCode.O,KeyCode.P, KeyCode.Q,KeyCode.R,KeyCode.S,KeyCode.T,KeyCode.U,KeyCode.V,KeyCode.W,KeyCode.X,KeyCode.Y,KeyCode.Z };

    #region Getters/Setters

    public float BeatDuration {
        get { return beatDuration; }
    }

    #endregion

    #region Setup
    private void Awake() {
        playingID = musicEvent.Post(gameObject, (uint)(AkCallbackType.AK_MusicSyncAll | AkCallbackType.AK_EnableGetMusicPlayPosition | AkCallbackType.AK_MIDIEvent), CallbackFunction);
    }
    #endregion


    #region Letter Spawning

    void ChooseLetter(AkMusicSyncCallbackInfo info) {
        Debug.Log($"Choosing letter {info}");
        Vector3 randPos = new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(-4.0f, 4.0f));
        int r = Random.Range(0, selectedKeys.Length);
        PopupText pt = Instantiate(popupTextPrefab, randPos, Quaternion.identity);

        float multiplier = 1.0f;
        if (info.userCueName == "2") {
            multiplier = 2.0f;
        }
        //Debug.Log($"Multiplier: {multiplier}");

        AkSegmentInfo segmentInfo = new AkSegmentInfo();
        AkSoundEngine.GetPlayingSegmentInfo(playingID, segmentInfo, true);

        pt.SpawnLetter(selectedKeys[r], multiplier, segmentInfo, this);

    }

    #endregion


    #region WWise Integration

    void CallbackFunction(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info) {
        AkMusicSyncCallbackInfo musicInfo;

        if (in_info is AkMusicSyncCallbackInfo) {
            musicInfo = (AkMusicSyncCallbackInfo)in_info;
            switch (in_type) {

                case AkCallbackType.AK_MusicSyncUserCue:
                    ChooseLetter(musicInfo);
                    break;

                case AkCallbackType.AK_MusicSyncBeat:
                    break;
                case AkCallbackType.AK_MusicSyncBar:
                    break;
            }

            if (in_type is AkCallbackType.AK_MusicSyncBar) {
                if (!durationSet) {
                    beatDuration = musicInfo.segmentInfo_fBeatDuration;
                    barDuration = musicInfo.segmentInfo_fBarDuration;
                    durationSet = true;
                }
            }

        }
    }

    public int GetMusicTimeInMS() {
        AkSegmentInfo segmentInfo = new AkSegmentInfo();
        AkSoundEngine.GetPlayingSegmentInfo(playingID, segmentInfo, true);
        return segmentInfo.iCurrentPosition;
    }

    #endregion

}
