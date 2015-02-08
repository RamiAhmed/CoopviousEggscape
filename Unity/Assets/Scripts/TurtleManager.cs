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

        _turtlesList = new List<GameObject>();

        for (int i = 0; i < maxTurtles; i++)
        {
            var newTurtle = Instantiate(turtlePrefab, spawningWaypoints[0].transform.position, Quaternion.identity) as GameObject;            
            newTurtle.SetActive(false);
            _turtlesList.Add(newTurtle);
        }

        _currentSpawnIndex = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_activated)
        {
            return;
        }

        float currentTime = Time.time;
        if (currentTime - _lastSpawn <= 1f / maxSpawnsPerSecond)
        {
            return;
        }

        _lastSpawn = currentTime;


        GameObject turtleToSpawn = _turtlesList[_currentSpawnIndex];
        if (turtleToSpawn.activeSelf)
        {
            return;
        }

        _currentSpawnIndex = _currentSpawnIndex + 1 > maxSpawnsPerSecond ? 0 : _currentSpawnIndex++;

        int random = Random.Range(0, spawningWaypoints.Length);
        Vector3 spawnPos = spawningWaypoints[random].transform.position;

        turtleToSpawn.transform.position = spawnPos;
        turtleToSpawn.SetActive(true);

        var turtleController = turtleToSpawn.GetComponent<TurtleController>();
        turtleController.targetPlayer = _players[Random.Range(0, _players.Length)];
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        _activated = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        _activated = false;
    }
}