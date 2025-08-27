using UnityEngine;
using Control;

namespace Stats
{
	public class Health : MonoBehaviour
	{
		[SerializeField] TakeDamageEvent takeDamage = null;
		[SerializeField] UnityEngine.Events.UnityEvent onDie;

		[System.Serializable]
		public class TakeDamageEvent : UnityEngine.Events.UnityEvent<float> { }

		private Character character;

		private float healthPoints;

		bool isDead;

		private void Awake()
		{
			character = GetComponent<Character>();
			healthPoints = GetInitialHealth();
		}

		private float GetInitialHealth()
		{
			return character.MaxHealth;
		}

		public void TakeDamage(GameObject instigator, float damage)
		{
			if (isDead) return;

			healthPoints = Mathf.Max(healthPoints - damage, 0);

			if (healthPoints == 0)
			{
				onDie.Invoke();
				Die();
			}
			else
			{
				takeDamage.Invoke(damage);
			}
		}

		public float GetHealthPoints()
		{
			return healthPoints;
		}

		public float GetMaxHealthPoints()
		{
			return character.MaxHealth;
		}

		public float GetPercentage()
		{
			return healthPoints / character.MaxHealth * 100;
		}

		public float GetFraction()
		{
			return healthPoints / character.MaxHealth;
		}

		public bool IsDead()
		{
			return healthPoints <= 0;
		}

		private void Die()
		{
			if (isDead)
				return;
			isDead = true;
			Destroy(gameObject, 0.2f);
		}

	}
}
