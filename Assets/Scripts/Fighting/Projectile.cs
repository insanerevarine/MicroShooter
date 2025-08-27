using UnityEngine;
using UnityEngine.Events;

namespace Fighting
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] float speed = 10;
		[SerializeField] LayerMask Walls;
		[SerializeField] float maxLifeTime = 5;
		[SerializeField] float lifeAferImpact = 0.1f;
		[SerializeField] UnityEvent onHit;

		Vector2 direction = Vector2.left;
		float damage = 0;
		LayerMask targetLayerMask;
		GameObject instigator = null;
		bool isHit = false;

		void Update()
		{
			if (direction != Vector2.zero)
			{
				Fly(direction);
			}
		}

		void Fly(Vector2 direction)
		{
			transform.Translate(direction * speed * Time.deltaTime);
		}

		public void Shoot(GameObject instigator, Vector2 direction, float damage, LayerMask targetLayerMask)
		{
			this.instigator = instigator;
			this.direction = direction;
			this.damage = damage;
			this.targetLayerMask = targetLayerMask;

			Destroy(gameObject, maxLifeTime);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (IsCollisionInLayerMask(collision, targetLayerMask)
				 || IsCollisionInLayerMask(collision, Walls))
			{
				if (collision.GetComponent<Stats.Health>() != null
				&& IsCollisionInLayerMask(collision, targetLayerMask))
				{
					collision.GetComponent<Stats.Health>().TakeDamage(instigator, damage);
				}

				if (isHit) return;
				onHit.Invoke();
				Destroy(gameObject, lifeAferImpact);
				isHit = true;
			}
		}

		bool IsCollisionInLayerMask(Collider2D collision, LayerMask layerMask)
		{
			return ((1 << collision.gameObject.layer) & layerMask) != 0;
		}
	}
}
