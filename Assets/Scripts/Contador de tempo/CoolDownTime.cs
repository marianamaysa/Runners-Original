using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownTime : MonoBehaviour
{

    public float contador = 0;
    public float multiplicador = 1;
    public bool final = false;

    public GameObject numero1;
    public GameObject numero2;
    public GameObject numero3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount += Time.deltaTime * 0.040f;

        contador += multiplicador * Time.deltaTime;

        if (final == false && GetComponent<Image>().fillAmount == 1)
        {
            GetComponent<Image>().fillAmount = 0;
            GetComponent<Image>().fillAmount += Time.deltaTime * 0.0315f;
        }


        if (contador >= 0){
            numero1.SetActive(true);
        }

        if (contador >= 25)
        {
            numero1.SetActive(false);
            numero2.SetActive(true);
        }

        if (contador >= 50)
        {
            numero2.SetActive(false);
            numero3.SetActive(true);
        }

        if (contador >= 75)
        {
            final = true;
        }
    }
}
