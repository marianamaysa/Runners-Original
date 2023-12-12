using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Encontrar todos os objetos com a tag "Player" na cena
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Ativar o script de movimenta��o para cada jogador
        foreach (GameObject player in players)
        {
            ActivateMovementScript(player);
        }
    }

    void ActivateMovementScript(GameObject playerObject)
    {
        // Obt�m o componente PlayerMovement do jogador e ativa o script
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        else
        {
            Debug.LogError("PlayerMovement script n�o encontrado no jogador.");
        }
    }
}

