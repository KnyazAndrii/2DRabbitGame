using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private float _jumpForse = 8f;
    private Animator _animator;

    public Player Player;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!Player.IsGrounded)
            {
                Player.Rigidbody2D.AddForce(transform.up * _jumpForse, ForceMode2D.Impulse);
                _animator.Play("JumpOnTrampoline");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.Play("IdleTrampoline");
    }
}
