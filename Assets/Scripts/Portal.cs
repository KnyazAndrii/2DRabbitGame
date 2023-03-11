using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int _nextLevel;
    private float _teleportculdown = 0.8f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine("TeleportTimer");
        }
    }

    IEnumerator TeleportTimer()
    {
        yield return new WaitForSeconds(_teleportculdown);
        SceneManager.LoadScene(_nextLevel);
    }
}
