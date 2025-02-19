using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AutocompleteHandler
{
    private static Trie trie = new Trie();

    public static void mapRoutes(WebApplication app)
    {
        app.MapPost("/insert", async (HttpContext context) =>
        {
            var data = await context.Request.ReadFromJsonAsync<Dictionary<string, string>>();
            if (data != null && data.ContainsKey("word"))
            {
                if (trie.Search(data["word"]))
                {
                    await context.Response.WriteAsync("Word already exists");
                }
                else
                {
                    trie.Insert(data["word"]);
                    await context.Response.WriteAsync("Word inserted");
                }
            }
        });
        app.MapGet("/search", async (HttpContext context) =>
        {
            string? query = context.Request.Query["q"];
            if (!string.IsNullOrEmpty(query))
            {
                var results = trie.ListAllWordsWithPrefix(query);
                await context.Response.WriteAsJsonAsync<List<string>>(results);
            }
            else
            {
                await context.Response.WriteAsJsonAsync<List<string>>([]);
            }
        });
        app.MapPost("/delete", async(HttpContext context) =>
        {
            var data = await context.Request.ReadFromJsonAsync<Dictionary<string, string>>();
            if (data != null && data.ContainsKey("word"))
            {
                if (trie.Search(data["word"]))
                {
                    trie.Delete(data["word"]);
                    await context.Response.WriteAsync("Word deleted");
                }
                else
                {
                    await context.Response.WriteAsync("Word not found");
                }
            }
        });
    }
}