using UnityEngine;

namespace Control
{
	// Calculations for raycast collision detections
	public class RaycastController : MonoBehaviour
	{
		[SerializeField] protected LayerMask collisionMask;
		protected const float skinWidth = 0.015f;
		[SerializeField] float distanceBetweenRays = 0.25f;
		protected int horizontalRayCount = 4;
		protected int verticalRayCount = 4;

		protected BoxCollider2D boxCollider2D;
		protected RaycastOrigins raycastOrigins;

		protected float horizontalRaySpacing;
		protected float verticalRaySpacing;


		public CollisionInfo collisions;

		private void Awake()
		{
			boxCollider2D = GetComponent<BoxCollider2D>();
		}

		void Start()
		{
			CalculateRaySpacing();
		}

		protected void UpdateRaycastOrigins()
		{
			Bounds bounds = boxCollider2D.bounds;
			bounds.Expand(skinWidth * -2);

			raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
			raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
			raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
			raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
		}

		void CalculateRaySpacing()
		{
			Bounds bounds = boxCollider2D.bounds;
			bounds.Expand(skinWidth * -2);

			float boundsWidth = bounds.size.x;
			float boundsHeight = bounds.size.y;

			horizontalRayCount = Mathf.RoundToInt(boundsHeight / distanceBetweenRays);
			verticalRayCount = Mathf.RoundToInt(boundsWidth / distanceBetweenRays);

			horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
			verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		}

		protected struct RaycastOrigins
		{
			public Vector2 TopLeft, TopRight;
			public Vector2 BottomLeft, BottomRight;
		}

		protected void ResetFallingThroughPlatform()
		{
			collisions.fallingThroughPlatform = false;
		}

		public struct CollisionInfo
		{
			public bool above, below, left, right;
			public bool climbingSlope, descendingSlope;
			public float slopeAngle, slopeAngleOld;
			public bool fallingThroughPlatform;

			public void Reset()
			{
				above = below = left = right = false;
				climbingSlope = descendingSlope = false;
				slopeAngleOld = slopeAngle;
				slopeAngle = 0;
			}
		}
	}
}