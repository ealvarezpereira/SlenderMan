using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeleportSlenderman : MonoBehaviour {

    //Follow
    public Transform objetivo;
    float pantalla = 0.8f;
    Rigidbody rb;
    public bool Visible = true;
    private float velocidad = 0;
    private float punto;
    public AudioClip horror = null;
    private float volumen = 1.0f;
    public Transform posicion = null;
    int m = 0, m2 = 0;
    public Camera camara;

    //Teleport
    public Transform player;      // the Object the player is controlling
    public Vector3 spawnOrgin;     // this will be the bottom right corner of a square we will use as the spawn area
    public Vector3 maximum;        // max distance in the x, y, and z direction the enemy can spawn
    public float spawnRate;        // how often the enemy will respawn
    private bool nearPlayer = false; // use this to stop the teleporting if near the player
    private float nextTeleport = 0.0f; // will keep track of when we to teleport next
    private float x, z;

    Terrain terr; // game terrain
    int hmWidth; // heightmap width
    int hmHeight; // heightmap height
    int posXInTerrain; // position of the game object in terrain width (x axis)
    int posYInTerrain; // position of the game object in terrain height (z axis)

    public RawImage imagen;
    public RawImage win;
    public RawImage YouDied;
    public float opacity;
    bool termino = false;
    public AudioClip wasted = null;
    public AudioClip youwin = null;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nextTeleport = spawnRate;
        x = 100f;
        z = 100f;
        opacity = 0f;
        terr = Terrain.activeTerrain;
        hmWidth = terr.terrainData.heightmapWidth;
        hmHeight = terr.terrainData.heightmapHeight;
        YouDied.color = new Color(1, 1, 1, 0);
        win.color = new Color(1, 1, 1, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Esvisible();

        transform.LookAt(objetivo.position);

        if (!Visible)
        {
            transform.position = transform.position + transform.forward * velocidad * Time.deltaTime;
        }

        if (transform.position.y < terr.terrainData.GetHeight(posXInTerrain, posYInTerrain)) {
            transform.position = new Vector3(transform.position.x, terr.terrainData.GetHeight(posXInTerrain, posYInTerrain)+4, transform.position.z);
        }

        imagen.color = new Color(1, 1, 1, opacity); //Imagen de ruido blanco

        if (opacity >= 2)
        {
           
            if (m2 == 0) AudioSource.PlayClipAtPoint(wasted, posicion.position, volumen);
            m2 = 1;
            termino = true;
            YouDied.color = new Color(1, 1, 1, 255);
            NuevoJuego();

        }
        else if (Note.notasRecogidas == 8) {

            if (m2 == 0) AudioSource.PlayClipAtPoint(youwin, posicion.position, volumen);
            m2 = 1;
            termino = true;
            win.color = new Color(1, 1, 1, 255);
            NuevoJuego();
        }
    }


    void Teleport() {

        // get the normalized position of slenderman relative to the terrain
        Vector3 tempCoord = (transform.position - terr.gameObject.transform.position);
        Vector3 coord;
        coord.x = tempCoord.x / terr.terrainData.size.x;
        coord.y = tempCoord.y / terr.terrainData.size.y;
        coord.z = tempCoord.z / terr.terrainData.size.z;

        // get the position of the terrain heightmap where slenderman is
        posXInTerrain = (int)(coord.x * hmWidth);
        posYInTerrain = (int)(coord.z * hmHeight);



        if (!nearPlayer)     // only teleport if we are not close to the player
        {
            if (Time.time > nextTeleport)   // only teleport if enough time has passed
            {
                transform.position = new Vector3(Random.Range(player.transform.position.x, player.transform.position.x+x),
                    terr.terrainData.GetHeight(posXInTerrain,posYInTerrain), Random.Range(player.transform.position.z,player.transform.position.z+z));   // teleport
                nextTeleport += spawnRate;    // update the next time to teleport
            }
        }
    }

    void Esvisible()
    {
        Vector3 delante = objetivo.forward;
        Vector3 otro = (this.transform.position - objetivo.position).normalized;


        Vector3 v1 = objetivo.position;

        Vector3 v2 = transform.position;


        punto = Vector3.Dot(delante, otro);

        if (termino == false)
        {
            if (Note.notasRecogidas >= 1)
            {
                if (Note.notasRecogidas >= 1 && Note.notasRecogidas <= 3)
                {
                    velocidad = 3;
                    spawnRate = 5;
                }
                else if (Note.notasRecogidas >= 4 && Note.notasRecogidas <= 6)
                {
                    velocidad = 5;
                    spawnRate = 4;
                    x = 50f;
                    z = 50f;
                }
                else if (Note.notasRecogidas >= 7 && Note.notasRecogidas <= 8)
                {
                    velocidad = 7;
                    spawnRate = 3;
                    x = 35f;
                    z = 35f;
                }

                if (punto > pantalla)
                {

                    if (Vector3.Distance(v1, v2) < 50)
                    {
                        if (m == 0) AudioSource.PlayClipAtPoint(horror, posicion.position, volumen);
                        Teleport();
                        nearPlayer = false;
                        opacity += .05f;
                    }

                    if (Vector3.Distance(v1, v2) < 20)
                    {
                        opacity += .05f;
                        if (m == 0) AudioSource.PlayClipAtPoint(horror, posicion.position, volumen);
                        Visible = true;
                        nearPlayer = true;
                    }
                    else
                    {
                        Teleport();
                        nearPlayer = false;
                    }

                    m++;
                    
                }
                else
                {
                    if (opacity > 0)
                    {
                        opacity -= .05f;
                    }

                    m = 0;
                    Visible = false;
                    Teleport();
                    nearPlayer = false;
                }

                if (Vector3.Distance(v1, v2) < 1)
                {

                    opacity = 5f;
                    nearPlayer = true;
                    Visible = true;
                    termino = true;
                }
            }
        }
        else
        {
            if (Note.notasRecogidas != 8) { 
                opacity = 5f;
            }
            nearPlayer = true;
            Visible = true;
            termino = true;
        }
    }

    void NuevoJuego() {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Time.timeScale = 1;
            transform.position = new Vector3(453.17f, 13.16f, 58.96f);
            termino = false;
            Note.notasRecogidas = 0;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        else if (Input.GetKeyDown(KeyCode.N)) {

            Application.Quit();
        }
    }

}
