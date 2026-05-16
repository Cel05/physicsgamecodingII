using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;

    public float forwardForce = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnBall();
        }
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb != null && forwardForce > 0f)
        {
            rb.AddForce(spawnPoint.forward * forwardForce, ForceMode.Impulse);
        }
    }
}