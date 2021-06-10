using System.Collections.Generic;

namespace LabirunModel.Labirun
{
    public partial class Inventory
    {
        //public long Id { get; set; }
        public List<InventoryItem> AllItems { get; set; }
        //FactoryGood: InventoryItem[];
        //Meals:InventoryItem[];
        //Boosts:InventoryItem[];
        //Chests:InventoryItem[];
        //SpeedUp:InventoryItem[];

        public Inventory()
        {
            AllItems = new List<InventoryItem>();
        }
    }
}