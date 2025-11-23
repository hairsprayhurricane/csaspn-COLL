using csaspn_COLL.Models;
using System;
using System.Data.SQLite;
using csaspn_COLL.Models;

namespace csaspn_COLL.Services
{
    public class ImageService
    {
        private readonly string _connectionString;

        public ImageService(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand(
                    @"CREATE TABLE IF NOT EXISTS Images (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FileName TEXT NOT NULL,
                        FilePath TEXT NOT NULL,
                        FileSize INTEGER NOT NULL,
                        UploadDate DATETIME NOT NULL
                    )", connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveImage(ImageFile image)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand(
                    @"INSERT INTO Images (FileName, FilePath, FileSize, UploadDate) 
                      VALUES (@FileName, @FilePath, @FileSize, @UploadDate)",
                    connection);

                cmd.Parameters.AddWithValue("@FileName", image.FileName ?? "");
                cmd.Parameters.AddWithValue("@FilePath", image.FilePath ?? "");
                cmd.Parameters.AddWithValue("@FileSize", image.FileSize);
                cmd.Parameters.AddWithValue("@UploadDate", image.UploadDate);

                cmd.ExecuteNonQuery();
            }
        }

        public List<ImageFile> GetAllImages()
        {
            var images = new List<ImageFile>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Images ORDER BY UploadDate DESC", connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        images.Add(new ImageFile
                        {
                            Id = (int)reader["Id"],
                            FileName = reader["FileName"].ToString(),
                            FilePath = reader["FilePath"].ToString(),
                            FileSize = (long)reader["FileSize"],
                            UploadDate = (DateTime)reader["UploadDate"]
                        });
                    }
                }
            }

            return images;
        }
    }
}
