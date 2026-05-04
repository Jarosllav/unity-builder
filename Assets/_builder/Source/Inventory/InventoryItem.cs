using nobodyworks.builder.items;

namespace nobodyworks.builder.inventories
{
    public class InventoryItem
    {
        private Item _item;
        private int _amount;

        public Item Item => _item;
        public int Amount => _amount;
        
        public InventoryItem(Item item, int amount = 1)
        {
            _item = item;
            _amount = amount;
        }

        public InventoryItem(ItemDefinition definition, int amount = 1)
        {
            _item = new Item(definition);
            _amount = amount;
        }

        public void SetAmount(int amount)
        {
            _amount = amount;
        }
    }
}