using HarmonyLib;
using UnityEngine;

namespace KeepInventory
{
    public class MuckPatch
    {
		[HarmonyPostfix, HarmonyPatch(typeof(Server), nameof(Server.InitializeServerPackets))]
		public static void InitializeServerPackets()
        {
			Server.PacketHandlers.Add(200, new Server.PacketHandler(Packets.ServerHandleCanKeepInventory));
			Server.PacketHandlers.Add(201, new Server.PacketHandler(Packets.ServerHandleDropItems));
        }

		[HarmonyPostfix, HarmonyPatch(typeof(LocalClient), nameof(LocalClient.InitializeClientData))]
		public static void InitializeClientData()
		{
			LocalClient.packetHandlers.Add(202, new LocalClient.PacketHandler(Packets.ClientHandleDropItems));
		}

		[HarmonyPostfix, HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
		public static void OnStartGame()
        {
			KeepInventory.playersWithMod.Clear();
			Packets.ClientSendCanKeepInventory();
        }

		[HarmonyPrefix, HarmonyPatch(typeof(PlayerStatus), nameof(PlayerStatus.PlayerDied))]
		public static bool OnPlayerDied(ref int damageType, ref int damageFromPlayer)
        {
			PlayerStatus.Instance.hp = 0f;
			PlayerStatus.Instance.shield = 0f;
			PlayerMovement.Instance.gameObject.SetActive(false);
			PlayerStatus.Instance.dead = true;
			GameManager.players[LocalClient.instance.myId].dead = true;
			Hotbar.Instance.UpdateHotbar();
			ClientSend.PlayerDied(damageFromPlayer);
			PlayerRagdoll component = Object.Instantiate(PlayerStatus.Instance.playerRagdoll, PlayerMovement.Instance.transform.position, PlayerMovement.Instance.orientation.rotation).GetComponent<PlayerRagdoll>();
			MoveCamera.Instance.PlayerDied(component.transform.GetChild(0).GetChild(0).GetChild(0));
			component.SetRagdoll(LocalClient.instance.myId, -component.transform.forward);
			GameManager.players[LocalClient.instance.myId].dead = true;
			if (InventoryUI.Instance.gameObject.activeInHierarchy)
			{
				OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
			}
			for (int j = 0; j < PlayerStatus.Instance.armor.Length; j++)
			{
				PlayerStatus.Instance.UpdateArmor(j, -1);
			}
			AchievementManager.Instance.AddDeath((PlayerStatus.DamageType)damageType);
			Packets.ClientSendDropItems();
			return false;
        }
    }
}
