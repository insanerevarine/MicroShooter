using UnityEngine;

namespace Enemies
{
	// Spawning enemies of given configurations at given coordinates
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField] Transform[] spawnPositions;
		[SerializeField] float spawnDelay = 3f;
		[SerializeField] EnemyConfig[] enemies;
		[SerializeField] Transform parent;
		[SerializeField] Transform player;

		float lastSpawnTime = 0;

		private void Update()
		{
			if (Time.time - lastSpawnTime >= spawnDelay)
			{
				lastSpawnTime = Time.time;
				SpawnEnemy();
			}
		}

		void SpawnEnemy()
		{
			int posIndex = Random.Range(0, spawnPositions.Length);
			int enemyIndex = Random.Range(0, enemies.Length);
			Vector2 spawnPosition = spawnPositions[posIndex].position;
			var enemyConf = enemies[enemyIndex];

			var enemy = enemyConf.Spawn(parent, spawnPosition);
			var controller = enemy.GetComponent<Control.EnemyController>();
			if (controller != null)
			{
				controller.SetTarget(player);
			}
		}
	}
}
