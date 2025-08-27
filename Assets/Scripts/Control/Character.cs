using UnityEngine;

namespace Control
{
	[RequireComponent(typeof(Mover))]
	// Calculation of movement amount and attack according to input
	public class Character : MonoBehaviour
	{
		[SerializeField] private float maxJumpHeight = 4f;
		[SerializeField] private float minJumpHeight = 1f;

		[SerializeField] private float timeToJumpApex = 0.4f;

		[SerializeField] private float moveSpeed = 6;

		[SerializeField] private float maxJumpVelocity = 8f;
		[SerializeField] private float minJumpVelocity = 8f;
		[SerializeField] private int maxJumps = 3;
		[SerializeField] private float jumpDelay = 0;

		[SerializeField] bool isAffectedByGravity = true;

		[SerializeField] int maxHealth = 100;

		public int MaxHealth => maxHealth;
		public bool IsFacingRight => transform.localScale.x > 0;

		private float gravity;
		private int currentJumps;
		private float lastJumpTime;

		Fighter fighter;

		private float moveAmountXSmoothing, moveAmountYSmoothing;
		private float accelerationTimeAirborne = 0.2f;
		private float accelerationTimeGrounded = 0.1f;

		private Vector2 moveAmount;
		private Vector2 oldMoveAmount;

		private float maxHeightReached = Mathf.NegativeInfinity;
		private bool reachedApex = true;

		private Mover mover;

		Vector2 directionalInput;

		private void Awake()
		{
			mover = GetComponent<Mover>();
			fighter = GetComponent<Fighter>();

		}
		private void Start()
		{
			gravity = isAffectedByGravity ? -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2) : 0;
			maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
			minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
			currentJumps = maxJumps;
			lastJumpTime = Time.time;
		}

		private void Update()
		{
			CalculateJumpHeight();
			CalculateVelocity();
			CalculateMovement();

		}

		void CalculateJumpHeight()
		{
			if (!reachedApex && maxHeightReached >= transform.position.y)
			{
				reachedApex = true;
			}
			maxHeightReached = Mathf.Max(transform.position.y, maxHeightReached);
		}

		void CalculateVelocity()
		{
			ContactFilter2D noFilter = new ContactFilter2D();
			noFilter.NoFilter();

			float targetVelocityX = directionalInput.x * moveSpeed;
			if (isAffectedByGravity)
			{
				moveAmount.x = Mathf.SmoothDamp(moveAmount.x, targetVelocityX, ref moveAmountXSmoothing,
				mover.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);
				moveAmount.y += gravity * Time.deltaTime;
			}
			else
			{
				float targetVelocityY = directionalInput.y * moveSpeed;
				moveAmount.x = Mathf.SmoothDamp(moveAmount.x, targetVelocityX, ref moveAmountXSmoothing,
					accelerationTimeAirborne);
				moveAmount.y = Mathf.SmoothDamp(moveAmount.y, targetVelocityY, ref moveAmountYSmoothing,
					accelerationTimeAirborne);
			}
		}

		void CalculateMovement()
		{
			if (mover.collisions.above || mover.collisions.below)
			{
				mover.Move(moveAmount * Time.deltaTime, directionalInput);
			}
			else
			{
				mover.Move(moveAmount * Time.deltaTime, directionalInput);
			}

			if (mover.collisions.above || mover.collisions.below)
			{
				moveAmount.y = oldMoveAmount.y = 0;
			}

			if (mover.collisions.below)
			{
				currentJumps = maxJumps;
			}
		}

		void CheckInputTurn()
		{
			if (!IsFacingRight && directionalInput.x > 0)
			{
				Turn(true);
			}

			else if (IsFacingRight && directionalInput.x < 0)
			{
				Turn(false);
			}
		}

		void CheckMouseInputTurn(Vector3 mousePosition)
		{
			if (!IsFacingRight && mousePosition.x > transform.position.x)
			{
				Turn(true);
				return;
			}

			if (IsFacingRight && mousePosition.x < transform.position.x)
			{
				Turn(false);
			}
		}

		void Turn(bool isToRight)
		{
			if (IsFacingRight == isToRight) return;

			var scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}

		public void OnJumpInputDown()
		{
			if (currentJumps > 0 && Time.time - lastJumpTime >= jumpDelay)
			{
				// reset
				maxHeightReached = float.NegativeInfinity;
				reachedApex = false;

				moveAmount.y = maxJumpVelocity;
				currentJumps--;
				lastJumpTime = Time.time;
			}
		}

		public void OnJumpInputUp()
		{
			if (moveAmount.y > minJumpVelocity)
			{
				moveAmount.y = minJumpVelocity;
			}
		}

		public void SetDirectionalInput(Vector2 input)
		{
			directionalInput = input;
			CheckInputTurn();
		}

		public void SetMouseInput(Vector3 input)
		{
			CheckMouseInputTurn(input);
		}

		public void FaceTarget(Vector2 target)
		{
			Turn(target.x >= transform.position.x);
		}

		public void Attack()
		{
			if (fighter != null) fighter.Attack();
		}

		public void SetWeapon(Fighting.WeaponConfig config)
		{
			if (fighter != null) fighter.EquipWeapon(config);
		}
	}
}

