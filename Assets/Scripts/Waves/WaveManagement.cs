using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WaveManagement : NetworkBehaviour
{
    public float contador = 0;
    public float multiplicador = 1;
    public SpawnObstacles zPosGeneral;
    public Wave2 zPosGeneral2;
    public Wave3 zPosGeneral3;
    public int generalPosition;
    public int obsNum;
    public GameObject[] obstacles;

    public bool chegou = false;
    public Final chegouatrasado;

    private List<GameObject> obsActive = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        chegouatrasado = FindAnyObjectByType<Final>();
        chegou = chegouatrasado.chegouatrasado;

        if (contador >= 0)
        {
            contador += multiplicador * Time.deltaTime;
            zPosGeneral = FindObjectOfType<SpawnObstacles>();
            generalPosition = zPosGeneral.zPos;
        }

        if (contador >= 25)
        {
            this.GetComponent<SpawnObstacles>().enabled = false;
            this.GetComponent<Wave2>().enabled = true;
        }

        if (contador >= 50)
        {
            zPosGeneral2 = FindObjectOfType<Wave2>();
            generalPosition = zPosGeneral2.zPos;
            this.GetComponent<Wave2>().enabled = false;
            this.GetComponent<Wave3>().enabled = true;
        }

        if (contador >= 75)
        {
            this.GetComponent <Wave3>().enabled = false;
            this.GetComponent<Final>().enabled = true;
            zPosGeneral3 = FindObjectOfType<Wave3>();
            generalPosition = zPosGeneral3.zPos;
            GameObject go = Instantiate(obstacles[obsNum], new Vector3(0, -3.50f, generalPosition), Quaternion.Euler(new Vector3(0, 0, 0)));
        }

        //this.GetComponent<Final>().enabled = true;

        if (contador >= 80)
        {
            this.GetComponent<SpawnObstacles>().enabled = false;
            this.GetComponent<Wave2>().enabled = false;
            this.GetComponent<Wave3>().enabled = false;
        }

        if (chegou == true)
        {
            this.GetComponent<Final>().enabled = false;
            this.GetComponent<Final2>().enabled = true;
        }
    }
}
