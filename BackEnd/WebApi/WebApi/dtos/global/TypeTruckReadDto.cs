namespace WebApplication1.DTOS.Read;

/// <summary>
/// The TypesOfTrucksDto stores information about the number of trucks / transports by category
/// </summary>
public class TypesOfTrucksDto
{
    public int refrigerator { get; set; }

    public int just_tractor { get; set; }

    public int dump_truck { get; set; }

    public int concrete_truck { get; set; }
}