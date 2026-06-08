using nobodyworks.builder.building;
using nobodyworks.builder.items;

namespace nobodyworks.builder
{
    public static class Databases
    {
        public static ItemsDatabase Items { get; private set; }
        public static ElementsDatabase Elements { get; private set; }

        public static void Setup(ItemsDatabase itemsDatabase, ElementsDatabase elementsDatabase)
        {
            Items = itemsDatabase;
            Elements = elementsDatabase;
        }
    }
}