﻿using System;
using System.Collections.Generic;
using Common;
using Entity;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UniversalWayTypeDAL
    {
        public int GetMaxId()
        {
            string SQL = "SELECT MAX(id) FROM UniversalTerWayType";
            object obj = SQLHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }

        public int GetMaxSequence(int parentId, TerType terType)
        {
            string SQL = "SELECT MAX(Sequence) FROM UniversalTerWayType WHERE ParentId='" + parentId + "' AND TerminalType='" + ((int)terType).ToString() + "'";
            object obj = SQLHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
        }

        public int TypeExist(UniversalCollectType type, string name, TerType terType)
        {
            string SQL = "SELECT COUNT(1) FROM UniversalTerWayType WHERE WayType='" + (int)type + "' AND Name='" + name + "' AND TerminalType='" + ((int)terType).ToString() + "'";
            object obj = SQLHelper.ExecuteScalar(SQL, null);
            if (obj != null && obj != DBNull.Value)
            {
                return (Convert.ToInt32(obj) > 0) ? 1 : 0;
            }
            else
            {
                return 0;
            }
        }

        public int Insert(UniversalWayTypeEntity entity,TerType terType)
        {
            if (entity == null)
                return 0;

            string SQL = @"INSERT INTO 
                            UniversalTerWayType(ID,Level,ParentID,WayType,Name,FrameWidth,Sequence,MaxMeasureRange,MaxMeasureRangeFlag,Precision,Unit,ModifyTime,TerminalType) VALUES(
                            @ID,@Level,@ParentID,@WayType,@Name,@FrameWidth,@Sequence,@MaxMeasureRange,@MaxMeasureRangeFlag,@Precision,@Unit,@ModifyTime,@terType)";
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@ID",DbType.Int32),
                new SqlParameter("@Level",DbType.Int32),
                new SqlParameter("@ParentID",DbType.Int32),
                new SqlParameter("@WayType",DbType.Int32),
                new SqlParameter("@Name",DbType.String),

                new SqlParameter("@FrameWidth",DbType.Int32),
                new SqlParameter("@Sequence",DbType.Int32),
                new SqlParameter("@MaxMeasureRange",DbType.Single),
                new SqlParameter("@MaxMeasureRangeFlag",DbType.Single),
                new SqlParameter("@Precision",DbType.Int32),

                new SqlParameter("@Unit",DbType.String),
                new SqlParameter("@ModifyTime",DbType.DateTime),
                new SqlParameter("@terType",DbType.Int32)
            };
            parms[0].Value = entity.ID;
            parms[1].Value = entity.Level;
            parms[2].Value = entity.ParentID;
            parms[3].Value = (int)entity.WayType;
            parms[4].Value = entity.Name;

            parms[5].Value = entity.FrameWidth;
            parms[6].Value = entity.Sequence;
            parms[7].Value = entity.MaxMeasureRange;
            parms[8].Value = entity.ManMeasureRangeFlag;
            parms[9].Value = entity.Precision;

            parms[10].Value = entity.Unit;
            parms[11].Value = entity.ModifyTime;
            parms[12].Value = (int)terType;

            SQLHelper.ExecuteNonQuery(SQL, parms);
            return 1;
        }

        public int Delete(int id)
        {
            if (id < 0)
                return 0;
            string SQL = "DELETE FROM UniversalTerWayType WHERE ID='" + id + "'";
            SQLHelper.ExecuteNonQuery(SQL, null);
            return 1;
        }
            
        public List<UniversalWayTypeEntity> Select(string where)
        {
            string SQL = "SELECT ID,Level,ParentID,WayType,Name,FrameWidth,Sequence,MaxMeasureRange,MaxMeasureRangeFlag,Precision,Unit,ModifyTime FROM UniversalTerWayType ";
            if (!string.IsNullOrEmpty(where))
                SQL +=  where;
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL, null))
            {
                List<UniversalWayTypeEntity> lst = new List<UniversalWayTypeEntity>();
                while (reader.Read())
                {
                    UniversalWayTypeEntity entity = new UniversalWayTypeEntity();

                    entity.ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : -1;
                    entity.Level = reader["Level"] != DBNull.Value ? Convert.ToInt32(reader["Level"]) : 0;
                    entity.ParentID = reader["ParentID"] != DBNull.Value ? Convert.ToInt32(reader["ParentID"]) : 0;
                    entity.WayType = reader["WayType"] != DBNull.Value ? (UniversalCollectType)(Convert.ToInt32(reader["WayType"])) : UniversalCollectType.Simulate;
                    entity.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "";

                    entity.FrameWidth = reader["FrameWidth"] != DBNull.Value ? Convert.ToInt32(reader["FrameWidth"]) : 2;
                    entity.Sequence = reader["Sequence"] != DBNull.Value ? Convert.ToInt32(reader["Sequence"]) : 1;
                    entity.MaxMeasureRange = reader["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(reader["MaxMeasureRange"]) : 0f;
                    entity.ManMeasureRangeFlag = reader["MaxMeasureRangeFlag"] != DBNull.Value ? Convert.ToSingle(reader["MaxMeasureRangeFlag"]) : 0f;
                    entity.Precision = reader["Precision"] != DBNull.Value ? Convert.ToInt32(reader["Precision"]) : 2;

                    entity.Unit = reader["Unit"] != DBNull.Value ? reader["Unit"].ToString() : "";
                    //entity.SyncState = reader["SyncState"] != DBNull.Value ? Convert.ToInt32(reader["SyncState"]) : 0;
                    entity.ModifyTime = reader["ModifyTime"] != DBNull.Value ? Convert.ToDateTime(reader["ModifyTime"]) : ConstValue.MinDateTime;

                    lst.Add(entity);
                }
                return lst;
            }
            return null;
        }

        /// <summary>
        /// 获得配置的PointID(不区分ID),id为空时，获取全部
        /// </summary>
        /// <returns></returns>
        public List<UniversalWayTypeEntity> GetConfigPointID(string id,TerType terType)
        {
            string SQL_Point = string.Format(@"SELECT ID,Level,ParentID,WayType,Name,FrameWidth,Sequence,MaxMeasureRange,MaxMeasureRangeFlag,Precision,Unit,ModifyTime 
                                FROM UniversalTerWayType WHERE ID IN (SELECT DISTINCT PointID FROM UniversalTerWayConfig WHERE TerminalType='{0}' {1}) AND TerminalType='{2}' ORDER BY WayType,Sequence", 
                                     (int)terType,(string.IsNullOrEmpty(id) ? "" : "AND TerminalID='" + id.Trim() + "'"),(int)terType);
            List<UniversalWayTypeEntity> lst = new List<UniversalWayTypeEntity>();
            using (SqlDataReader reader = SQLHelper.ExecuteReader(SQL_Point, null))
            {
                while (reader.Read())
                {
                    UniversalWayTypeEntity entity = new UniversalWayTypeEntity();

                    entity.ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : -1;
                    entity.Level = reader["Level"] != DBNull.Value ? Convert.ToInt32(reader["Level"]) : 0;
                    entity.ParentID = reader["ParentID"] != DBNull.Value ? Convert.ToInt32(reader["ParentID"]) : 0;
                    entity.WayType = reader["WayType"] != DBNull.Value ? (UniversalCollectType)(Convert.ToInt32(reader["WayType"])) : UniversalCollectType.Simulate;
                    entity.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "";

                    entity.FrameWidth = reader["FrameWidth"] != DBNull.Value ? Convert.ToInt32(reader["FrameWidth"]) : 2;
                    entity.Sequence = reader["Sequence"] != DBNull.Value ? Convert.ToInt32(reader["Sequence"]) : 1;
                    entity.MaxMeasureRange = reader["MaxMeasureRange"] != DBNull.Value ? Convert.ToSingle(reader["MaxMeasureRange"]) : 0f;
                    entity.ManMeasureRangeFlag = reader["MaxMeasureRangeFlag"] != DBNull.Value ? Convert.ToSingle(reader["MaxMeasureRangeFlag"]) : 0f;
                    entity.Precision = reader["Precision"] != DBNull.Value ? Convert.ToInt32(reader["Precision"]) : 2;

                    entity.Unit = reader["Unit"] != DBNull.Value ? reader["Unit"].ToString() : "";
                    //entity.SyncState = reader["SyncState"] != DBNull.Value ? Convert.ToInt32(reader["SyncState"]) : 0;
                    entity.ModifyTime = reader["ModifyTime"] != DBNull.Value ? Convert.ToDateTime(reader["ModifyTime"]) : ConstValue.MinDateTime;

                    lst.Add(entity);
                }
            }
            foreach (UniversalWayTypeEntity entity in lst)
            {
                entity.HaveChild = IDHaveChild(entity.ID.ToString());
            }
            return lst.Count>0 ? lst:null;
        }

        private bool IDHaveChild(string PointID)
        {
            string SQL = "SELECT COUNT(1) FROM UniversalTerWayType WHERE ParentID='" + PointID + "'";
            object obj_count = SQLHelper.ExecuteScalar(SQL, null);
            if (obj_count != null)
            {
                return Convert.ToInt32(obj_count) > 0 ? true : false;
            }
            return false;
        }

        public DataTable GetTerminalID_Configed(TerType terType)
        {
            string SQL = "SELECT DISTINCT TerminalID FROM UniversalTerWayConfig WHERE TerminalType='"+((int)terType).ToString()+"' ORDER BY TerminalID";
            DataTable dt = SQLHelper.ExecuteDataTable(SQL, null);
            return dt;
        }

        public DataTable GetTerminalDataToShow(List<string> lstTerID, List<int> lstTypeID)
        {
            if (lstTerID == null || lstTerID.Count == 0)
                return null;
            if (lstTypeID == null || lstTypeID.Count == 0)
                return null;

            string str_typeid = "";
            foreach (int typeid in lstTypeID)
            {
                str_typeid += "'" + typeid + "',";
            }
            str_typeid = str_typeid.Substring(0, str_typeid.Length - 1);
            string SQL = @"SELECT [TerminalID],[DataValue],[Simulate1Zero],[Simulate2Zero],[CollTime],[UnloadTime],[TypeTableID] FROM UniversalTerData 
                        WHERE ID IN(SELECT Max(ID) FROM UniversalTerData WHERE TypeTableID IN(" + str_typeid + ") GROUP BY TypeTableID)";
            DataTable dt_select = SQLHelper.ExecuteDataTable(SQL, null);

            if (dt_select != null && dt_select.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("TerminalID");
                for (int i = 1; i <= lstTypeID.Count; i++)
                    dt.Columns.Add("column" + i);  //column+index

                for (int i = 0; i < lstTerID.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["TerminalID"] = lstTerID[i];
                    DataRow[] dr_sel = dt_select.Select("TerminalID='" + lstTerID[i] + "'");
                    if (dr_sel != null && dr_sel.Length > 0)
                    {
                        foreach (DataRow dr_tmp in dr_sel)
                        {
                            int tabledid = Convert.ToInt32(dr_tmp["TypeTableID"]);
                            int index = lstTypeID.IndexOf(tabledid)+1;
                            dr["column" + index] = dr_tmp["DataValue"];
                        }
                    }

                    dt.Rows.Add(dr);
                }

                return dt;
            }
            else
                return null;
        }

        public int GetCofingSequence(string Terid,string pointid,TerType terType)
        {
            string SQL = "SELECT Sequence FROM UniversalTerWayConfig WHERE PointID ='" + pointid + "' AND TerminalID='" + Terid + "' AND TerminalType='"+((int)terType).ToString()+"'";
            object obj_sequence=SQLHelper.ExecuteScalar(SQL,null);
            if(obj_sequence!=null)
            {
                return Convert.ToInt32(obj_sequence);
            }
            return -1;
        }

    }
}
