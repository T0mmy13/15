namespace EFCORE15.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;
}
