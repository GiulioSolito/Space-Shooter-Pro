using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 20f;

    [SerializeField] private GameObject _explosionPrefab;

    public static Action onStartSpawning;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * _rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.25f);
            Destroy(other.gameObject);
            Destroy(explosion, 3f);

            if (onStartSpawning != null)
            {
                onStartSpawning();
            }            
        }
    }
}
