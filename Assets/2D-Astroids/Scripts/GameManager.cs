using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Making this a singleton so that it can be referenced anywhere.
    public static GameManager instance;


    public ParticleSystem explosion;
    public Player player;
    public float respawnTime = 3.0f;
    public int lives = 3;
    public float respawnInvulnerabilityTime = 3;
    public int score = 0;
    public AsteroidSpawner asteroidSpawner;

    public TMP_Text Score;
    public TMP_Text Lives;

    public GameObject GameOverCanvas;
    public bool isGameOver;
    
    private void Awake()
    {
        isGameOver = false;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    // Called whenever an Astroid is destroyed
    public void AstroidDestroyed(Astroid astroid)
    {
        explosion.transform.position = astroid.transform.position;
        explosion.Play();

        // Increase score based on size, smaller = more points.
        if (astroid.size < 0.75)
        {
            score += 100;
        }
        else if (astroid.size < 1.15f)
        {
            score += 50;
        }
        else
        {
            score += 25;
        }
    }

    // This is called by the player whenever they get hit.
    public void PlayerDied()
    {
        explosion.transform.position = player.transform.position;
        explosion.Play();

        lives--;

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), respawnTime);
        }
    }
    
    private void Respawn()
    {
        Respawn(false);
    }
    
    private void Respawn(bool _newGame)
    {
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        player.gameObject.SetActive(true);
        if(!_newGame)
        {
            // Gives Invulnerability for a few seconds.
            Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
        }
    }

    private void TurnOnCollisions()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void GameOver()
    {
        isGameOver = true;
        GameOverCanvas.gameObject.SetActive(true);
    }
    public void NewGame()
    {
        asteroidSpawner.ClearAsteroids();
        Respawn(true);
        lives = 3;
        score = 0;
        isGameOver = false;
        GameOverCanvas.gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
        Lives.text = "x" + lives;
        Score.text = score.ToString("N0");
    }

}
