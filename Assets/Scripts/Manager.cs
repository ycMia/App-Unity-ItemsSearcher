using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class Manager : MonoBehaviour
{
    public static void CreateDB(string dbName)
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/DB" + dbName;

        //必须要创建IDbConnection实现open和close, 才能成功创建一个空数据库
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        dbcon.Close();
    }

    public static int CreateTable(string dbName, string tableName)
    {
        int returnValue;
        string connection = "URI=file:" + Application.persistentDataPath + "/DB" + dbName;

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
        returnValue = dbcmd.ExecuteNonQuery();

        dbcon.Close();
        return returnValue;
    }

    public static int Insert(string dbName, string tableName,int id,int val)
    {
        int returnValue;
        string connection = "URI=file:" + Application.persistentDataPath + "/DB" + dbName;
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmd_read = dbcon.CreateCommand();
        cmd_read.CommandText = "INSERT INTO "+ tableName + " (id, val) VALUES ("+ id + ", "+val+")";
        returnValue = cmd_read.ExecuteNonQuery();

        dbcon.Close();
        return returnValue;
    }
    public static void ReadTable_All(string dbName, string tableName)
    {
        // Create database
        string connection = "URI=file:" + Application.persistentDataPath + "/DB" + dbName;

        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Read and print all values in table
        IDbCommand cmnd_read = dbcon.CreateCommand();

        // Edit Command
        string query = "SELECT * FROM " + tableName;
        cmnd_read.CommandText = query;

        IDataReader reader;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("[Data]\r\n"+"id: " + reader[0].ToString() + "\t"+"val: " + reader[1].ToString());
        }

        // Close connection
        dbcon.Close();
    }

    // Use this for initialization
    void Start()
    {
        string dbName = "002.db"; 
        string tbName = "defaultTable";
        CreateDB(dbName);
        CreateTable(dbName, tbName);
        Insert(dbName, tbName, 0, 5);
        ReadTable_All(dbName, tbName);
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