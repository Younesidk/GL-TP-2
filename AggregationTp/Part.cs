namespace AggregationTp;

public class Part
{
    public int Reference { get; private set; }
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public int StockQuantity { get; private set; }
    public double Price { get; private set; }

    public Part(int reference, string name, int quantity, int stockQuantity, double price)
    {
        Reference = reference;
        Name = name;
        Quantity = quantity;
        StockQuantity = stockQuantity;
        Price = price;
    }

    public void Restock(int quantity)
    {
        Quantity += quantity;
    }

    public double CalculatePartAmount() =>
        Price * Quantity;

    public override string ToString() =>
        $"Part {{ Reference = {Reference}, Name = '{Name}', Quantity = {Quantity}, StockQuantity = {StockQuantity}, Price = {Price} }}";
}
