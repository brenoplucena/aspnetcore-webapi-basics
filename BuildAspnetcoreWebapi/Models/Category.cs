namespace BuildAspnetcoreWebapi.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    //Relation
    public virtual IEnumerable<Product> Products { get; set; }
}
