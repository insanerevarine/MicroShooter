using UnityEngine;

namespace Control
{
	// Calculation of avaliable movement according to collisions
	public class Mover : RaycastController
	{
		[SerializeField] float maxClimbAngle = 80;
		[SerializeField] float maxDescendAngle = 80;

		Vector2 moveAmountOld;		

		void VerticalCollisions(ref Vector2 moveAmount)
		{
			float directionY = Mathf.Sign(moveAmount.y);
			float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;


			for (int i = 0; i < verticalRayCount; i++)
			{
				Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);

				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

				if (hit)
				{
					moveAmount.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;

					if (collisions.climbingSlope)
					{
						moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
					}

					collisions.below = directionY == -1;
					collisions.above = directionY == 1;
				}
			}

			if (collisions.climbingSlope)
			{
				float directionX = Mathf.Sign(moveAmount.x);
				rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
				Vector2 rayOrigin = (directionX == -1 ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight)
					+ Vector2.up * moveAmount.y;
				var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
				if (hit)
				{
					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
					if (slopeAngle != collisions.slopeAngle)
					{
						moveAmount.x = (hit.distance - skinWidth) * directionX;
						collisions.slopeAngle = slopeAngle;
					}
				}
			}
		}

		void HorizontalCollisions(ref Vector2 moveAmount)
		{
			float directionX = Mathf.Sign(moveAmount.x);
			float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

			if (Mathf.Abs(moveAmount.x) < skinWidth)
			{
				rayLength = 2 * skinWidth;
			}

			for (int i = 0; i < horizontalRayCount; i++)
			{
				Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);

				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

				if (hit)
				{
					if (hit.distance == 0)
					{
						continue;
					}
					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);


					if (i == 0 && slopeAngle <= maxClimbAngle  // bottom ray
						) //|| slopeAngle > maxClimbAngle && CanClimbTile(moveAmount)) // climb one tile
					{
						if (collisions.descendingSlope)
						{
							collisions.descendingSlope = false;
							moveAmount = moveAmountOld;
						}
						float distanceToSlopeStart = 0;
						if (slopeAngle != collisions.slopeAngleOld)
						{
							distanceToSlopeStart = hit.distance - skinWidth;
							moveAmount.x -= distanceToSlopeStart * directionX;
						}
						ClimbSlope(ref moveAmount, slopeAngle);
						moveAmount.x += distanceToSlopeStart * directionX;
					}

					if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
					{
						//moveAmount.x = (hit.distance - skin) * directionX;
						//rayLength = hit.distance;
						moveAmount.x = Mathf.Min(Mathf.Abs(moveAmount.x), (hit.distance - skinWidth)) * directionX;
						rayLength = Mathf.Min(Mathf.Abs(moveAmount.x) + skinWidth, hit.distance);

						if (collisions.climbingSlope)
						{
							moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
						}

						collisions.left = directionX == -1;
						collisions.right = directionX == 1;
					}
				}
			}
		}

		void ClimbSlope(ref Vector2 moveAmount, float slopeAngle)
		{
			float moveDistance = Mathf.Abs(moveAmount.x);
			float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
			if (moveAmount.y > climbVelocityY)
			{
				// jump on slope
			}

			else
			{
				moveAmount.y = climbVelocityY;
				moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
				collisions.below = true;
				collisions.climbingSlope = true;
				collisions.slopeAngle = slopeAngle;
			}
		}

		void DescendSlope(ref Vector2 moveAmount)
		{
			float directionX = Mathf.Sign(moveAmount.x);
			Vector2 rayOrigin = directionX == -1 ? raycastOrigins.BottomRight : raycastOrigins.BottomLeft;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
				{
					if (Mathf.Sign(hit.normal.x) == directionX)
					{
						if (hit.distance - skinWidth <=
							Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
						{
							float moveDistance = Mathf.Abs(moveAmount.x);
							float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
							moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
							moveAmount.y -= descendVelocityY;

							collisions.slopeAngle = slopeAngle;
							collisions.descendingSlope = true;
							collisions.below = true;
						}
					}
				}
			}
		}

		public void Move(Vector2 moveAmount, Vector2 input)
		{
			UpdateRaycastOrigins();
			collisions.Reset();
			moveAmountOld = moveAmount;

			if (moveAmount.y < 0)
			{
				DescendSlope(ref moveAmount);
			}

			if (moveAmount.x != 0)
			{
				HorizontalCollisions(ref moveAmount);
			}

			if (moveAmount.y != 0)
			{
				VerticalCollisions(ref moveAmount);
			}
			transform.Translate(moveAmount);

		}

		public bool IsGrounded()
		{
			return collisions.below;
		}
	}
}