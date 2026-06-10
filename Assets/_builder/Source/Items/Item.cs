namespace nobodyworks.builder.items
{
    public class Item
    {
        private readonly ItemDefinition _definition;

        public ItemDefinition Definition => _definition;
        
        public Item(ItemDefinition definition)
        {
            _definition = definition;
        }

        public static implicit operator ItemDefinition(Item item)
        {
            return item.Definition;
        }
    }
}