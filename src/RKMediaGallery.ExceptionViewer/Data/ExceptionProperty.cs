namespace RKMediaGallery.ExceptionViewer.Data;

public class ExceptionProperty(string name, string value)
{
    public string Name { get; } = name;
    public string Value { get; } = value;

    public override string ToString()
    {
        return $"{this.Name}: {this.Value}";
    }
}