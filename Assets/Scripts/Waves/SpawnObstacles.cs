using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

public class SpawnObstacles : NetworkBehaviour
{
    public GameObject[] obstacles;
    public int zPos = 60;
    public bool spawnobs = false;
    public int obsNum;

    private List<GameObject> obsActive = new List<GameObject>();

    // Update is called once per frame
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
        obsNum = Random.Range(0, 2);
        zPos += 60;
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
        yield return new WaitForSeconds(40);
        Destroy(obsActive[0]);
        obsActive.RemoveAt(0);
    }
}
