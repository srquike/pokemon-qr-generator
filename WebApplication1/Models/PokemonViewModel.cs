using System.Text;

namespace WebApplication1;

public class PokemonViewModel
{
    public int Id { get; set; }
    public int Height { get; set; }
    public int Order { get; set; }
    public string? Name { get; set; }
    public int Weight { get; set; }

    public string GetFormatedInfo()
    {
        var builder = new StringBuilder();

        builder.Append("Pokemon info \n");
        builder.Append($"Id: {Id}\n");
        builder.Append($"Name: {Name}\n");
        builder.Append($"Height: {Height}\n");
        builder.Append($"Order: {Order}\n");
        builder.Append($"Weight: {Weight}\n");

        return builder.ToString();
    }
}
