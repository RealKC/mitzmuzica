using System.Data.SQLite;
using MitzMuzica.PlaylistAPI;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public sealed class Database : IDatabase
{
    private static SQLiteConnection? _connection;
    
    public Database(){ }

    public void EstablishConnection(string path)
    {
        _connection ??= new SQLiteConnection($"Data Source={path};Version=3;");
    }

    public void InsertNewSong(string title, string path)
    {
        try
        {
            _connection.Open();
            string query = "INSERT INTO songs(title, path) VALUES (@title, @path)";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@path", path);
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            if (ex.Message.Contains("UNIQUE constraint failed"))
            {
                throw new Exception($"\nValori duplicate!\nTitle: \"{title}\", Path: \"{path}\" exista deja in databse");
            }
            else
            {
                throw new Exception(ex.Message);
            }
        }
    }
    
    public (int, string, string) GetSong(int songId)
    {
        int id = 0;
        string title = "";
        string path = "";

        try
        {
            _connection.Open();
            string query = $"SELECT * FROM songs WHERE s_id = {songId}";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("s_id"));
                        title = reader.GetString(reader.GetOrdinal("title"));
                        path = reader.GetString(reader.GetOrdinal("path"));
                        
                        Console.WriteLine($"ID: {id}, Title: {title}, Path: {path}");

                    }
                }
            }
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }

        return (id, title, path);
    }

    public void DeleteSong(string songId)
    {
        throw new NotImplementedException();
    }

    public IPlaylist GetPlaylist(string playlistId)
    {
        throw new NotImplementedException();
    }

    public void InsertNewPlaylist(IPlaylist playlist)
    {
        throw new NotImplementedException();
    }

    public void DeletePlaylist(string playlist)
    {
        throw new NotImplementedException();
    }
}
