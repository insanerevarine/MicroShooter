using UnityEngine;
using Control;

namespace Enemies
{

	[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
	public class EnemyConfig : ScriptableObject
	{
		[SerializeField] float damage;
		[SerializeField] float timeBetweenAttaks = 1;
		[SerializeField] GameObject enemyPrefab;

		public float Damage => damage;
		public float TimeBetweenAttaks => timeBetweenAttaks;


		public GameObject Spawn(Transform parent, Vector2 position)
		{
			var enemy = Instantiate(enemyPrefab, position, Quaternion.identity, parent);
			var controller = enemy.GetComponent<EnemyController>();
			if (controller != null)
			{
				controller.SetConfig(this);
			}
			return enemy;
		}
	}
}