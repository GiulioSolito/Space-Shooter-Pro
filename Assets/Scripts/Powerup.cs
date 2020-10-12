using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { tripleShot, speed, shield };

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    [SerializeField] private PowerupType _powerupType;

    public static Action onPowerupPickup;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerupType)
                {
                    case PowerupType.tripleShot:
                        player.TripleShotActive();
                        break;
                    case PowerupType.speed:
                        player.SpeedPowerupActive();
                        break;
                    case PowerupType.shield:
                        player.ShieldPowerupActive();
                        break;
                }
            }

            if (onPowerupPickup != null)
            {
                onPowerupPickup();
            }

            Destroy(gameObject);
        }
    }
}
