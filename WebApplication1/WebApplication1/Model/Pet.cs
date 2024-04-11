namespace WebApplication1.Model;

public class Pet
{
    public int id { get; set; }
    public string name { get; set; }
    public Category category { get; set; }
    public double weight { get; set; }
    public string color { get; set; }
    public List<Visit> Visits { get; set; }
}