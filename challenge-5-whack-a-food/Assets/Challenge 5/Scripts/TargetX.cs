using System.Collections;
using UnityEngine;

public class TargetX : MonoBehaviour
{
    private const float MinValueX = -3.75f; // the x value of the center of the left-most square
    private const float MinValueY = -3.75f; // the y value of the center of the bottom-most square
    private const float SpaceBetweenSquares = 2.5f; // the distance between the centers of squares on the game board

    private Rigidbody _rb;
    private GameManagerX _gameManagerX;

    public int pointValue;
    public GameObject explosionFx;
    public float timeOnScreen = 1.0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gameManagerX = GameObject.Find("Game Manager").GetComponent<GameManagerX>();

        transform.position = RandomSpawnPosition(); 
        StartCoroutine(RemoveObjectRoutine()); // begin timer before target leaves screen
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = MinValueX + (RandomSquareIndex() * SpaceBetweenSquares);
        float spawnPosY = MinValueY + (RandomSquareIndex() * SpaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;
    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex ()
    {
        return Random.Range(0, 4);
    }

    private void OnMouseDown()
    {
        if (_gameManagerX.isGameActive)
        {
            Destroy(gameObject);
            _gameManagerX.UpdateScore(pointValue);
            Instantiate(explosionFx, transform.position, explosionFx.transform.rotation);
        }
    }

    // If target that is NOT the bad object collides with sensor, trigger game over
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (other.gameObject.CompareTag("Sensor") && !gameObject.CompareTag("Bad"))
        {
            _gameManagerX.GameOver();
        }
    }

    // After a delay, Moves the object behind background so it collides with the Sensor object
    IEnumerator RemoveObjectRoutine()
    {
        yield return new WaitForSeconds(timeOnScreen);

        if (_gameManagerX.isGameActive)
        {
            transform.Translate(Vector3.forward * 5, Space.World);
        }
    }
}
