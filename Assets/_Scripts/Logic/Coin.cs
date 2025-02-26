using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.IncreaseScore();
        Destroy(gameObject);
    }
}
