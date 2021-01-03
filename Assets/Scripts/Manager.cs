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
    public string g_val;

    public static bool CheckDB(string dbName)
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

    public static int CreateDB(string dbName)
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

    public static int CreateTable(string dbName, string tableName)
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
            string q_createTable = "CREATE TABLE IF NOT EXISTS "+tableName+" (id INTEGER PRIMARY KEY, val TEXT )";
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

    public static int Insert(string dbName, string tableName,int id,string val)
    {
        string mval = "'" + val + "'";//字符串在SQLite中需加单引号
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            IDbCommand cmd_read = dbcon.CreateCommand();
            cmd_read.CommandText = "INSERT INTO "+ tableName + " (id,val) VALUES ("+ id + "," + mval + ")";
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

    public static int Delete(string dbName, string tableName, int id)
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

    public static int ReadTable_All(string dbName, string tableName)
    {
        try
        {
            bool r = false;
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
                r = true;
            }
            if(r==false)
            {
                print("no data");
            }
            dbcon.Close();
            return 0;
        }
        catch(System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }

    public static int FindMinimalId(string dbName,string tableName)
    {
        int reVal = 0;

        try
        {
            bool r = false;
            string connection = "URI=file:" + Application.persistentDataPath + "/" + dbName;
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();
            IDbCommand cmnd_read = dbcon.CreateCommand();
            string query = "SELECT * FROM " + tableName;
            cmnd_read.CommandText = query;
            IDataReader reader;
            reader = cmnd_read.ExecuteReader();
            int pre = -1;
            while (reader.Read())
            {
                //Debug.Log("[Data]\r\n" + "id: " + reader[0].ToString() + "\t" + "val: " + reader[1].ToString());
                reVal = int.Parse(reader[0].ToString());
                if (reVal-pre>1) // 跳行
                {
                    dbcon.Close();
                    return pre+1;
                }
                else if (reader[1].ToString() == "") // 空字符串
                {
                    dbcon.Close();
                    return reVal;
                }
                pre = reVal;
                r = true;
            }

            if (r == false)
            {
                dbcon.Close();
                return 0;
            }
            
            dbcon.Close();
            return reVal+1;
        }
        catch (System.Exception e)
        {
            print(e.ToString());
            return -1;
        }
    }

    // Use this for initialization
    void Start()
    {
        if(!CheckDB(g_dbName))
        {
            CreateDB(g_dbName);
            CreateTable(g_dbName, g_tbName);
        }

        Insert(g_dbName, g_tbName,
            FindMinimalId(g_dbName, g_tbName)
            , g_val);
        //Delete(g_dbName, g_tbName, g_id);
        ReadTable_All(g_dbName, g_tbName);

//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Insert(g_dbName, g_tbName,
                FindMinimalId(g_dbName, g_tbName)
                , g_val);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ReadTable_All(g_dbName, g_tbName);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Delete(g_dbName, g_tbName, g_id);
        }
    }
}
