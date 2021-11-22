using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
	private GameObject gameGameObject;
	private GameManager game;
	private AsteroidSpawner asteroidSpawner;
	private Player player;
	private float waitTime = 0.1f;

	[SetUp]
	public void Setup()
	{
		gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
		game = gameGameObject.GetComponent<GameManager>();
		asteroidSpawner = gameGameObject.GetComponentInChildren<AsteroidSpawner>();
		player = gameGameObject.GetComponentInChildren<Player>();
	}
	
	[TearDown]
	public void Teardown()
	{
		// Destroy the game after the test so nothing is left in the scene.
		Object.Destroy(game.gameObject);
	}
	
	[UnityTest]
	public IEnumerator AsteroidMoves()
	{
		// Spawns one asteroid as asteroid.
		GameObject asteroid = asteroidSpawner.SpawnOneAsteroid();
		Vector2 initialPos = asteroid.transform.position;
		yield return new WaitForSeconds(waitTime);
		
		// Checks if the new position is different to the old and if so it passes the test.
		Assert.AreNotEqual(asteroid.transform.position, initialPos);
	}
	
	[UnityTest]
	public IEnumerator GameOverOnPlayerCollision()
	{
		int initialLives = game.lives;
		GameObject asteroid = asteroidSpawner.SpawnOneAsteroid();
		
		// Puts the asteroid ontop of the player.
		asteroid.transform.position = player.gameObject.transform.position;
		yield return new WaitForSeconds(waitTime);
		
		// Checks if the player has lost lives.
		Assert.Less(game.lives, initialLives);
	}
	
	[UnityTest]
	public IEnumerator NewGameRestartsGame()
	{
		game.isGameOver = true;
		game.NewGame();
		Assert.False(game.isGameOver);
		yield return null;
	}
	
	[UnityTest]
	public IEnumerator BulletsMove()
	{
		GameObject bullet = player.Shoot();
		Vector2 initialYPos = bullet.transform.position;

		yield return new WaitForSeconds(waitTime);
		
		// Checks if it now has a different position.
		Assert.AreNotEqual(bullet.transform.position, initialYPos);
	}
	
	[UnityTest]
	public IEnumerator BulletDestroysAsteroid()
	{
		// Spawns an asteroid at origin
		GameObject asteroid = asteroidSpawner.SpawnOneAsteroid();
		asteroid.transform.position = Vector3.zero;
		
		// Shoots bullet
		GameObject bullet = player.Shoot();
		bullet.transform.position = Vector3.zero;
		
		yield return new WaitForSeconds(waitTime);
		
		// Use Unity to check if null.
		UnityEngine.Assertions.Assert.IsNull(asteroid);
	}
}