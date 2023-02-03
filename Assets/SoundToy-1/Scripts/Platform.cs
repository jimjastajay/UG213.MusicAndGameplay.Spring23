using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    AudioSource audioSource;

    //Part III Adding synth
    //PXStrax pxStrax;

    [SerializeField]
    float midiNote = 100.0f;

    float[] d = new float[100];

    private void Awake() {
        //pxStrax = GetComponent<PXStrax>();//Part III
        audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < d.Length; i++) {
            d[i] = Random.Range(100, 900);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            audioSource.Play();
            //pxStrax.KeyOn(midiNote); //Part III
        }
    }
}
