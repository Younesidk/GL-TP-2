namespace AggregationTp;

/// <summary>
/// A replacement part. It lives independently of any Repair (aggregation):
/// it keeps its own inventory balance (StockQuantity) whether or not a repair uses it.
/// </summary>
public class Part
{
    private int _ref;
    private string _name;
    private int _quantity;
    private int _stockQuantity;
    private double _price;

    public Part(int reference, string name, int quantity, double price)
        : this(reference, name, quantity, 0, price)
    {
    }

    public Part(int reference, string name, int quantity, int stockQuantity, double price)
    {
        _ref = reference;
        _name = name;
        _quantity = quantity;
        _stockQuantity = stockQuantity;
        _price = price;
    }

    // Getters / setters
    public int Ref
    {
        get => _ref;
        set => _ref = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public int Quantity
    {
        get => _quantity;
        set => _quantity = value;
    }

    public int StockQuantity
    {
        get => _stockQuantity;
        set => _stockQuantity = value;
    }

    public double Price
    {
        get => _price;
        set => _price = value;
    }

    /// <summary>Adds the given quantity to the part's available inventory.</summary>
    public void Restock(int quantity)
    {
        _stockQuantity += quantity;
    }

    /// <summary>partAmount = quantity × price</summary>
    public double CalculatePartAmount() => _quantity * _price;

    public override string ToString() =>
        $"Part {{ Ref = {_ref}, Name = '{_name}', Quantity = {_quantity}, StockQuantity = {_stockQuantity}, Price = {_price:F2}, Amount = {CalculatePartAmount():F2} }}";
}
