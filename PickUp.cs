using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PickUp : MonoBehaviour {

    public AudioClip cansancio = null;
    private int m = 0;
    public Text texto;
    //---------------------------------------------------------
    private float Stamina = 7.0f;
    private float MaxStamina = 7.0f;
    //---------------------------------------------------------
    private float StaminaRegenTimer = 0.0f;
    //---------------------------------------------------------
    private const float StaminaDecreasePerFrame = 1.0f;
    private const float StaminaIncreasePerFrame = 5.0f;
    private const float StaminaTimeToRegen = 3.0f;
    //---------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        texto.text = "Left-Click to pickup";
    }

    void OnTriggerExit(Collider other)
    {
        texto.text = null;
    }

    private void Update()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning)
        {
            Stamina = Mathf.Clamp(Stamina - (StaminaDecreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
            StaminaRegenTimer = 0.0f;

            if (Stamina == 0.0f)
            {
                if (m == 0) AudioSource.PlayClipAtPoint(cansancio, transform.position, 500);
                FirstPersonController.m_RunSpeed = 5f;
                m++;
            }
            else
            {
                m = 0;
                FirstPersonController.m_RunSpeed = 10f;
            }
        }
        else if (Stamina < MaxStamina)
        {
            if (StaminaRegenTimer >= StaminaTimeToRegen)
            { 
                Stamina = Mathf.Clamp(Stamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
                FirstPersonController.m_RunSpeed = 10f;
                m = 0;
            }
            else
            {
                m = 0;
                StaminaRegenTimer += Time.deltaTime;
                FirstPersonController.m_RunSpeed = 10f;
            }

        }
    }
}