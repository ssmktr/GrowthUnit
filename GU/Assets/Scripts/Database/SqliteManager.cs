﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.SqliteClient;
using System.IO;
using System.Data;

public class SqliteManager : MonoBehaviour {

    public static void LoadSqliteData<T>(string _URL, System.Action<IDataReader> _ResponseCallBack)
    {
        
    }

    public static IEnumerator RequestGetUnit(int _id)
    {
        yield return null;

        string ConnectionString = "URI=file:" + Application.streamingAssetsPath + "/GrowthUnit.sqlite";

        using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                UnitDataBase.Data Unit = DataManager.GetUnitData(_id);
                if (Unit != null)
                {
                    string Query = string.Format(
                        "INSERT INTO UnitData (id, stringid, type, classtype, move_speed, hp, atk, def, cri, attackrange) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})"
                        , Unit.id, Unit.stringid, Unit.type, Unit.classtype, Unit.move_speed, Unit.hp, Unit.atk, Unit.def, Unit.cri, Unit.attackrange);

                    dbCmd.CommandText = Query;
                    dbCmd.ExecuteReader();

                    Query = "SELECT * FROM UnitData";
                    dbCmd.CommandText = Query;

                    using (IDataReader reader = dbCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!GameManager.HaveUnitData.ContainsKey(reader.GetInt32(0)))
                            {
                                NetData.UnitData data = new NetData.UnitData();
                                data.uid = reader.GetInt32(0);
                                data.id = reader.GetInt32(1);
                                data.level = reader.GetInt32(2);
                                data.stringid = reader.GetInt32(3);
                                data.type = reader.GetInt32(4);
                                data.classtype = reader.GetInt32(5);
                                data.move_speed = reader.GetFloat(6);
                                data.hp = reader.GetFloat(7);
                                data.atk = reader.GetInt32(8);
                                data.cri = reader.GetFloat(9);
                                data.attackrange = reader.GetFloat(10);
                                GameManager.HaveUnitData.Add(reader.GetInt32(0), data);
                            }
                        }

                        reader.Close();
                        dbConnection.Close();
                    }
                }
            }
        }
    }

    public static IEnumerator RequestLoadMyUnit()
    {
        yield return null;

        string ConnectionString = "URI=file:" + Application.streamingAssetsPath + "/GrowthUnit.sqlite";

        using (IDbConnection Connection = new SqliteConnection(ConnectionString))
        {
            Connection.Open();
            using (IDbCommand Cmd = Connection.CreateCommand())
            {
                string Query = "SELECT * FROM UnitData";
                Cmd.CommandText = Query;

                using (IDataReader reader = Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NetData.UnitData data = new NetData.UnitData();
                        data.uid = reader.GetInt32(0);
                        data.id = reader.GetInt32(1);
                        data.level = reader.GetInt32(2);
                        data.stringid = reader.GetInt32(3);
                        data.type = reader.GetInt32(4);
                        data.classtype = reader.GetInt32(5);
                        data.move_speed = reader.GetFloat(6);
                        data.hp = reader.GetFloat(7);
                        data.atk = reader.GetInt32(8);
                        data.cri = reader.GetFloat(9);
                        data.attackrange = reader.GetFloat(10);
                        GameManager.HaveUnitData.Add(reader.GetInt32(0), data);
                    }

                    reader.Close();
                    Connection.Close();
                }
            }
        }
    }
}