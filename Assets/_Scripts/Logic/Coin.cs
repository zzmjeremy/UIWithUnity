using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    private ScoreManager scoreManager;
    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        scoreManager.IncreaseScore();
        Destroy(gameObject);
    }
}
