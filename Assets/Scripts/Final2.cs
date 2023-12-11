using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Final2 : MonoBehaviour
{

    public bool chegou = false;
    public bool chegouatrasado = false;
    public WaveManagement zPosGeneralFinal;
    public int zPos;
    public PlayerMovement PosJogador;
    public float zPosJogador;

    public WaveManagement chegouWave;


    // Start is called before the first frame update
    void Start()
    {
        zPosGeneralFinal = FindObjectOfType<WaveManagement>();
        zPos = zPosGeneralFinal.generalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        chegouWave = FindAnyObjectByType<WaveManagement>();
        chegouatrasado = chegouWave.chegou;

        if (chegouatrasado == true)
        {
            Debug.Log("Defeat");
            SceneManager.LoadScene(4);
            this.GetComponent<Final2>().enabled = false;
        }

       

    }
}
