using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

//SQlite:https://github.com/rizasif/sqlite-unity-plugin

public class Manager : MonoBehaviour
{
    public string g_dbName;// = "002.db"
    public string g_tbName;// = "defaultTable"
    public int g_id;
    public int g_val;
    public static void CreateDB(string dbName)
    {
        string dbPath = Application.persistentDataPath + "/" + dbName;
        
        if(File.Exists(dbPath))
        {
            print("存在\"" + dbPath + "\"处的数据库");
        }
        else
        {
            print("不存在\"" + dbPath + "\"处的数据库,创建中...");
            try
            {
                string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
                //必须要创建IDbConnection实现open和close, 才能成功创建一个空数据库
                IDbConnection dbcon = new SqliteConnection(connection);
                dbcon.Open();
                dbcon.Close();
            }
            catch(System.Exception e)
            {
                print(e.ToString());
            }
        }
    }

    public static void CreateTable(string dbName, string tableName)
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

            // Edit Command
            string q_createTable = "CREATE TABLE IF NOT EXISTS "+tableName+" (id INTEGER PRIMARY KEY, val INTEGER )";
            dbcmd.CommandText = q_createTable;

            //Execute
            dbcmd.ExecuteNonQuery();

            dbcon.Close();
        }
        catch(System.Exception e)
        {
            print(e.ToString());
        }
    }

    public static void Insert(string dbName, string tableName,int id,int val)
    {
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            IDbCommand cmd_read = dbcon.CreateCommand();
            cmd_read.CommandText = "INSERT INTO "+ tableName + " (id, val) VALUES ("+ id + ", "+val+")";
            cmd_read.ExecuteNonQuery();

            dbcon.Close();
        }
        catch(System.Exception e)
        {
            print(e.ToString());
        }
    }

    public static void Delete(string dbName, string tableName, int id)
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
        }
        catch (System.Exception e)
        {
            print(e.ToString());
        }
    }

    public static void ReadTable_All(string dbName, string tableName)
    {
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();
            IDbCommand cmnd_read = dbcon.CreateCommand();
            string query = "SELECT * FROM " + tableName;
            cmnd_read.CommandText = query;
            IDataReader reader;
            reader = cmnd_read.ExecuteReader();
            while (reader.Read())
            {
                Debug.Log("[Data]\r\n"+"id: " + reader[0].ToString() + "\t"+"val: " + reader[1].ToString());
            }
            dbcon.Close();
        }
        catch(System.Exception e)
        {
            print(e.ToString());
        }
    }

    // Use this for initialization
    void Start()
    {
        CreateDB(g_dbName);
        CreateTable(g_dbName, g_tbName);
        Insert(g_dbName, g_tbName, g_id, g_val);
        //Delete(g_dbName, g_tbName, g_id);
        ReadTable_All(g_dbName, g_tbName);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}
