# Pokedex

Pokedex is a web service that will get you basic pokemon information with an optional funny translation.

## Prerequisites

You will need [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) to build the project and [.NET 5 Runtime](https://dotnet.microsoft.com/download/dotnet/5.0) to run the project.

## Installation

Clone the repository and run

```bash
dotnet run --project Pokedex\Pokedex.WebAPI\Pokedex.WebAPI.csproj
```

You can also use Docker

```bash
docker build -t pokedex -f Pokedex\Pokedex.WebAPI\Dockerfile Pokedex
docker run -it -p 5000:80 pokedex
```

## Configuration
You can configure underlying requests to external APIs using HttpClientConfig section in _appsettings.json_

```json
"HttpClientConfig": {
    "RetryPolicy": {
      "FirstRetryDelayMs": 100,
      "NumberOfRetries": 3
    },
    "CircuitBreakerPolicy": {
      "HandledEventsBeforeBreaking": 2,
      "DurationOfBreakMs": 300000
    },
    "ClientConfigs": {
      "YodaStringTranslator": {
        "BaseUrl": "https://api.funtranslations.com/translate/yoda.json?text=",
        "Policies": [ "Retry", "CircuitBreaker" ]
      },
      "ShakespeareStringTranslator": {
        "BaseUrl": "https://api.funtranslations.com/translate/shakespeare.json?text=",
        "Policies": [ "Retry", "CircuitBreaker" ]
      },
      "PokeAPIPokemonStore": {
        "BaseUrl": "https://pokeapi.co/api/v2/pokemon-species/",
        "Policies": [ "Retry" ]
      }
    }
  }
```

## Usage

Web service has two endpoints that will return original and translated information respectively:

* /pokemon/_{name}_
* /pokemon/translated/_{name}_

For example, to get the information about [Mewtwo](https://www.pokemon.com/ru/pokedex/mewtwo) you can use the following commands

```bash
curl -GET http://localhost:5000/pokemon/mewtwo
{"name":"mewtwo","description":"It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.","habitat":"rare","isLegendary":true}

curl -GET http://localhost:5000/pokemon/translated/mewtwo
{"name":"mewtwo","description":"Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.","habitat":"rare","isLegendary":true}
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

## License

[The Unlicense](https://choosealicense.com/licenses/unlicense/)