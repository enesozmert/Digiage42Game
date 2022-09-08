using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(destroyDelay());
    }
    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
