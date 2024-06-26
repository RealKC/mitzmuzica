/**************************************************************************
 *                                                                        *
 *  Description: Implementation of the IDatabase Interface                *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Petrisor Eduard-Gabriel                        *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using System.Data;
using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public sealed class Database : IDatabase
{
    private static SQLiteConnection? _connection;

    public Database() { }

    public void CreateDatabase(string databasePath = @"..\..\..\..\MitzMuzica\Resources\playlistsDB.db")
    {
        try
        {
            if (File.Exists(databasePath))
            {
                EstablishConnection(databasePath);
                return;
            }
            SQLiteConnection.CreateFile(databasePath);
            EstablishConnection(databasePath);
            
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
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }

    private void EstablishConnection(string path)
    {
        _connection ??= new SQLiteConnection($"Data Source={path};foreign keys=true");
    }

    public int InsertNewSong(string title, string path)
    {
        int songId;
        try
        {
            _connection.Open();
            
            string query = "INSERT INTO songs(title, path) VALUES (@title, @path) RETURNING s_id";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@path", path);
                songId = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        catch (SQLiteException ex)
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
            
            if (ex.Message.Contains("UNIQUE constraint failed"))
            {
                songId =  GetSongId(title);
            }
            else
            {
                throw new Exception(ex.Message);
            }
        }    
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        return songId;
    }
    
    public string GetSongPath(int songId)
    {
        string path = "";
        try
        {
            _connection.Open();
            string query = "SELECT * FROM songs WHERE s_id = @s_id";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@s_id", songId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        path = reader.GetString(reader.GetOrdinal("path"));
                    }
                }
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        if (path == "")
            throw new Exception("Provided identifier does not exist!");
        
        return path;
    }
    
    public string GetSongPath(string title)
    {
        string path = "";
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
                        path = reader.GetString(reader.GetOrdinal("path"));
                    }
                }
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        if (path == "")
            throw new Exception("Provided title does not exist!");
        
        return path;
    }
    
    public int GetSongId(string title)
    {
        int songId = 0;
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
                        songId = reader.GetInt32(reader.GetOrdinal("s_id"));
                    }
                }
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        if (songId == 0)
            throw new Exception("Provided title does not exist!");
        
        return songId;
    }
    
    public string GetSongTitle(int songId)
    {
        string songTitle = "";
        try
        {
            _connection.Open();
            string query = "SELECT * FROM songs WHERE s_id = @s_id";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@s_id", songId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        songTitle = reader.GetString(reader.GetOrdinal("title"));
                    }
                }
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        if (songTitle == "")
            throw new Exception("Provided identifier does not exist!");
        
        return songTitle;
    }

    public void DeleteSong(int songId)
    {
        int affectedRows = 0;
        try
        {
            _connection.Open();
            string query = "DELETE FROM songs WHERE s_id = @s_id";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@s_id", songId);
                affectedRows = command.ExecuteNonQuery();
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }        
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        
        if (affectedRows == 0)
            throw new Exception("Provided identifier does not exist!");
    }
    public void DeleteSong(string title)
    {
        int affectedRows = 0;
        try
        {
            _connection.Open();
            string query = $"DELETE FROM songs WHERE title = @title";
            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@title", title);
                command.ExecuteNonQuery();
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        
        if (affectedRows == 0)
            throw new Exception("Provided title does not exist!");
    }
    public List<int> GetPlaylist(int playlistId)
    {
        List<int> results = new List<int>();
        try
        {
            _connection.Open();
            string query = "SELECT s_id FROM play_queue WHERE p_id=@p_id";
    
            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@p_id", playlistId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(reader.GetInt32(reader.GetOrdinal("s_id")));
                    }
                }
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        if (results.Count == 0)
            throw new Exception("Provided identifier does not exist!");
        
        return results;
    }
    
    public List<int> GetPlaylist(string name)
    {
        List<int> results = new List<int>();
        int playlistId = 0;
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
                        playlistId = reader.GetInt32(reader.GetOrdinal("p_id"));
                    }
                }
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }        
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    
        results = GetPlaylist(playlistId);

        return results;
    }

    public int InsertNewPlaylist(string name, List<int> songIds)
    {
        if (songIds.Count == 0)
        {
            throw new Exception("A Playlist should have at least 1 song!");
        }

        try
        {
            int playlistId = 0;
            _connection.Open();
            using (SQLiteTransaction transaction = _connection.BeginTransaction())
            {
                string query = "INSERT INTO playlists(name) VALUES (@name) RETURNING p_id";

                using (SQLiteCommand command = new SQLiteCommand(query, _connection, transaction))
                {
                    command.Parameters.AddWithValue("@name", name);
                    playlistId = Convert.ToInt32(command.ExecuteScalar());
                }
            
                foreach (var songId in songIds)
                {
                    query = "INSERT INTO play_queue(p_id, s_id) VALUES (@p_id, @songId)";
                    using (SQLiteCommand command = new SQLiteCommand(query, _connection, transaction))
                    {
                        command.Parameters.AddWithValue("@p_id", playlistId);
                        command.Parameters.AddWithValue("@songId", songId);

                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
            return playlistId;
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
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }

    public void DeletePlaylist(string name)
    {
        int affectedRows = 0;
        try
        {
            _connection.Open();
            string query = "DELETE FROM playlists WHERE name = (@name)";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@name", name);
                affectedRows = command.ExecuteNonQuery();
            }
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        if (affectedRows == 0)
            throw new Exception("Provided title does not exist!");
    }

    public List<string> GetPlaylistNames()
    {
        try
        {
            List<string> result = new List<string>();
            
            _connection.Open();
            string query = "SELECT name FROM playlists";

            using (SQLiteCommand command = new SQLiteCommand(query, _connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(reader.GetOrdinal("name")));
                    }
                }
            }
            _connection.Close();
            return result;
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
