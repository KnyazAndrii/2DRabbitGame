using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform PointsContainer;
    public List<Transform> Points;
    public int Damage = 20;
    public Transform PlayerTransform;

    private float _culdown = 1f;
    private int _currentPoint = 0;
    private float _speed = 0.1f;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < PointsContainer.childCount; i++)
        {
            Points.Add(PointsContainer.GetChild(i));
        }
    }

    private void FixedUpdate()
    {
        AttackPlayer();
    }

    private void Update()
    {
        FlipEnemy();
    }

    private void MoveToPoints()
    {
        if (transform.position == Points[_currentPoint].position)
        {
            _currentPoint++;

            if (_currentPoint == Points.Count)
            {
                _currentPoint = 0;
            }
        }
    }

    private void AttackPlayer()
    {
        if(PlayerTransform)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerTransform.position, _speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Points[_currentPoint].position, _speed);

            MoveToPoints();
        }
    }

    private void FlipEnemy()
    {
        Vector3 targetPoint;

        if(PlayerTransform)
        {
            targetPoint = PlayerTransform.position;
        }
        else
        {
            targetPoint = Points[_currentPoint].position;
        }

        if (transform.position.x > targetPoint.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            enabled = false;
            Damage = 0;

            GetComponent<Animator>().enabled = false;
            Destroy(gameObject, 2f);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            _speed = 0;
            StartCoroutine("CuldownTimer");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerTransform = collision.transform;
            _speed = 0.05f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerTransform = null;
            _speed = 0.1f;
        }
    }
    
    IEnumerator CuldownTimer()
    {
        yield return new WaitForSeconds(_culdown);
        _speed = 0.075f;
    }
}
