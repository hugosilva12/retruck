namespace WebApplication1.Global;

/// <summary>
/// This class stores information about response of ModelResponseDistanceMatrix
/// </summary>
public class ModelResponseDistanceMatrix
{
    public IList<string> destination_addresses { get; set; }
    public IList<string> origin_addresses { get; set; }
    public IList<Row> rows { get; set; }
    public string status { get; set; }
}

/// <summary>
/// This class stores information about all rows
/// </summary>
public class Row
{
    public IList<Element> elements { get; set; }
}

/// <summary>
/// This class stores information about distance, duration and status between two locations
/// </summary>
public class Element
{
    public Distance distance { get; set; }
    public Duration duration { get; set; }
    public string status { get; set; }
}

/// <summary>
/// This class stores information about distance between two locations
/// </summary>
public class Distance
{
    public string text { get; set; }
    public int value { get; set; }
}

/// <summary>
/// This class stores information about duration between two locations
/// </summary>
public class Duration
{
    public string text { get; set; }
    public int value { get; set; }
}