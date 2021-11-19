using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Making this a singleton so that it can be referenced anywhere.
    public static GameManager instance;
    private void Awake()
    {
        isGameOver = false;
        if (instance == null)
        {
            // This makes sure this Game Manager Persists over scenes and doesn't change.
            // Might need to be deleated once all lives are lost to not hold data.                  <<<--- !!! TO DO !!!
            //DontDestroyOnLoad(gameObject);                                                        <<<--- Edited out

            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

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

    
    // Called whenever an Astroid is destroyed
    public void AstroidDestroyed(Astroid astroid)
    {
        this.explosion.transform.position = astroid.transform.position;
        this.explosion.Play();

        // Increase score based on size, smaller = more points.
        if (astroid.size < 0.75)
        {
            this.score += 100;
        }
        else if (astroid.size < 1.15f)
        {
            this.score += 50;
        }
        else
        {
            this.score += 25;
        }
    }

    // Updates the ths lives and score every frame.
    private void Update()
    {
        Lives.text = ("x" + lives);
        Score.text = (score.ToString("N0"));
    }

    // This is called by the player whenever they get hit.
    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        this.lives--;

        if (this.lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    /// <summary>
    /// Respawns Player
    /// </summary>
    /// <param name="_newGame">Put "True" if it is a new game (gives Invulnerability for a few seconds).</param>
    private void Respawn()
    {
        Respawn(false);
    }

    /// <summary>
    /// Respawns Player
    /// </summary>
    /// <param name="_newGame">Put "True" if it is a new game (gives Invulnerability for a few seconds).</param>
    private void Respawn(bool _newGame)
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        this.player.gameObject.SetActive(true);
        if(!_newGame)
        {
            // Gives Invulnerability for a few seconds.
            Invoke(nameof(TurnOnCollisions), this.respawnInvulnerabilityTime);
        }
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
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

}
