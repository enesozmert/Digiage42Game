using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    EnemyStats _enemyStats;
    private GameObject _player;
    private playerMove _playerMove;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMove = _player.GetComponent<playerMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemyStats = other.GetComponent<EnemyStats>();
            _enemyStats.EnemyHp -= 20;
            if (_playerMove._lookRight)
                other.gameObject.GetComponent<Rigidbody>().AddForce(150, 100, 0);
            else
                other.gameObject.GetComponent<Rigidbody>().AddForce(-150, 100, 0);
            if (_enemyStats.EnemyHp <= 0)
                Destroy(other.gameObject);
            Debug.Log(_enemyStats.EnemyHp);
        }
    }
}
