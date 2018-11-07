using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour {

    public AudioSource p;

	// Use this for initialization
	void Start () {
        p.Stop();
	}
	
	// Update is called once per frame
	void Update () {


        if (Note.notasRecogidas > 1) {
            p.Play();   
        }
	}   
}
