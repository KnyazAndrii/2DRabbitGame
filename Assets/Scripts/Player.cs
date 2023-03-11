using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    private float _health = 100f;
    private float _speed = 0.1f;
    private float _jumpForce = 2.5f;
    private float _sprintForce = 0.75f;
    private float _bulletSpeed = 15f;
    private float _teleportSpeed = 0.1f;
    private float _horizontal;
    private bool _isTeleporting;

    public bool IsGrounded;
    public int Coins;

    public TMP_Text TextCoins;
    public Image HealthImage;
    public GameObject bulletPrefab;
    public GameObject firePrefab;
    public Rigidbody2D Rigidbody2D;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        Coins = PlayerPrefs.GetInt("Coins");
    }

    private void Update()
    {
        Animation();
        Shoot();
        HealthBarUpdate();
        AddTextCoins();
        EndGame();
    }


    private void FixedUpdate()
    {
        Move();
        FlipLeft();
        FlipRight();
        Jump();
        Sprint();
        RestartLevel();
    }

    private void Move()
    {
        _horizontal = Input.GetAxis("Horizontal");
        transform.position = new Vector3(transform.position.x + _horizontal * _speed, transform.position.y, transform.position.z);
    }

    private void FlipLeft()
    {
        if(Input.GetKey(KeyCode.A))
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void FlipRight()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _spriteRenderer.flipX = false;
        }
    }

    private void Jump()
    {
        if(Input.GetKey(KeyCode.W) && IsGrounded)
        {
            Rigidbody2D.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.Q) && _horizontal < 0)
        {
            Rigidbody2D.AddForce(transform.right * (-_sprintForce), ForceMode2D.Impulse);
            _spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.E) && _horizontal > 0)
        {
            Rigidbody2D.AddForce(transform.right * _sprintForce, ForceMode2D.Impulse);
            _spriteRenderer.flipX = false;
        }
    }

    private void Animation()
    {
        if(_isTeleporting)
        {
            _animator.Play("TeleportPlayer");

            Vector3 portalCenter = new Vector3(11.5f, -0.7f, 0f);
            transform.position = Vector3.MoveTowards(transform.position, portalCenter, _teleportSpeed);
        }
        else if (IsGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0 && Input.GetKey(KeyCode.LeftControl))
            {
                _speed = 0.05f;
                _animator.Play("SneakingPlayer");
            }
            else if (Input.GetAxis("Horizontal") != 0)
            {
                _speed = 0.1f;
                _animator.Play("RunPlayer");
            }
            else
            {
                _animator.Play("IdlePlayer");
            }
        }
        else
        { 
            _animator.Play("JumpPlayer");
        }
    }

    private void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_spriteRenderer.flipX == true)
            {
                Vector3 _spawnPositionFire = new Vector3(transform.position.x - 0.719f, transform.position.y - 0.156f, transform.position.z);
                Vector3 _spawnPositionBullet = new Vector3(transform.position.x - 0.2f, transform.position.y - 0.15f, transform.position.z);
                GameObject newBullet = Instantiate(bulletPrefab, _spawnPositionBullet, Quaternion.identity);
                GameObject newFire = Instantiate(firePrefab, _spawnPositionFire, Quaternion.identity);

                newBullet.GetComponent<Rigidbody2D>().AddForce(-newBullet.transform.right * _bulletSpeed, ForceMode2D.Impulse);
                newBullet.transform.Rotate(0, 180, 0);
                newFire.transform.Rotate(0, 180, 0);
            }
            else
            {
                Vector3 _spawnPositionFire = new Vector3(transform.position.x + 0.719f, transform.position.y - 0.156f, transform.position.z);
                Vector3 _spawnPositionBullet = new Vector3(transform.position.x + 0.2f, transform.position.y - 0.15f, transform.position.z);
                GameObject newBullet = Instantiate(bulletPrefab, _spawnPositionBullet, Quaternion.identity);
                GameObject newFire = Instantiate(firePrefab, _spawnPositionFire, Quaternion.identity);

                newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.transform.right * _bulletSpeed, ForceMode2D.Impulse);
                newBullet.transform.Rotate(0, 0, 0);
                newFire.transform.Rotate(0, 0, 0);
            }
        }
    }

    private void AddTextCoins()
    {
        TextCoins.text = "Coins: " + Coins;
    }

    private void RestartLevel()
    {
        if((gameObject.transform.position.y < -6) || (_health <= 0))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void HealthBarUpdate()
    {
        HealthImage.fillAmount = _health / 100;
    }

    private void EndGame()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            _health -= collision.gameObject.GetComponent<Enemy>().Damage;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Portal"))
        {
            _isTeleporting = true;
        }
    }
}