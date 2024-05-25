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
                        title = reader.GetString(reader.GetOrdinal("title"));
                        path = reader.GetString(reader.GetOrdinal("path"));
                    }
                }
            }
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }

        return (songId, title, path);
    }
    
    public int GetSongID(string title)
    {
        int s_id = 0;
        try
        {
            _connection.Open();
            string query = "SELECT * FROM songs WHERE title = @title";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@title", title);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        s_id = reader.GetInt32(reader.GetOrdinal("s_id"));
                    }
                }
            }
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }

        return s_id;
    }

    public void DeleteSong(int songId)
    {
        try
        {
            _connection.Open();
            string query = $"DELETE FROM songs WHERE s_id = {songId}";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
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
