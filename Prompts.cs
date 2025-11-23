namespace NatureAPI;

public static class Prompts
{
    public static string GeneratePlacesPrompt(string jsonData)
{
    return $@"
Eres un analista experto en turismo, visualización de datos, comportamiento del visitante
y diseño de dashboards para aplicaciones móviles y web.

Analiza el siguiente arreglo JSON de lugares:
{jsonData}

Cada objeto contiene:
- Id
- Name
- Category
- Latitude
- Longitude
- Accessible
- EntryFee
- AverageRating
- ReviewCount
- TrailCount
- PhotosCount

Devuelve EXCLUSIVAMENTE un JSON válido con esta estructura EXACTA:

{{
  ""summary"": {{
    ""totalPlaces"": int,
    ""totalReviews"": int,
    ""averageRatingGlobal"": double,
    ""averageTrailsPerPlace"": double,
    ""averagePhotosPerPlace"": double
  }},
  ""geoStats"": {{
    ""minLat"": double,
    ""maxLat"": double,
    ""minLng"": double,
    ""maxLng"": double,
    ""centerLat"": double,
    ""centerLng"": double
  }},
  ""ratingHistogram"": {{
    ""1"": int, ""2"": int, ""3"": int, ""4"": int, ""5"": int
  }},
  ""correlations"": {{
    ""rating_vs_photos"": double,
    ""rating_vs_trails"": double
  }},
  ""rankings"": {{
    ""topRatedOverall"": [
      {{ ""id"": int, ""name"": string, ""averageRating"": double, ""reviewCount"": int }}
    ],
    ""bestForHiking"": [
      {{ ""id"": int, ""name"": string, ""trailCount"": int, ""averageRating"": double }}
    ],
    ""mostPopular"": [
      {{ ""id"": int, ""name"": string, ""reviewCount"": int, ""averageRating"": double }}
    ]
  }},
  ""featured"": {{
    ""hiddenGems"": [
      {{ ""id"": int, ""name"": string, ""averageRating"": double, ""reviewCount"": int }}
    ],
    ""topFreePlaces"": [
      {{ ""id"": int, ""name"": string, ""averageRating"": double, ""entryFee"": double }}
    ],
    ""accessibleHighlights"": [
      {{ ""id"": int, ""name"": string, ""averageRating"": double, ""accessible"": bool }}
    ]
  }},
  ""categoryStats"": [
    {{
      ""category"": string,
      ""placeCount"": int,
      ""averageRating"": double,
      ""averageReviewsPerPlace"": double
    }}
  ],
  ""accessibilityStats"": {{
    ""accessiblePlaces"": int,
    ""nonAccessiblePlaces"": int
  }},
  ""patterns"": [string]
}}

Instrucciones:
- ratingHistogram debe basarse en ReviewCount y AverageRating redondeado.
- correlations debe calcular correlaciones (pearson aproximado) entre:
    rating–photos y rating–trailcount.
- geoStats debe calcular rangos y centro aproximado.
- patterns debe incluir insights útiles para un dashboard moderno.
- NO incluyas ningún texto fuera del JSON.
- Usa punto decimal.
";
}

    
    
    public static string GenerateTrailsPrompt(string jsonData)
{
    return $@"
Eres un analista experto en senderismo, seguridad en montaña y diseño de experiencias para apps de rutas naturales.

Analiza el siguiente **arreglo JSON de senderos (trails)**. Cada objeto contiene:
- Id: int
- Name: string
- DistanceKm: double
- EstimatedTimeMinutes: int
- Difficulty: string
- IsLoop: bool

Datos de entrada (JSON):
{jsonData}

Debes responder **EXCLUSIVAMENTE** con un **JSON válido** con esta estructura EXACTA:

{{
  ""summary"": {{
    ""totalTrails"": int,
    ""averageDistanceKm"": double,
    ""averageEstimatedTimeMinutes"": double,
    ""loopTrailCount"": int,
    ""nonLoopTrailCount"": int
  }},
  ""difficultyStats"": [
    {{
      ""difficulty"": string,
      ""trailCount"": int,
      ""averageDistanceKm"": double,
      ""averageEstimatedTimeMinutes"": double
    }}
  ],
  ""topTrailsByDistance"": [
    {{
      ""id"": int,
      ""name"": string,
      ""distanceKm"": double,
      ""difficulty"": string
    }}
  ],
  ""recommendedForBeginners"": [
    {{
      ""id"": int,
      ""name"": string,
      ""difficulty"": string,
      ""distanceKm"": double,
      ""estimatedTimeMinutes"": int
    }}
  ],
  ""recommendedForExperiencedHikers"": [
    {{
      ""id"": int,
      ""name"": string,
      ""difficulty"": string,
      ""distanceKm"": double,
      ""estimatedTimeMinutes"": int
    }}
  ],
  ""patterns"": [string]
}}

Criterios:

- ""summary"":
  * totalTrails = número total de senderos
  * averageDistanceKm = promedio de DistanceKm
  * averageEstimatedTimeMinutes = promedio de EstimatedTimeMinutes
  * loopTrailCount = IsLoop == true
  * nonLoopTrailCount = IsLoop == false

- ""difficultyStats"":
  * Agrupa por Difficulty y calcula count, distancia promedio y tiempo promedio

- ""topTrailsByDistance"":
  * Top 3 senderos más largos

- ""recommendedForBeginners"":
  * Dificultad Baja
  * Distancia <= 5 km (si es posible)

- ""recommendedForExperiencedHikers"":
  * Dificultad Media/Alta
  * Distancia alta

- ""patterns"":
  * Observaciones generales útiles (tendencias, agrupaciones, dificultades comunes)

Reglas:
- Devuelve **solo** el JSON, sin texto adicional.
- Usa **punto decimal**.
- Si falta información para una sección, devuelve solo lo disponible.
- Si ocurre un error, responde únicamente: error
";
}

    
    
    
}
