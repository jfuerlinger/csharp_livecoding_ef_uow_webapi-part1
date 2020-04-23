using ConsoleTableExt;
using MovieManager.Core.DataTransferObjects;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieManager.ConsoleClient
{
  class Program
  {
    private const string ServerNameWithPort = "localhost:57745";

    static void Main(string[] args)
    {
      //RetrieveCategories();
      RetrieveMoviesForCategoryId(3);
    }

    private static void RetrieveMoviesForCategoryId(int id)
    {
      var client = new RestClient($"http://{ServerNameWithPort}");
      var request = new RestRequest($"api/categories/{id}/movies", DataFormat.Json);

      var response = client.Get(request);

      JArray moviesAsJson = JArray.Parse(response.Content);

      List<MovieDto> movies =
              moviesAsJson
                  .Select(m => m.ToObject<MovieDto>())
                  .OrderBy(m => m.Title)
                  .ToList();

      ConsoleTableBuilder
          .From(movies)
          .ExportAndWriteLine();

    }

    public static void RetrieveCategories()
    {
      var client = new RestClient($"http://{ServerNameWithPort}");
      var request = new RestRequest("api/categories", DataFormat.Json);

      var response = client.Get(request);


      JArray categories = JArray.Parse(response.Content);

      foreach (var category in categories)
      {
        Console.WriteLine(category);
      }

      Console.ReadKey();
    }
  }
}
