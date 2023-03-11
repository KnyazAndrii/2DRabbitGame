using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsCollected" + gameObject.name) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Coins++;

            PlayerPrefs.SetInt("Coins", player.Coins);
            PlayerPrefs.SetInt("IsCollected" + gameObject.name, 1);

            Destroy(gameObject);
        }
    }
}
