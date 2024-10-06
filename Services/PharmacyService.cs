using System;
using CosmosPostgresAPI.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CosmosPostgresAPI.Services;

public class PharmacyService
{
    private readonly AppDbContext _context;

    public PharmacyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LoadDataFromCsvAsync(string filepath)
    {
        await using var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
        await connection.OpenAsync();

        var sDestinationSchemaAndTableName = "pharmacy";
        if (File.Exists(filepath))
        {
            await using var writer = connection.BeginTextImport($"COPY {sDestinationSchemaAndTableName} FROM STDIN WITH(FORMAT CSV, HEADER true, NULL '');");
            foreach (var line in File.ReadLines(filepath))
            {
                await writer.WriteLineAsync(line);
            }
        }
        Console.WriteLine("Data loaded successfully.");
    }

}
