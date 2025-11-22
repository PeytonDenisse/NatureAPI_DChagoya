namespace NatureAPI;

public static class Prompts
{
    public static string GeneratePlacesPrompt(string jsonData)
    {
        return $@"
Eres un analista experto en turismo, geografía, comportamiento del visitante,
y diseño de experiencias en apps móviles de naturaleza.

Analiza el siguiente **arreglo JSON de lugares**. Cada objeto contiene:
- Id: int
- Name: string
- Category: string
- Latitude: double
- Longitude: double
- Accessible: bool
- EntryFee: double
- AverageRating: double
- ReviewCount: int
- TrailCount: int
- PhotosCount: int

Datos de entrada:
{jsonData}

Debes responder **EXCLUSIVAMENTE** con un **JSON válido** con esta estructura EXACTA:

{{
  ""summary"": {{
    ""totalPlaces"": int,
    ""totalReviews"": int,
    ""averageRatingGlobal"": double,
    ""averageTrailsPerPlace"": double,
    ""averagePhotosPerPlace"": double
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

Criterios:

1. **rankings.topRatedOverall**
   - Top 3 lugares con mejor rating
   - Empates: mayor cantidad de reseñas

2. **rankings.bestForHiking**
   - Top 3 con más senderos (TrailCount)
   - Empates: mayor rating

3. **rankings.mostPopular**
   - Top 3 con más reseñas

4. **featured.hiddenGems**
   - Lugares con pocas reseñas pero rating alto (> 4.5 si existe)
   - Máx. 3 resultados

5. **featured.topFreePlaces**
   - Lugares con EntryFee == 0 ordenados por rating

6. **featured.accessibleHighlights**
   - Top 3 accesibles (Accessible == true) con mayor rating

7. **patterns**
   Genera insights útiles como:
   - tendencias
   - categorías populares
   - lugares subestimados
   - patrones de accesibilidad
   - relación entre senderos, fotos y rating

Reglas:
- Devuelve **solo** el JSON sin nada más.
- Usa separador decimal con punto.
- Si hay pocos datos en alguna sección, devuelve solo los disponibles.
- Si ocurre un error, responde únicamente:
error
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
