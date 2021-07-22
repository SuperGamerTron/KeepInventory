namespace KeepInventory
{
    public class Packets
    {
        public static void ClientSendCanKeepInventory()
        {
            using (Packet packet = new Packet(200))
            {
                ClientSend.SendTCPData(packet);
            }
        }

        public static void ServerHandleCanKeepInventory(int fromClient, Packet packet)
        {
            KeepInventory.playersWithMod.Add(fromClient);
        }

        public static void ClientSendDropItems()
        {
            using (Packet packet = new Packet(201))
            {
                ClientSend.SendTCPData(packet);
            }
        }

        public static void ServerHandleDropItems(int fromClient, Packet packet)
        {
            if (KeepInventory.playersWithMod.Count < GameManager.players.Count)
            {
                ServerSendDropItems(fromClient);
            }
        }

        public static void ServerSendDropItems(int fromClient)
        {
            using (Packet packet = new Packet(202))
            {
                ServerSend.SendTCPData(fromClient, packet);
            }
        }

        public static void ClientHandleDropItems(Packet packet)
        {
            foreach (InventoryCell inventoryCell in InventoryUI.Instance.allCells)
            {
                if (!(inventoryCell.currentItem == null))
                {
                    InventoryUI.Instance.DropItemIntoWorld(inventoryCell.currentItem);
                    inventoryCell.currentItem = null;
                    inventoryCell.UpdateCell();
                }
            }
            InventoryUI.Instance.UpdateAllCells();
        }
    }
}
