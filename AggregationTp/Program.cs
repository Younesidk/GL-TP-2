namespace AggregationTp;

/// <summary>Main test class: builds a repair with several parts and exercises every method.</summary>
public class Program
{
    public static void Main()
    {
        // Parts exist independently of any repair (aggregation).
        var filter = new Part(101, "Oil filter", 2, 10, 15.50);
        var pads = new Part(102, "Brake pads", 4, 20, 32.00);
        var belt = new Part(103, "Timing belt", 1, 5, 120.00);

        // ---- Constructors ----
        Title("Constructors");
        var repair = new Repair(new DateTime(2026, 11, 8), 5, "Engine maintenance",
            new List<Part> { filter, pads });
        repair.EquipmentValue = 500;
        Console.WriteLine(repair);
        repair.DisplayPartsList();

        var emptyRepair = new Repair(new DateTime(2026, 11, 9), 2, "Inspection");
        Console.WriteLine(emptyRepair);
        emptyRepair.DisplayPartsList();

        // ---- Part methods ----
        Title("Part.CalculatePartAmount / Part.Restock");
        Console.WriteLine($"Brake pads amount = {pads.CalculatePartAmount():F2} (expected 128.00)");
        pads.Restock(15);
        Console.WriteLine($"Brake pads stock after Restock(15) = {pads.StockQuantity} (expected 35)");

        // ---- CalculateRepairAmount ----
        Title("CalculateRepairAmount");
        Console.WriteLine($"Total = {repair.CalculateRepairAmount():F2} (expected 2*15.50 + 4*32.00 = 159.00)");

        // ---- AddPart(ref, name, qty, price) ----
        Title("AddPart(ref, name, qty, price)");
        repair.AddPart(104, "Spark plug", 4, 8.75);   // new part
        repair.AddPart(101, "Oil filter", 1, 15.50);  // existing -> quantity 2 + 1 = 3
        repair.DisplayPartsList();

        // ---- AddPart(Part) ----
        Title("AddPart(Part)");
        repair.AddPart(belt);                                  // new part
        repair.AddPart(new Part(102, "Brake pads", 2, 32.00)); // existing -> quantity 4 + 2 = 6
        repair.DisplayPartsList();

        // ---- ModifyPart(ref, name, qty, price) ----
        Title("ModifyPart(ref, name, qty, price)");
        repair.ModifyPart(104, "Iridium spark plug", 4, 12.00);
        repair.ModifyPart(999, "Ghost part", 1, 1.00); // unknown ref -> nothing happens
        repair.DisplayPartsList();

        // ---- ModifyPart(Part) ----
        Title("ModifyPart(Part)");
        repair.ModifyPart(new Part(103, "Timing belt kit", 1, 5, 180.00));
        repair.DisplayPartsList();

        // ---- RemovePart ----
        Title("RemovePart(101)");
        repair.RemovePart(101);
        repair.RemovePart(999); // unknown ref -> nothing happens
        repair.DisplayPartsList();
        Console.WriteLine($"Oil filter object still exists outside the repair: {filter}");

        // ---- IsAvailable ----
        Title("IsAvailable");
        Console.WriteLine($"IsAvailable(\"Brake pads\") = {repair.IsAvailable("Brake pads")} (expected True)");
        Console.WriteLine($"IsAvailable(\"Oil filter\") = {repair.IsAvailable("Oil filter")} (expected False)");

        // ---- IsScrappable ----
        Title("IsScrappable");
        Console.WriteLine($"Total = {repair.CalculateRepairAmount():F2}, equipment value = {repair.EquipmentValue:F2}");
        Console.WriteLine($"IsScrappable() = {repair.IsScrappable()}");
        repair.EquipmentValue = 300;
        Console.WriteLine($"Equipment value lowered to {repair.EquipmentValue:F2} -> IsScrappable() = {repair.IsScrappable()}");

        // ---- Final state ----
        Title("Final repair");
        Console.WriteLine(repair);
        repair.DisplayPartsList();
        Console.WriteLine($"Total = {repair.CalculateRepairAmount():F2}");
    }

    private static void Title(string text) => Console.WriteLine($"\n===== {text} =====");
}
