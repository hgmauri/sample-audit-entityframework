namespace Sample.Audit.Persistence.Entities;

public class Company : Base
{
    public string Name { get; set; }
    public List<User> Users { get; set; }
}