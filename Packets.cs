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

        public static void ClientSendKeepInventory()
        {
            using (Packet packet = new Packet(201))
            {
                ClientSend.SendTCPData(packet);
            }
        }

        public static void ServerHandleKeepInventory(int fromClient, Packet packet)
        {
            ServerSendKeepInventory(fromClient);
        }

        public static void ServerSendKeepInventory(int fromClient)
        {
            using (Packet packet = new Packet(202))
            {
                packet.Write(KeepInventory.playersWithMod.Count == GameManager.players.Count);
                ServerSend.SendTCPData(fromClient, packet);
            }
        }

        public static void ClientHandleKeepInventory(Packet packet)
        {
            bool canKeepInventory = packet.ReadBool();
            if (!canKeepInventory)
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
}
