using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatformV2 : MonoBehaviour
{
    public GameObject[] platform;
    public int zPos = 109;
    public bool spawnplat = false;
    public int platNum;

    private List<GameObject> platActive = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (spawnplat == false)
        {
            spawnplat = true;
            StartCoroutine(SpawnPlatform());
            StartCoroutine(DeletePlataform());
        }
    }

    IEnumerator SpawnPlatform()
    {
        platNum = Random.Range(0, 2);
        GameObject go = Instantiate(platform[platNum], new Vector3(0, -4, zPos), Quaternion.Euler(new Vector3(0, 90, 0)));
        platActive.Add(go);
        zPos += 109;
        yield return new WaitForSeconds(2.8f);
        spawnplat = false; 
    }

    IEnumerator DeletePlataform()
    {
        yield return new WaitForSeconds(40);
        Destroy(platActive[0]);
        platActive.RemoveAt(0);
    }
}
