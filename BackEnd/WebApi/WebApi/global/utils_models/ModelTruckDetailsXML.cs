using WebApplication1.Global.Enumerations;

namespace WebApplication1.Global.Utils;

/// <summary>
/// This class stores the XML information of all trucks
/// </summary>
public class ModelTruckDetailsXML
{
    /// <summary>
    /// contains the information of all trucks
    /// </summary>
    public List<TruckDetailsXml> truckDetails { get; set; }
}

/// <summary>
/// This class stores the information of a truck that is present in the XML file
/// </summary>
public class TruckDetailsXml
{
    /// <summary>
    /// contains the capacity of the truck
    /// </summary>
    public int capacity { get; set; }

    /// <summary>
    /// contains the power of the truck
    /// </summary>
    public int power { get; set; }

    /// <summary>
    /// contains the registration of the truck
    /// </summary>
    public string matricula { get; set; }

    /// <summary>
    /// contains the year of the truck
    /// </summary>
    public int year { get; set; }

    /// <summary>
    /// contains the fuel consumption of the truck
    /// </summary>
    public int fuelConsumption { get; set; }

    /// <summary>
    /// contains the kms of the truck
    /// </summary>
    public int kms { get; set; }

    /// <summary>
    /// contains the kms  when the next review should be done
    /// </summary>
    public int nextRevision { get; set; }

    /// <summary>
    /// contains the category of the truck
    /// </summary>
    public TruckCategory truckCategory { get; set; }
}