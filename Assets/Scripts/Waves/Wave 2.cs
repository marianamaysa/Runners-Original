using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Wave2 : NetworkBehaviour
{
    public GameObject[] obstacles;
    public WaveManagement zPosGeneral;
    public int zPos;
    public bool spawnobs = false;
    public int obsNum;

    private List<GameObject> obsActive = new List<GameObject>();

    void Start()
    {
        zPosGeneral = FindObjectOfType<WaveManagement>();
        zPos = zPosGeneral.generalPosition;
        zPos += 60;
    }
    void Update()
    {
        if (spawnobs == false)
        {
            spawnobs = true;
            StartCoroutine(SpawnObstacle());
            StartCoroutine(DeleteObstacle());
        }
    }


    IEnumerator SpawnObstacle()
    {
        CreateWaveServerRpc();
        yield return new WaitForSeconds(1);
        spawnobs = false;
    }

    [ServerRpc]
    public void CreateWaveServerRpc()
    {
        obsNum = Random.Range(0, 4);
        zPos += 40;
        CreateWaveClientRpc(obsNum, zPos);
    }

    [ClientRpc]
    public void CreateWaveClientRpc(int obsNum, int zPos)
    {
        GameObject go = Instantiate(obstacles[obsNum], new Vector3(0, -3.50f, zPos), Quaternion.Euler(new Vector3(0, -90, 0)));
        obsActive.Add(go);
    }

    IEnumerator DeleteObstacle()
    {
        yield return new WaitForSeconds(80);
        Destroy(obsActive[0]);
        obsActive.RemoveAt(0);
    }
}
