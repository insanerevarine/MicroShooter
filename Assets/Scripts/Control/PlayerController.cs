using UnityEngine;
using Fighting;

namespace Control
{
	//Processesing of player input
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		WeaponKeys[] weaponKeys;

		Character player;

		private void Awake()
		{
			player = GetComponent<Character>();
		}

		void Update()
		{
			ManageMoveInput();
			ManageAttackInput();
			ManageChangeWeaponInput();
		}

		void ManageMoveInput()
		{
			Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			player.SetDirectionalInput(directionalInput);

			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
			{
				player.OnJumpInputDown();
			}

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
			{
				player.OnJumpInputUp();
			}

			if (Input.GetMouseButton(0))
			{
				var worldCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				player.SetMouseInput(worldCoord);
			}
		}

		void ManageAttackInput()
		{
			if (Input.GetMouseButton(0))
			{
				player.Attack();
			}
		}

		void ManageChangeWeaponInput()
		{
			foreach (var weaponKey in weaponKeys)
			{
				if (Input.GetKeyDown(weaponKey.keyCode))
				{
					player.SetWeapon(weaponKey.config);
				}
			}
		}
		[System.Serializable]
		struct WeaponKeys
		{
			public KeyCode keyCode;
			public WeaponConfig config;
		}

	}
}