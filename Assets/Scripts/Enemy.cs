using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private int _scoreToAdd = 10;

    [SerializeField] private GameObject _laserPrefab;

    private Animator _anim;
    private AudioSource _audio;

    public static Action onDamagePlayer;
    public static Action<int> onAddScore;

    private WaitForSeconds _deathDelay = new WaitForSeconds(3);
    private float _fireRate = 3f;
    private float _canFire = 0;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        if (_anim == null)
        {
            Debug.LogError("The Enemy Animator Is NULL!");
        }

        if (_audio == null)
        {
            Debug.LogError("The Enemy Audio Source Is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5)
        {
            float randomX = UnityEngine.Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 8f, 0);
        }
    }

    void Shoot()
    {
        if (Time.time > _canFire)
        {
            _fireRate = UnityEngine.Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (onDamagePlayer != null)
            {
                onDamagePlayer();
            }

            StartCoroutine(EnemyDeath());
        }

        else if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (onAddScore != null)
            {
                onAddScore(_scoreToAdd);
            }

            StartCoroutine(EnemyDeath());
        }
    }

    IEnumerator EnemyDeath()
    {
        _audio.Play();
        _anim.SetTrigger("Die");
        _speed = 0;
        Destroy(GetComponent<Collider2D>());
        yield return _deathDelay;
        Destroy(gameObject);
    }
}
