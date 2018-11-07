using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour {

    public GameObject pickup;
    public GameObject area;
    public Text texto;
    public Text contador;
    public AudioClip recogerNota = null;
    public float volumen = 1.0f;
    public Transform posicion = null;
    public static int notasRecogidas;
    public void Start()
    {
        
        posicion = transform;
        SetCountText();
    }

    private void OnMouseDown()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            notasRecogidas++;
            if (recogerNota) AudioSource.PlayClipAtPoint(recogerNota, posicion.position, volumen);
            texto.text = null;
            SetCountText();
            area.SetActive(false);
            pickup.SetActive(false);
        }
    }
    void SetCountText()
    {
        contador.text = notasRecogidas.ToString()+"/8 Notas Recogidas.";
    }
}
