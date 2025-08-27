using UnityEngine;

namespace Fighting
{

	[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
	public class WeaponConfig : ScriptableObject
	{
		[SerializeField] float damage = 10;
		[SerializeField] int burstLength = 1;
		[SerializeField] float timeBetweenBursts = 2f;
		[SerializeField] int rayCount = 1;
		[SerializeField] float rayAngle = 90;
		[SerializeField] float timeBetweenBullets = .5f;
		[SerializeField] GameObject equippedPrefab = null;
		[SerializeField] Projectile projectile = null;

		public float Damage => damage;
		public int BurstLength => burstLength;
		public float TimeBetweenBursts => timeBetweenBursts;
		public int RayCount => rayCount;
		public float RayAngle => rayAngle;
		public float TimeBetweenBullets => timeBetweenBullets;


		public GameObject Spawn(Transform parent, Vector2 position)
		{

			GameObject weapon = null;

			if (equippedPrefab != null && parent != null)
			{
				weapon = Instantiate(equippedPrefab, position, Quaternion.identity, parent);
			}

			return weapon;
		}

		public void LaunchProjectile(Transform parent, Vector2 position, Vector2 direction, Quaternion rotation, GameObject instigator, LayerMask targetLayerMask)
		{
			Projectile projectileInstance = Instantiate(projectile, position, rotation, parent);
			projectileInstance.Shoot(instigator, direction, damage, targetLayerMask);
		}
	}
}

