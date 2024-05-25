using System.Data.SQLite;
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
                throw new Exception($"\nValori duplicate!\nTitle: \"{title}\", Path: \"{path}\" exista deja in database");
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

    public int GetPlaylist(string playlistId)
    {
        throw new NotImplementedException();
    }

    public void InsertNewPlaylist(string name, int[] songIds)
    {
        try
        {
            int p_id = 0;
            _connection.Open();
            string query = "INSERT INTO playlists(name) VALUES (@name)";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            }
            
            query = "SELECT p_id FROM playlists WHERE name=@name";
            
            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@name", name);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p_id = reader.GetInt32(reader.GetOrdinal("p_id"));
                    }
                }
            }
            
            foreach (var songId in songIds)
            {
                query = "INSERT INTO play_queue(p_id, s_id) VALUES (@p_id, @songId)";
                using (SQLiteCommand command = new SQLiteCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@p_id", p_id);
                    command.Parameters.AddWithValue("@songId", songId);

                    command.ExecuteNonQuery();
                }
            }
            
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            if (ex.Message.Contains("UNIQUE constraint failed"))
            {
                throw new Exception($"\nValori duplicate!\nNume: \"{name}\" exista deja in database");
            }
            else
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public void DeletePlaylist(int p_id)
    {
        throw new NotImplementedException();
    }
}
