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
        yield return null;
    }

    public IEnumerator RequestLoadMyUnit()
    {
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
                            data.def = reader.GetInt32(9);
                            data.cri = reader.GetFloat(10);
                            data.attackrange = reader.GetFloat(11);
                            data.grade = reader.GetInt32(12);
                            GameManager.HaveUnitData.Add(reader.GetInt32(0), data);
                        }
                    }

                    reader.Close();
                    Connection.Close();
                }
            }
        }
        yield return null;
    }

    public IEnumerator RequestRemoveUnit(int _uid)
    {
        using (IDbConnection Connection = new SqliteConnection(SqliteConnection))
        {
            Connection.Open();
            using (IDbCommand Cmd = Connection.CreateCommand())
            {
                string Query = string.Format("DELETE FROM UnitData WHERE uid={0}", _uid);
                Cmd.CommandText = Query;
                Cmd.ExecuteReader();
                GameManager.HaveUnitData.Remove(_uid);

                Connection.Close();
            }
        }
        yield return null;
    }
    #endregion

    #region USERDATA
    public IEnumerator RequestLoadUserData()
    {
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
                        GameManager.Instance.Exp = reader.GetInt32(1);
                        GameManager.Instance.Energy = reader.GetInt32(2);
                        GameManager.Instance.Gold = reader.GetInt32(3);
                        GameManager.Instance.Dia = reader.GetInt32(4);
                        GameManager.Instance.Heart = reader.GetInt32(5);
                    }

                    reader.Close();
                    Connection.Close();
                }
            }
        }
        UIManager.Instance.SetUpInfoData();
        yield return null;
    }

    public IEnumerator RequestUpdateUserData(int _type, int _value)
    {
        using (IDbConnection Connection = new SqliteConnection(SqliteConnection))
        {
            Connection.Open();
            using (IDbCommand Cmd = Connection.CreateCommand())
            {
                string Type = "";
                switch (_type)
                {
                    case 1: Type = "level"; break;
                    case 2: Type = "exp"; break;
                    case 3: Type = "energy"; break;
                    case 4: Type = "gold"; break;
                    case 5: Type = "dia"; break;
                    case 6: Type = "heart"; break;
                };
                string Quest = string.Format("UPDATE UserData SET {0}={1} WHERE rowid = 1", Type, _value);
                Cmd.CommandText = Quest;
                Cmd.ExecuteReader();
                Connection.Close();
            }
        }
        yield return RequestLoadUserData();
    }
    #endregion
}
