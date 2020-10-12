using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedMultiplier = 2f;

    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;

    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = 0f;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;

    [SerializeField] private GameObject _shieldVisual;
    [SerializeField] private GameObject[] _engines;

    [SerializeField] private AudioClip _laserClip;

    private AudioSource _audio;

    private bool _isTripleShotPowerupActive = false;
    private bool _isShieldPowerupActive = false;

    public static Action<int> onUpdateScoreUI;
    public static Action<int> onUpdateLivesUI;
    public static Action onPlayerDeath;

    void OnEnable()
    {
        Enemy.onDamagePlayer += Damage;
        Enemy.onAddScore += AddScore;
        Laser.onDamagePlayer += Damage;
    }

    void Start()
    {
        _audio = GetComponent<AudioSource>();

        if (_audio == null)
        {
            Debug.LogError("The Player Audio Source Is NULL.");
        }
        else
        {
            _audio.clip = _laserClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shoot();
        }
    }

    void Move()
    {
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        
        transform.Translate(new Vector3(horInput, vertInput, 0) * _speed * Time.deltaTime);
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11f, transform.position.y, transform.position.z);
        }

        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11f, transform.position.y, transform.position.z);
        }
    }

    void Shoot()
    {        
        _canFire = Time.time + _fireRate;

        if (_isTripleShotPowerupActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
                
        _audio.Play();
    }

    public void Damage()
    {
        if (_isShieldPowerupActive)
        {
            _isShieldPowerupActive = false;
            _shieldVisual.SetActive(false);
            return;
        }

        _lives--;

        switch (_lives)
        {
            case 1:
                _engines[0].SetActive(true);
                break;
            case 2:
                _engines[1].SetActive(true);
                break;
        }

        if (onUpdateLivesUI != null)
        {
            onUpdateLivesUI(_lives);
        }

        if (_lives < 1)
        {
            if (onPlayerDeath != null)
            {
                onPlayerDeath();
            }
            
            Destroy(gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotPowerupActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotPowerupActive = false;
    }

    public void SpeedPowerupActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDown());
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5);
        _speed /= _speedMultiplier;
    }

    public void ShieldPowerupActive()
    {
        _isShieldPowerupActive = true;
        _shieldVisual.SetActive(true);
    }

    public void AddScore(int amount)
    {
        _score += amount;

        if (onUpdateScoreUI != null)
        {
            onUpdateScoreUI(_score);
        }
    }

    void OnDisable()
    {
        Enemy.onDamagePlayer -= Damage;
        Enemy.onAddScore -= AddScore;
        Laser.onDamagePlayer -= Damage;
    }
}
