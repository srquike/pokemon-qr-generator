using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("Pokemon");
        var response = await client.GetAsync("?limit=20&offset=0");
        var json = await response.Content.ReadAsStringAsync();
        var results = JsonNode.Parse(json);
        var data = results["results"].ToJsonString();
        var pokemons = JsonSerializer.Deserialize<IReadOnlyList<PokemonViewModel>>(data, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return View(pokemons);
    }

    [HttpPost("qr")]
    public ActionResult<string> GetQRCode([FromBody] PokemonViewModel pokemon)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(pokemon.GetFormatedInfo(), QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCode(qrCodeData);

        var qrCodeImage = qrCode.GetGraphic(20);

        using var stream = new MemoryStream();

        qrCodeImage.Save(stream, ImageFormat.Png);

        var qrCodeBase64String = Convert.ToBase64String(stream.ToArray());


        return Ok(string.Concat("data:image/png;base64,", qrCodeBase64String));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
