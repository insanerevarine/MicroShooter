using System.Collections.Generic;
using UnityEngine;
using Fighting;
using Stats;

namespace Control
{
	// Figting control for player
	// Weapon configuration and launching fire
	public class Fighter : MonoBehaviour
	{
		[SerializeField] Transform handTransform = null;
		[SerializeField] Transform bulletsTransform;
		[SerializeField] LayerMask enemiesLayerMask;
		[SerializeField] WeaponConfig defaultWeapon = null;

		float timeSinceLastAttack = Mathf.Infinity;
		Weapon currentWeapon;

		WeaponConfig currentWeaponConfig;
		public WeaponConfig CurrentWeaponConfig => currentWeaponConfig;

		ContactFilter2D filter;
		Character character;

		private void Awake()
		{
			currentWeaponConfig = defaultWeapon;
			SetupDefaultWeapon();
			character = GetComponent<Character>();
		}

		void Start()
		{
			if (currentWeapon == null)
			{
				EquipWeapon(defaultWeapon);
			}

			filter = new ContactFilter2D();
			filter.SetLayerMask(enemiesLayerMask);
			filter.useTriggers = true;
		}

		void Update()
		{
			timeSinceLastAttack += Time.deltaTime;
		}

		public void Attack()
		{
			if (timeSinceLastAttack > currentWeaponConfig.TimeBetweenBursts)
			{
				timeSinceLastAttack = 0;
				Vector2 direction = character.IsFacingRight ? Vector2.right : Vector2.left;
				currentWeapon.Fire(currentWeaponConfig, bulletsTransform, handTransform.position, direction, gameObject, enemiesLayerMask);
			}
		}


		public void EquipWeapon(WeaponConfig weapon)
		{
			if (weapon == null) return;
			if (currentWeapon != null) Destroy(currentWeapon.gameObject);
			currentWeaponConfig = weapon;
			currentWeapon = AttachWeapon(weapon);
		}

		private Weapon AttachWeapon(WeaponConfig weaponConfig)
		{
			Weapon weapon = weaponConfig.Spawn(handTransform, handTransform.position).GetComponent<Weapon>();
			return weapon;
		}

		private void SetupDefaultWeapon()
		{
			EquipWeapon(defaultWeapon);
		}
	}
}