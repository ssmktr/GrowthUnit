using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.SqliteClient;
using System.IO;
using System.Data;

public class SqliteManager : Singleton<SqliteManager> {

    string SqliteConnection = "";

    public SqliteManager()
    {
        SqliteConnection = "URI=file:" + Application.streamingAssetsPath + "/GrowthUnit.sqlite";
    }

    #region UNITDATA
    public IEnumerator RequestGetUnit(int _id)
    {
        yield return null;

        using (IDbConnection dbConnection = new SqliteConnection(SqliteConnection))
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
                    dbConnection.Close();
                }
            }
        }
    }

    public IEnumerator RequestLoadMyUnit()
    {
        yield return null;

        using (IDbConnection Connection = new SqliteConnection(SqliteConnection))
        {
            Connection.Open();
            using (IDbCommand Cmd = Connection.CreateCommand())
            {
                string Query = "SELECT * FROM UnitData";
                Cmd.CommandText = Query;

                using (IDataReader reader = Cmd.ExecuteReader())
                {
                    GameManager.HaveUnitData.Clear();
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
                    Connection.Close();
                }
            }
        }
    }
    #endregion

    #region USERDATA
    public IEnumerator RequestLoadUserData()
    {
        yield return null;

        using (IDbConnection Connection = new SqliteConnection(SqliteConnection))
        {
            Connection.Open();
            using (IDbCommand Cmd = Connection.CreateCommand())
            {
                string Query = "SELECT * FROM UserData";
                Cmd.CommandText = Query;

                using (IDataReader reader = Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GameManager.Instance.Level = reader.GetInt32(0);
                        GameManager.Instance.Gold = reader.GetInt32(1);
                        GameManager.Instance.Dia = reader.GetInt32(2);
                        GameManager.Instance.Energy = reader.GetInt32(3);
                        GameManager.Instance.Heart = reader.GetInt32(4);
                        GameManager.Instance.Exp = reader.GetInt32(5);
                    }

                    reader.Close();
                    Connection.Close();
                }
            }
        }
    }
    #endregion
}
