using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public sealed class Database : IDatabase
{
    private static SQLiteConnection? _connection;

    public Database() { }

    public void CreateDatabase()
    {
        try
        {
            string path = "..\\..\\..\\..\\MitzMuzica\\Resources\\playlistsDB.db";
            if (File.Exists(path))
            {
                EstablishConnection(path);
                return;
            }
            SQLiteConnection.CreateFile(path);
            EstablishConnection(path);
            
            _connection.Open();

            string query = @"CREATE TABLE IF NOT EXISTS songs (
                                s_id  INTEGER PRIMARY KEY ASC AUTOINCREMENT,
                                title TEXT    NOT NULL
                                              UNIQUE,
                                path  TEXT    NOT NULL
                                              UNIQUE
                            );

                            CREATE TABLE IF NOT EXISTS playlists (
                                p_id INTEGER PRIMARY KEY ASC AUTOINCREMENT,
                                name TEXT    UNIQUE
                                             NOT NULL
                            );
                            
                            CREATE TABLE IF NOT EXISTS play_queue (
                                p_id INTEGER REFERENCES playlists (p_id) ON DELETE CASCADE
                                                                         ON UPDATE CASCADE,
                                s_id INTEGER REFERENCES songs (s_id) ON DELETE CASCADE
                                                                     ON UPDATE CASCADE
                            );";
            
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

    public void EstablishConnection(string path)
    {
        _connection ??= new SQLiteConnection($"Data Source={path};foreign keys=true");
    }

    public void InsertNewSong(string title, string path)
    {
        // TO DO: Add return id on inserting
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
    
    public string GetSongPath(int s_id)
    {
        string path = "";
        try
        {
            _connection.Open();
            string query = "SELECT * FROM songs WHERE s_id = @s_id";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@s_id", s_id);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
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

        return path;
    }
    
    public string GetSongPath(string title)
    {
        string path = "";
        int s_id = 0;
        try
        {
            _connection.Open();
            string query = "SELECT s_id FROM songs WHERE title = @title";

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

        path = GetSongPath(s_id);
        return path;
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

    public void DeleteSong(int s_id)
    {
        try
        {
            _connection.Open();
            string query = "DELETE FROM songs WHERE s_id = @s_id";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@s_id", s_id);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public void DeleteSong(string title)
    {
        try
        {
            _connection.Open();
            string query = $"DELETE FROM songs WHERE title = @title";
            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@title", title);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public List<int> GetPlaylist(int p_id)
    {
        List<int> results = new List<int>();
        try
        {
            _connection.Open();
            string query = "SELECT s_id FROM play_queue WHERE p_id=@p_id";
    
            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@p_id", p_id);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(reader.GetInt32(reader.GetOrdinal("s_id")));
                    }
                }
            }
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    
        return results;
    }
    
    public List<int> GetPlaylist(string name)
    {
        List<int> results = new List<int>();
        int p_id = 0;
        try
        {
            _connection.Open();
            string query = "SELECT p_id FROM playlists WHERE name = @name";

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
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    
        results = GetPlaylist(p_id);

        return results;
    }

    public int InsertNewPlaylist(string name, List<int> songIds)
    {
        try
        {
            int p_id = 0;
            _connection.Open();
            using (SQLiteTransaction transaction = _connection.BeginTransaction())
            {
                string query = "INSERT INTO playlists(name) VALUES (@name) RETURNING p_id";

                using (SQLiteCommand command = new SQLiteCommand(query, _connection, transaction))
                {
                    command.Parameters.AddWithValue("@name", name);
                    p_id = Convert.ToInt32(command.ExecuteScalar());
                }
            
                foreach (var songId in songIds)
                {
                    query = "INSERT INTO play_queue(p_id, s_id) VALUES (@p_id, @songId)";
                    using (SQLiteCommand command = new SQLiteCommand(query, _connection, transaction))
                    {
                        command.Parameters.AddWithValue("@p_id", p_id);
                        command.Parameters.AddWithValue("@songId", songId);

                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
            _connection.Close();
            return p_id;
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

    public void DeletePlaylist(string name)
    {
        try
        {
            _connection.Open();
            string query = "DELETE FROM playlists WHERE name = (@name)";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
