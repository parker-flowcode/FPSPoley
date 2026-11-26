using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Configuración de Moneda")]
    public int value = 1;
    public float respawnTime = 5f;
    public float rotationSpeed = 90f;

    private Collider[] colliders;
    private Renderer[] renderers;
    private bool isCollected = false;

    void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if (!isCollected)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddCoin(value);
            }
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        isCollected = true;
        SetCoinActive(false);

        yield return new WaitForSeconds(respawnTime);

        SetCoinActive(true);
        isCollected = false;
    }

    private void SetCoinActive(bool active)
    {
        foreach (Collider col in colliders)
        {
            col.enabled = active;
        }

        foreach (Renderer rend in renderers)
        {
            rend.enabled = active;
        }
    }
}
