using UnityEngine;

namespace UI
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] Stats.Health health = null;
		[SerializeField] RectTransform foreground = null;
		[SerializeField] Canvas rootCanvas = null;

		void Update()
		{
			var healthFraction = health.GetFraction();
			if (Mathf.Approximately(healthFraction, 0))
			{
				rootCanvas.enabled = false;
				return;
			}
			foreground.localScale = new Vector3(healthFraction, 1, 1);
		}
	}
}
