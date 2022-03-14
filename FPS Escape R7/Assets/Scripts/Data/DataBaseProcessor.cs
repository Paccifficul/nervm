using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Data
{
    public class DataBaseProcessor
    {
        private const string fileName = "db.bytes";//Название файла с ДБ, тоже по идее необязательно.
        private static string DBPath;
        private static SQLiteConnection connection;
        private static SQLiteCommand command;

        static DataBaseProcessor()
        {
            DBPath = GetDatabasePath();
        }

        /// <summary>
        /// Это метод для получения пути к ДБ. По идее, если договориться, где хранить то можно и без этого обойтись
        /// </summary>
        /// <returns></returns>
        private static string GetDatabasePath()
        {
            return "path/to/somewhere/where/you/want.bytes";
//#if UNITY_EDITOR
//            return Path.Combine(Application.streamingAssetsPath, fileName);
//#endif
//#if UNITY_STANDALONE
//            string filePath = Path.Combine(Application.dataPath, fileName);
//            if (!File.Exists(filePath)) UnpackDatabase(filePath);
//            return filePath;
//#elif UNITY_ANDROID
//    string filePath = Path.Combine(Application.persistentDataPath, fileName);
//    if(!File.Exists(filePath)) UnpackDatabase(filePath);
//    return filePath;
//#endif
        }

        /// <summary> 
        /// Распаковывает базу данных в указанный путь. 
        /// </summary>
        /// <param name="toPath"> Путь в который нужно распаковать базу данных. </param>
        private static void UnpackDatabase(string toPath)
        {
            string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

            UnityWebRequest reader = new UnityWebRequest(fromPath);
            while (!reader.isDone) { }

            File.WriteAllBytes(toPath, reader.downloadHandler.data);
        }

        public static void CloseConnection()
        {
            connection.Close();
            command = null;
        }

        public static void ExecuteQueryWithoutAnswer(string query)
        {
            connection = new SQLiteConnection("Data Source=" + DBPath);
            command = connection.CreateCommand(query);
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public static string ExecuteQueryWithAnswer(string query)
        {
            connection = new SQLiteConnection("Data Source=" + DBPath);
            command = connection.CreateCommand(query);
            command.CommandText = query;
            var answer = command.ExecuteScalar<object>();
            CloseConnection();

            if (answer != null) return answer.ToString();
            else return null;
        }

        public static TableQuery<T> GetTable<T>() where T : new()
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + DBPath);
                return connection.Table<T>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public static void CreateDatabase()
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + DBPath);
                connection.CreateTable<PlayerData>();
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
