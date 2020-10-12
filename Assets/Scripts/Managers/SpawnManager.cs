using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyContainer;

    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private Transform _powerupContainer;

    [SerializeField] private bool _stopSpawning = false;

    private WaitForSeconds _startSpawningDelay = new WaitForSeconds(3f);
    private WaitForSeconds _spawnEnemyDelay = new WaitForSeconds(5f);
    private WaitForSeconds _spawnPowerupDelay;

    void OnEnable()
    {
        Asteroid.onStartSpawning += StartSpawning;
        Player.onPlayerDeath += OnPlayerDeath;
    }

    void Start()
    {
        _spawnPowerupDelay = new WaitForSeconds(Random.Range(3f, 7f));
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnEnemy()
    {
        yield return _startSpawningDelay;

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject enemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer);
            yield return _spawnEnemyDelay;
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return _startSpawningDelay;

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            int randomPowerup = Random.Range(0, _powerups.Length);
            GameObject powerup = Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            powerup.transform.SetParent(_powerupContainer);
            yield return _spawnPowerupDelay;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    void OnDisable()
    {
        Asteroid.onStartSpawning -= StartSpawning;
        Player.onPlayerDeath -= OnPlayerDeath;
    }
}
