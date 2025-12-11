using csaspn_COLL.Services;
using System.Data.SqlClient;
using System.Collections.Generic;

public class ImageService
{
    private readonly string _connectionString;

    public ImageService(string connectionString)
    {
        _connectionString = connectionString;
        EnsureDbAndTable();
    }

    private void EnsureDbAndTable()
    {
        var builder = new SqlConnectionStringBuilder(_connectionString);
        var dbName = builder.InitialCatalog;

        var sysConnString = new SqlConnectionStringBuilder(_connectionString) { InitialCatalog = "master" }.ConnectionString;
        using (var conn = new SqlConnection(sysConnString))
        {
            conn.Open();
            using (var cmd = new SqlCommand($"IF DB_ID('{dbName}') IS NULL CREATE DATABASE [{dbName}]", conn))
                cmd.ExecuteNonQuery();
        }

        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new SqlCommand(
                @"IF OBJECT_ID('dbo.ImageContainer', 'U') IS NULL
                  CREATE TABLE dbo.ImageContainer (
                      Id INT IDENTITY(1,1) PRIMARY KEY,
                      FileContent VARBINARY(MAX) NOT NULL,
                      FileName NVARCHAR(255) NOT NULL
                  )", conn))
            { cmd.ExecuteNonQuery(); }
        }
    }

    public void SaveImage(ImageData img)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new SqlCommand(
                "INSERT INTO dbo.ImageContainer (FileContent, FileName) VALUES (@file, @name)", conn))
            {
                cmd.Parameters.AddWithValue("@file", img.FileContent);
                cmd.Parameters.AddWithValue("@name", img.FileName);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<ImageData> GetAllImages()
    {
        var images = new List<ImageData>();
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new SqlCommand("SELECT Id, FileContent, FileName FROM dbo.ImageContainer ORDER BY Id DESC", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    images.Add(new ImageData
                    {
                        Id = reader.GetInt32(0),
                        FileContent = (byte[])reader["FileContent"],
                        FileName = reader["FileName"].ToString()
                    });
                }
            }
        }
        return images;
    }

    public ImageData GetImageById(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new SqlCommand(
                "SELECT Id, FileContent, FileName FROM dbo.ImageContainer WHERE Id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ImageData
                        {
                            Id = reader.GetInt32(0),
                            FileContent = (byte[])reader["FileContent"],
                            FileName = reader["FileName"].ToString()
                        };
                    }
                }
            }
        }
        return null;
    }
}
