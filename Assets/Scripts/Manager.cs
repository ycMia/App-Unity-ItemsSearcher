using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;

//SQlite:https://github.com/rizasif/sqlite-unity-plugin

public class Manager : MonoBehaviour
{
    public string g_dbName;// = "002.db"
    public string g_tbName;// = "defaultTable"
    public int g_id;
    public string g_val1;
    public string g_val2;

    public Text OutText;

    internal bool CheckDB(string dbName)
    {
        string dbPath = Application.persistentDataPath + "/" + dbName;

        if (File.Exists(dbPath))
        { 
            print("CheckDB:存在\"" + dbPath + "\"处的数据库");
            return true;
        }
        else
        {
            print("CheckDB:不存在\"" + dbPath + "\"处的数据库");
            return false;
        }
    }

    internal int CreateDB(string dbName)
    {
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            //必须要创建IDbConnection实现open和close, 才能成功创建一个空数据库
            print("CreateDB:Creating\"" + Application.persistentDataPath + "/" + dbName+"\"...");
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();
            dbcon.Close();
            return 0;
        }
        catch(System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }

    internal int CreateTable(string dbName, string tableName)
    {
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            // Open connection
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            // Create table
            IDbCommand dbcmd;
            dbcmd = dbcon.CreateCommand();
            string q_createTable = "CREATE TABLE IF NOT EXISTS "+tableName+" (id INTEGER PRIMARY KEY, val1 TEXT ,val2 TEXT)";
            dbcmd.CommandText = q_createTable;
            dbcmd.ExecuteNonQuery();//Execute

            dbcon.Close();
            return 0;
        }
        catch(System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }

    internal int Insert(string dbName, string tableName,int id,string val1 , string val2)
    {
        string mval1 = "'" + val1 + "'";
        string mval2 = "'" + val2 + "'";
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            IDbCommand cmd_read = dbcon.CreateCommand();
            cmd_read.CommandText = "INSERT INTO "+ tableName + " (id,val1,val2) VALUES ("+ id + "," + mval1 + "," + mval2 + ")";
            cmd_read.ExecuteNonQuery();

            dbcon.Close();
            return 0;
        }
        catch(System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }

    internal int Remove(string dbName, string tableName, int id)
    {
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            IDbCommand cmd_read = dbcon.CreateCommand();
            cmd_read.CommandText = "DELETE FROM " + tableName + " WHERE id=" + id;
            cmd_read.ExecuteNonQuery();

            dbcon.Close();
            return 0;
        }
        catch (System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }
    
    internal int ReadTable_All(string dbName, string tableName)
    {
        string rstr = "[Data]";
        try
        {
            bool r = false;
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();
            IDbCommand cmd_read = dbcon.CreateCommand();
            string query = "SELECT * FROM " + tableName;
            cmd_read.CommandText = query;
            IDataReader reader;
            reader = cmd_read.ExecuteReader();
            while (reader.Read())
            {
                rstr += "\r\n"+"id: " + reader[0].ToString() + "\t"+"val1: " + reader[1].ToString() + "\t" + "val2: " + reader[2].ToString();
                r = true;
            }
            if(r==false)
            {
                rstr += "\r\nno data";
            }
            dbcon.Close();
            print(rstr);
            return 0;
        }
        catch(System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }

    internal int FindMinimalId(string dbName,string tableName)
    {
        int reVal = 0;
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();
            IDbCommand cmd = dbcon.CreateCommand();
            string scmd = "SELECT min(id) FROM "+ tableName + ";";
            cmd.CommandText = scmd;
            IDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader[0].ToString() != "")
                {
                    reVal = int.Parse(reader[0].ToString());
                }
                //else reVal = 0;//从 0 开始计 id
            }
            dbcon.Close();
            print(reVal);
            return reVal;
        }
        catch (System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }

    internal int FindMaximalId(string dbName, string tableName)
    {
        int reVal = 0;
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();
            IDbCommand cmd = dbcon.CreateCommand();
            string scmd = "SELECT max(id) FROM " + tableName + ";";
            cmd.CommandText = scmd;
            IDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader[0].ToString() != "")
                {
                    reVal = int.Parse(reader[0].ToString());
                }
                //else reVal = 0;//从 0 开始计 id
            }
            dbcon.Close();
            print(reVal);
            return reVal;
        }
        catch (System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }

    void Start()
    {
        if(!CheckDB(g_dbName))
        {
            CreateDB(g_dbName);
            CreateTable(g_dbName, g_tbName);

            Insert(g_dbName, g_tbName, 0, "jafiownb", "faiwjbia");
            Insert(g_dbName, g_tbName, 1, "jafiownb", "faiwjbia");
            Insert(g_dbName, g_tbName, 2, "jafiownb", "faiwjbia");
            Insert(g_dbName, g_tbName, 3, "jafiownb", "faiwjbia");
            Insert(g_dbName, g_tbName, 4, "jafiownb", "faiwjbia");
        }

        ReadTable_All(g_dbName, g_tbName);
    }
    
    void Update()
    {

    }

    public  void G_PauseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}