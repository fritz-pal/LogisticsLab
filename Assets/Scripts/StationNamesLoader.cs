using System;
using System.Collections.Generic;

public class StationNamesLoader
{
    
    private static List<string> strings = new(new[]
    {
        "Frankfurt",
        "Heilbronn",
        "Hochschule Heilbronn",
        "Stuttgart",
        "Karlsruhe",
        "Mannheim",
        "Heidelberg",
        "München",
        "Hamburg",
        "Hamburg Altona",
        "Berlin HBF",
        "Köln Messe",
        "Köln HBF",
        "Köln Südbahnhof",
        "Düsseldorf",
        "Dortmund",
        "Essen",
        "Duisburg",
        "Bochum",
        "Bonn",
        "Monopoly Bahnhof Süd",
        "Paris",
        "London",
        "Amsterdam",
        "Wien",
        "Zürich",
        "Rom",
        "Mailand",
        "Madrid",
        "Barcelona",
        "Lissabon",
        "Lyon",
        "Marseille",
        "Nizza",
        "Brüssel",
        "Prag",
        "Budapest",
        "Warschau",
        "Kopenhagen",
        "Oslo",
        "Stockholm",
        "Helsinki",
        "Istanbul",
        "Dubai",
        "Kairo",
        "Kapstadt",
        "Nairobi",
        "Mumbai",
        "Delhi",
        "Peking",
        "Shanghai",
        "Tokio",
        "Seoul",
        "Sydney",
        "Auckland",
        "Los Angeles",
        "San Francisco",
        "New York",
        "Toronto",
        "Mexico City",
        "Rio de Janeiro",
        "Buenos Aires",
        "Santiago",
        "Lima",
        "Bogotá",
        "Caracas",
        "São Paulo",
        "Lagos"
    });

    public static string GetRandomName()
    {
        Random random = new();
        return strings[random.Next(strings.Count)];
    }
}
