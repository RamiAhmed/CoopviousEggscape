using System.Collections.Generic;
using UnityEngine;

public class TurtleManager : MonoBehaviour
{
    public GameObject[] spawningWaypoints;
    public int maxTurtles = 100;
    public GameObject turtlePrefab;
    public float maxSpawnsPerSecond = 0.5f;

    private float _lastSpawn;
    private List<GameObject> _turtlesList;
    private bool _activated;
    private int _currentSpawnIndex;
    private GameObject[] _players;

    private GameController _gameController;

    // Use this for initialization
    private void Start()
    {
        if (turtlePrefab == null)
        {
            Debug.LogError(this.gameObject.name + " is missing its turtlePrefab reference");
            return;
        }

        if (spawningWaypoints == null || spawningWaypoints.Length == 0)
        {
            Debug.LogError(this.gameObject.name + " could not find any spawning waypoints");
            return;
        }

        _players = GameObject.FindGameObjectsWithTag("Player");
        if (_players == null || _players.Length == 0)
        {
            Debug.LogError(this.gameObject.name + " could not find any players tagged with Player");
            return;
        }

        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (_gameController == null)
        {
            Debug.LogError(this.gameObject.name + " could not find the GameController game object and its GameController component");
            return;
        }

        _turtlesList = new List<GameObject>();

        for (int i = 0; i < maxTurtles; i++)
        {
            var newTurtle = Instantiate(turtlePrefab, spawningWaypoints[0].transform.position, Quaternion.identity) as GameObject;
            newTurtle.transform.parent = this.transform;
            newTurtle.SetActive(false);
            _turtlesList.Add(newTurtle);
        }

        _currentSpawnIndex = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_gameController.gameState == GameController.GameState.MENU)
        {
            return;
        }

        if (!_activated)
        {
            return;
        }

        float currentTime = Time.time;
        if (currentTime - _lastSpawn > 1f / maxSpawnsPerSecond)
        {
            _lastSpawn = currentTime;

            GameObject turtleToSpawn = _turtlesList[_currentSpawnIndex];
            if (turtleToSpawn.activeSelf)
            {
                return;
            }

            _currentSpawnIndex = _currentSpawnIndex + 1 > maxTurtles ? 0 : _currentSpawnIndex + 1;

            int random = Random.Range(0, spawningWaypoints.Length);
            Vector3 spawnPos = spawningWaypoints[random].transform.position;

            turtleToSpawn.transform.position = spawnPos;
            turtleToSpawn.SetActive(true);

            var turtleController = turtleToSpawn.GetComponent<TurtleController>();
            turtleController.targetPlayer = _players[Random.Range(0, _players.Length)];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
        {
            return;
        }

        _activated = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
        {
            return;
        }

        _activated = false;
    }
}