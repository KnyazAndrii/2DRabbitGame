using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    private float _xPosition = -1;

    private SpriteRenderer _spriteRenderer;
    public Player Player;
    private Animator _animator;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _animator.Play("FlyingCompanion");
    }

    private void Update()
    {
        FlipLeft();
        FlipRight();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            _xPosition = -1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            _xPosition = 1;
        }

        transform.position = new Vector2(Player.transform.position.x + _xPosition, Player.transform.position.y + 1);
    }

    private void FlipLeft()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void FlipRight()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _spriteRenderer.flipX = false;
        }
    }
}
