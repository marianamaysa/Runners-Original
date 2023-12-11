using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Vida : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public float time;
    private float timeStore;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public int colidiu = 0;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "OBS")
        {
            colidiu++;

        }
    }

    void Update()
    {
        if (colidiu == 2)
        {
            numOfHearts = 3;
        }

        if (colidiu == 4)
        {
            numOfHearts = 2;
        }

        if (colidiu == 6)
        {
            numOfHearts = 1;
        }

        if (colidiu == 8)
        {
            numOfHearts = 0;
            SceneManager.LoadScene(4);
        }


        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {

            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }

            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
}
    }
}
