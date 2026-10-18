namespace AggregationTp;

public class Repair
{
    private DateTime RepairDate { get; set; }
    private int HoursWorked { get; set; }
    private string WorkDescription { get; set; }
    public double WriteOffThreshold { get; set; }

    private List<Part> _repairParts = [];

    public Repair(DateTime repairDate, int hoursWorked, string workDescription, double writeOffThreshold = 0)
    {
        RepairDate = repairDate;
        HoursWorked = hoursWorked;
        WorkDescription = workDescription;
        WriteOffThreshold = writeOffThreshold;
    }

    public Repair(List<Part> parts, DateTime repairDate, int hoursWorked, string workDescription, double writeOffThreshold = 0)
    {
        _repairParts = parts ?? [];
        RepairDate = repairDate;
        HoursWorked = hoursWorked;
        WorkDescription = workDescription;
        WriteOffThreshold = writeOffThreshold;
    }

    public double CalculateRepairAmount(double hourlyRate = 0)
    {
        return _repairParts.Sum(p => p.CalculatePartAmount()) + (HoursWorked * hourlyRate);
    }

    public void AddPart(int reference, string name, int quantity, double price)
    {
        var newPart = new Part(reference, name, quantity, 0, price);
        _repairParts.Add(newPart);
    }

    public void AddPart(Part? part)
    {
        if (part is not null)
        {
            _repairParts.Add(part);
        }
    }

    public void UpdatePart(int reference, string name, int quantity, double price)
    {
        var index = _repairParts.FindIndex(p => p.Reference == reference);
        if (index == -1)
            return;

        _repairParts[index] = new Part(reference, name, quantity, _repairParts[index].StockQuantity, price);
    }

    public void UpdatePart(Part? part)
    {
        if (part is null)
            return;

        var index = _repairParts.FindIndex(p => p.Reference == part.Reference);
        if (index is not -1)
        {
            _repairParts[index] = part;
        }
    }

    public void DeletePart(Part part)
    {
        _repairParts.Remove(part);
    }

    public bool IsAvailable(string name)
    {
        return _repairParts.Exists(p => p.Name == name);
    }

    public bool IsScrappable()
    {
        return CalculateRepairAmount() > WriteOffThreshold;
    }

    public bool IsScrappable(double writeOffThreshold)
    {
        return CalculateRepairAmount() > writeOffThreshold;
    }

    public void DisplayParts()
    {
        foreach (var part in _repairParts)
        {
            Console.WriteLine(part);
        }
    }
}
