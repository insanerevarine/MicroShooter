using UnityEngine;
using Stats;
using Enemies;

namespace Control
{
	// Simple AI for enemies' behaviour
	// Enemiy targets to player and try to touch to deal damage
	public class EnemyController : MonoBehaviour
	{
		[SerializeField] LayerMask playerLayerMask;
		Transform target;
		Character character;
		EnemyConfig config;

		float lastAttackTime = 0;

		private void Awake()
		{
			character = GetComponent<Character>();
		}

		public void SetConfig(EnemyConfig config)
		{
			this.config = config;
		}

		public void SetTarget(Transform target)
		{
			this.target = target;
		}

		void MoveToTarget()
		{
			if (target == null) return;

			Vector2 direction = target.position - transform.position;
			character.SetDirectionalInput(direction.normalized);
		}

		private void Update()
		{
			if (target == null) return;
			MoveToTarget();
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (IsCollisionInLayerMask(collision, playerLayerMask))
			{
				if (CanAttack() && collision.GetComponent<Health>() != null
				&& IsCollisionInLayerMask(collision, playerLayerMask))
				{
					collision.GetComponent<Health>().TakeDamage(gameObject, config.Damage);
					lastAttackTime = Time.time;
				}
			}
		}

		bool IsCollisionInLayerMask(Collider2D collision, LayerMask layerMask)
		{
			return ((1 << collision.gameObject.layer) & layerMask) != 0;
		}

		bool CanAttack()
		{
			if (Time.time - lastAttackTime >= config.TimeBetweenAttaks)
				return true;
			else
				return false;
		}
	}
}