using System.Collections;
using UnityEngine;

namespace Fighting
{
	// Calculations for shooting 
	public class Weapon : MonoBehaviour
	{
		[SerializeField] Transform handle;
		[SerializeField] Transform barrel;

		private IEnumerator FireBurstRoutine(WeaponConfig config, Transform parent, Vector2 startPoint, Vector2 direction, Quaternion rotation, GameObject instigator, LayerMask targetLayerMask)
		{
			for (int i = 0; i < config.BurstLength; i++)
			{

				config.LaunchProjectile(parent, startPoint, direction, rotation, instigator, targetLayerMask);
				yield return new WaitForSeconds(config.TimeBetweenBullets);
			}
		}

		private void FireBurst(WeaponConfig config, Transform parent, Vector2 startPoint, Vector2 direction, Quaternion rotation, GameObject instigator, LayerMask targetLayerMask)
		{
			StartCoroutine(FireBurstRoutine(config, parent, startPoint, direction, rotation, instigator, targetLayerMask));
		}

		private void FireSplash(WeaponConfig config, Transform parent, Vector2 startPoint, Vector2 generalDirection, GameObject instigator, LayerMask targetLayerMask)
		{

			float angleStep = config.RayAngle / config.RayCount;
			float aimingAngle = gameObject.transform.rotation.eulerAngles.z;
			float centeringOffset = (config.RayAngle / 2) - (angleStep / 2);

			for (int i = 0; i < config.RayCount; i++)
			{
				float currentBulletAngle = angleStep * i;

				Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, aimingAngle + currentBulletAngle - centeringOffset));
				FireBurst(config, parent, startPoint, generalDirection, rotation, instigator, targetLayerMask);
			}
		}

		public void Fire(WeaponConfig config, Transform parent, Vector2 startPoint, Vector2 generalDirection, GameObject instigator, LayerMask targetLayerMask)
		{
			FireSplash(config, parent, startPoint, generalDirection, instigator, targetLayerMask);
		}
	}
}