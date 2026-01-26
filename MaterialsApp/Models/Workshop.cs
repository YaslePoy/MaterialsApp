using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace MaterialsApp.Models;

public class Workshop
{
    [Key]
    public int Id { get; set; }
    public string ShemaName { get; set; }
    public string ShemaData { get; set; }

    public override string ToString() => ShemaName;
    [NotMapped]
    public Bitmap UrlPathSegmentValue => new Bitmap(AssetLoader.Open(new Uri($"avares://MaterialsApp/Assets/{ShemaName}.png")));
    [NotMapped]
    public List<WorkshopPlacement> Parsed { get => JsonSerializer.Deserialize<List<WorkshopPlacement>>(ShemaData ?? "[]")!; set => ShemaData = JsonSerializer.Serialize(value); }
}

public record WorkshopPlacement
{
    public string ItemName { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
}