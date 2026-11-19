using AlertaBairro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore.Infrastructure;

﻿var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var oracleConnectionString = builder.Configuration.GetConnectionString("Oracle")
    ?? Environment.GetEnvironmentVariable("ORACLE_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(oracleConnectionString))
{
    throw new InvalidOperationException(
        "A connection string do Oracle não foi configurada. " +
        "Defina ConnectionStrings__Oracle ou ORACLE_CONNECTION_STRING.");
}

builder.Services.AddDbContext<AlertaBairroDbContext>(options =>
    options.UseOracle(oracleConnectionString, oracle =>
        oracle.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
