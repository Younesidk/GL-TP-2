namespace AggregationTp;

/// <summary>
/// Maintenance work performed on a piece of equipment.
/// Aggregates a collection of Part objects (Repair ◇—— * Part).
/// All traversals of the parts collection go through an iterator
/// (IEnumerator: MoveNext() ≈ hasNext()+next(), Current ≈ the returned element).
/// </summary>
public class Repair
{
    private DateTime _repairDate;
    private int _hoursWorked;
    private string _workDescription;
    // Original acquisition value of the equipment, used as the write-off threshold.
    private double _equipmentValue;

    // C# equivalent of Java's Vector<Part> repairPartsList
    private List<Part> _repairPartsList;

    public Repair(DateTime repairDate, int hoursWorked, string workDescription)
    {
        _repairDate = repairDate;
        _hoursWorked = hoursWorked;
        _workDescription = workDescription;
        _repairPartsList = [];
    }

    public Repair(DateTime repairDate, int hoursWorked, string workDescription, List<Part> repairPartsList)
        : this(repairDate, hoursWorked, workDescription)
    {
        _repairPartsList = repairPartsList ?? [];
    }

    // Getters / setters
    public DateTime RepairDate
    {
        get => _repairDate;
        set => _repairDate = value;
    }

    public int HoursWorked
    {
        get => _hoursWorked;
        set => _hoursWorked = value;
    }

    public string WorkDescription
    {
        get => _workDescription;
        set => _workDescription = value;
    }

    public double EquipmentValue
    {
        get => _equipmentValue;
        set => _equipmentValue = value;
    }

    public List<Part> RepairPartsList
    {
        get => _repairPartsList;
        set => _repairPartsList = value ?? [];
    }

    /// <summary>Returns the part with the given reference, or null (traversal via iterator).</summary>
    private Part? FindPart(int reference)
    {
        using IEnumerator<Part> partIterator = _repairPartsList.GetEnumerator();
        while (partIterator.MoveNext())
        {
            Part p = partIterator.Current;
            if (p.Ref == reference)
                return p;
        }
        return null;
    }

    /// <summary>1. Sum of (price × quantity) of every replacement part.</summary>
    public double CalculateRepairAmount()
    {
        double total = 0;
        using IEnumerator<Part> partIterator = _repairPartsList.GetEnumerator();
        while (partIterator.MoveNext())
        {
            Part p = partIterator.Current;
            total += p.CalculatePartAmount();
        }
        return total;
    }

    /// <summary>2. Adds a new part, or only updates the quantity if the reference already exists.</summary>
    public void AddPart(int reference, string name, int quantity, double price)
    {
        Part? existing = FindPart(reference);
        if (existing is not null)
            existing.Quantity += quantity;
        else
            _repairPartsList.Add(new Part(reference, name, quantity, price));
    }

    /// <summary>3. Adds the part object, or only updates the quantity if it is already present.</summary>
    public void AddPart(Part p)
    {
        if (p is null)
            return;

        Part? existing = FindPart(p.Ref);
        if (existing is not null)
            existing.Quantity += p.Quantity;
        else
            _repairPartsList.Add(p);
    }

    /// <summary>4. Updates the attributes of the part identified by its reference.</summary>
    public void ModifyPart(int reference, string name, int quantity, double price)
    {
        Part? existing = FindPart(reference);
        if (existing is null)
            return;

        existing.Name = name;
        existing.Quantity = quantity;
        existing.Price = price;
    }

    /// <summary>5. Updates the matching part's attributes if a part with the same reference exists.</summary>
    public void ModifyPart(Part p)
    {
        if (p is null)
            return;

        Part? existing = FindPart(p.Ref);
        if (existing is null)
            return;

        existing.Name = p.Name;
        existing.Quantity = p.Quantity;
        existing.StockQuantity = p.StockQuantity;
        existing.Price = p.Price;
    }

    /// <summary>
    /// 6. Removes the part matching the reference.
    /// C#'s IEnumerator has no remove(), and modifying the list while iterating throws
    /// InvalidOperationException (the C# counterpart of ConcurrentModificationException),
    /// so the part is located with the iterator first and removed once traversal is over.
    /// </summary>
    public void RemovePart(int reference)
    {
        Part? toRemove = FindPart(reference);
        if (toRemove is not null)
            _repairPartsList.Remove(toRemove);
    }

    /// <summary>7. True if a part with this name is in the parts list.</summary>
    public bool IsAvailable(string name)
    {
        using IEnumerator<Part> partIterator = _repairPartsList.GetEnumerator();
        while (partIterator.MoveNext())
        {
            Part p = partIterator.Current;
            if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    /// <summary>8. True if the total repair cost exceeds the equipment's acquisition value.</summary>
    public bool IsScrappable() => CalculateRepairAmount() > _equipmentValue;

    /// <summary>9. Prints every replacement part used in this repair.</summary>
    public void DisplayPartsList()
    {
        if (_repairPartsList.Count == 0)
        {
            Console.WriteLine("  (no parts)");
            return;
        }

        using IEnumerator<Part> partIterator = _repairPartsList.GetEnumerator();
        while (partIterator.MoveNext())
        {
            Part p = partIterator.Current;
            Console.WriteLine($"  {p}");
        }
    }

    public override string ToString() =>
        $"Repair {{ Date = {_repairDate:yyyy-MM-dd}, HoursWorked = {_hoursWorked}, Description = '{_workDescription}', EquipmentValue = {_equipmentValue:F2}, Parts = {_repairPartsList.Count} }}";
}
