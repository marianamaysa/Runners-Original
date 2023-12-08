using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{

    public bool chegou = false;
    public bool chegouatrasado = false;
    public WaveManagement zPosGeneralFinal;
    public int zPos;
    public PlayerMovement PosJogador;
    public float zPosJogador;

    public WaveManagement chegouWave;


    void Start()
    {
        zPosGeneralFinal = FindObjectOfType<WaveManagement>();
        zPos = zPosGeneralFinal.generalPosition;
    }

    // Update is called once per frame
    void Update()
    {
       PosJogador = FindAnyObjectByType<PlayerMovement>();
        zPosJogador = PosJogador.transform.position.z;

        chegouWave = FindAnyObjectByType<WaveManagement>();
        chegouatrasado = chegouWave.chegou;

        if (zPosJogador > zPos)
        {
            chegou = true;
        }

        
        if (chegou == true)
        {
            Debug.Log("Vitoria");
            SceneManager.LoadScene(1);
            chegouatrasado = true;
            this.GetComponent <Final>().enabled = false;
        }
    }
}
