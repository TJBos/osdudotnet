namespace OsduLib.Models.Record;

public class DatasetRegistryRecord
{
    public DatasetProperties DatasetProperties;
    public string? ResourceSecurityClassification;
    public string? SchemaFormatTypeID;
    public string? Name;

    public DatasetRegistryRecord()
    {
        DatasetProperties = new DatasetProperties();
    }
}