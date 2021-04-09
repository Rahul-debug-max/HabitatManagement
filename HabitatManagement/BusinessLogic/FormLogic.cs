using HabitatManagement.BusinessEntities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HabitatManagement.BusinessLogic
{
    public class FormLogic
    {
        static string _connectionstring = Startup.ConnectionString;

        public static List<PermitFormScreenDesignTemplateBE> BlockFetchPermitFormScreenDesignTemplate(int pageIndex, int pageSize, out int totalRecords, string searchForm)
        {
            totalRecords = 0;
            List<PermitFormScreenDesignTemplateBE> list = new List<PermitFormScreenDesignTemplateBE>();

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplate_BlockFetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("SearchForm", searchForm ?? string.Empty);
                cmd.Parameters.AddWithValue("PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("PageSize", pageSize);
                cmd.Parameters.Add("RecordCount", SqlDbType.Int, 8);
                cmd.Parameters["RecordCount"].Direction = ParameterDirection.Output;
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToPermitFormScreenDesignTemplateBE(sqlDataReader));
                    }
                }
                if (cmd.Parameters["RecordCount"].Value != DBNull.Value)
                {
                    totalRecords = Convert.ToInt32(cmd.Parameters["RecordCount"].Value);
                }
            }
            return list;
        }

        public static PermitFormScreenDesignTemplateBE FetchPermitFormScreenDesignTemplate(int formId)
        {
            PermitFormScreenDesignTemplateBE o = null;

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplate_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToPermitFormScreenDesignTemplateBE(reader);
                }
            }
            return o;
        }

        public static bool AddPermitFormScreenDesignTemplate(PermitFormScreenDesignTemplateBE o, out int id)
        {
            bool success = false;
            id = 0;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplate_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<PermitFormScreenDesignTemplateBE>(o);
                FromPermitFormScreenDesignTemplateBE(ref cmd, o);
                cmd.Parameters.Add("ErrorOccured", SqlDbType.Bit);
                cmd.Parameters["ErrorOccured"].Direction = ParameterDirection.Output;
                cmd.Parameters["FormID"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["ErrorOccured"].Value != DBNull.Value)
                {
                    success = Convert.ToBoolean(cmd.Parameters["ErrorOccured"].Value);
                }
                if (cmd.Parameters["FormID"].Value != DBNull.Value)
                {
                    id = Convert.ToInt32(cmd.Parameters["FormID"].Value);
                }
            }
            return success;
        }

        public static bool UpdatePermitFormScreenDesignTemplate(PermitFormScreenDesignTemplateBE o)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplate_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<PermitFormScreenDesignTemplateBE>(o);
                FromPermitFormScreenDesignTemplateBE(ref cmd, o);
                cmd.Parameters.Add("ErrorOccured", SqlDbType.Bit);
                cmd.Parameters["ErrorOccured"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["ErrorOccured"].Value != DBNull.Value)
                {
                    success = Convert.ToBoolean(cmd.Parameters["ErrorOccured"].Value);
                }
            }
            return success;
        }

        public static List<PermitFormScreenDesignTemplateDetailBE> FetchAllPermitFormScreenDesignTemplateDetail(int formId)
        {
            List<PermitFormScreenDesignTemplateDetailBE> list = new List<PermitFormScreenDesignTemplateDetailBE>();
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplateDetail_FetchAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormId", formId);
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToPermitFormScreenDesignTemplateDetailBE(sqlDataReader));
                    }
                }
            }
            return list;
        }

        public static PermitFormScreenDesignTemplateDetailBE FetchPermitFormScreenDesignTemplateDetail(int formId, int field)
        {
            PermitFormScreenDesignTemplateDetailBE o = null;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplateDetail_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formId);
                cmd.Parameters.AddWithValue("Field", field);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToPermitFormScreenDesignTemplateDetailBE(reader);
                }
            }

            return o;
        }

        public static void AddPermitFormScreenDesignTemplateDetail(PermitFormScreenDesignTemplateDetailBE o)
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplateDetail_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<PermitFormScreenDesignTemplateDetailBE>(o);
                FromPermitFormScreenDesignTemplateDetailBE(ref cmd, o);
                cmd.ExecuteNonQuery();

            }
        }

        public static void UpdatePermitFormScreenDesignTemplateDetail(PermitFormScreenDesignTemplateDetailBE o)
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplateDetail_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<PermitFormScreenDesignTemplateDetailBE>(o);
                FromPermitFormScreenDesignTemplateDetailBE(ref cmd, o);
                cmd.Parameters.AddWithValue("Field", o.Field);
                cmd.ExecuteNonQuery();

            }
        }

        public static string GetDigitalSignature(int signatureId)
        {
            string base64String = string.Empty;
            try
            {
                DigitalSignatureBE signature = null;

                using (SqlConnection conn = new SqlConnection(_connectionstring))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("usp_DigitalSignature_Fetch", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("SignatureID", signatureId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            signature = ToDigitalSignatureBE(reader);
                    }
                }

                if (signature != null)
                {
                    base64String = Convert.ToBase64String(signature.Blob, 0, signature.Blob.Length);
                    base64String = "data:image/png;base64," + base64String;
                }
            }
            catch (Exception e)
            {

            }
            return base64String;
        }


        public DigitalSignatureBE FetchDigitalSignature(int surrogate)
        {
            DigitalSignatureBE o = null;

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_DigitalSignature_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("SignatureID", surrogate);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToDigitalSignatureBE(reader);
                }
            }
            return o;
        }

        public void AddDigitalSignature(DigitalSignatureBE o, out int createdSurrogate)
        {
            createdSurrogate = 0;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_DigitalSignature_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("Surrogate", SqlDbType.Int, 8).Direction = ParameterDirection.Output;
                BusinessEntityHelper.ReplaceNullProperties<DigitalSignatureBE>(o);
                FromDigitalSignatureBE(ref cmd, o);
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["Surrogate"].Value != DBNull.Value)
                {
                    createdSurrogate = Convert.ToInt32(cmd.Parameters["Surrogate"].Value);
                }
            }
        }

        public void UpdateDigitalSignature(DigitalSignatureBE o)
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_DigitalSignature_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<DigitalSignatureBE>(o);
                FromDigitalSignatureBE(ref cmd, o);
                cmd.Parameters.AddWithValue("LastUpdatedDate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }


        #region Private Methods

        private static DigitalSignatureBE ToDigitalSignatureBE(SqlDataReader rdr)
        {
            DigitalSignatureBE o = new DigitalSignatureBE();
            o.SignatureID = Convert.ToInt32(rdr["SignatureID"]);
            o.UserID = Convert.ToString(rdr["UserID"]);
            o.Blob = rdr["Blob"] != DBNull.Value ? (byte[])(rdr["Blob"]) : null;
            o.DigitalSignatoryTypeSurrogate = Convert.ToInt32(rdr["DigitalSignatoryTypeSurrogate"]);
            o.CreationDateTime = Convert.ToDateTime(rdr["CreationDateTime"]);
            o.LastUpdatedDate = Convert.ToDateTime(rdr["LastUpdatedDate"]);

            return o;
        }

        private void FromDigitalSignatureBE(ref SqlCommand cmd, DigitalSignatureBE o)
        {
            cmd.Parameters.AddWithValue("SignatureID", o.SignatureID);
            cmd.Parameters.AddWithValue("UserID", o.UserID);
            cmd.Parameters.AddWithValue("CreationDateTime", o.CreationDateTime);
            cmd.Parameters.AddWithValue("DigitalSignatoryTypeSurrogate", o.DigitalSignatoryTypeSurrogate);
            if (o.Blob != null)
            {
                cmd.Parameters.AddWithValue("Blob", o.Blob);
            }
            else
            {
                cmd.Parameters.AddWithValue("Blob", DBNull.Value);
                cmd.Parameters["Blob"].SqlDbType = SqlDbType.Image;
            }
        }

        private static PermitFormScreenDesignTemplateDetailBE ToPermitFormScreenDesignTemplateDetailBE(SqlDataReader rdr)
        {
            PermitFormScreenDesignTemplateDetailBE o = new PermitFormScreenDesignTemplateDetailBE();
            o.FormID = Convert.ToInt32(rdr["FormID"]);
            o.Field = Convert.ToInt32(rdr["Field"]);
            o.FieldName = Convert.ToString(rdr["FieldName"]);
            o.FieldType = (FormFieldType)Convert.ToInt32(rdr["FieldType"]);
            o.Section = (PromptFormSectionField)Convert.ToInt32(rdr["Section"]);
            o.Sequence = Convert.ToInt32(rdr["Sequence"]);
            return o;
        }

        private static void FromPermitFormScreenDesignTemplateDetailBE(ref SqlCommand cmd, PermitFormScreenDesignTemplateDetailBE o)
        {
            cmd.Parameters.AddWithValue("FormID", o.FormID);
            cmd.Parameters.AddWithValue("FieldName", o.FieldName);
            cmd.Parameters.AddWithValue("FieldType", (int)o.FieldType);
            cmd.Parameters.AddWithValue("Section", (int)o.Section);
            cmd.Parameters.AddWithValue("Sequence", o.Sequence);
        }

        private static PermitFormScreenDesignTemplateBE ToPermitFormScreenDesignTemplateBE(SqlDataReader sqlDataReader)
        {
            PermitFormScreenDesignTemplateBE designTemplate = new PermitFormScreenDesignTemplateBE();
            designTemplate.FormID = Convert.ToInt32(sqlDataReader["FormID"]);
            designTemplate.Design = Convert.ToString(sqlDataReader["Design"]);
            designTemplate.Description = Convert.ToString(sqlDataReader["Description"]);
            designTemplate.Active = Convert.ToBoolean(sqlDataReader["Active"]);
            designTemplate.CreatedDateTime = Convert.ToDateTime(sqlDataReader["CreatedDateTime"]);
            designTemplate.LastUpdatedDateTime = Convert.ToDateTime(sqlDataReader["LastUpdatedDateTime"]);
            designTemplate.UpdatedBy = Convert.ToString(sqlDataReader["UpdatedBy"]);
            designTemplate.CreatedBy = Convert.ToString(sqlDataReader["CreatedBy"]);
            return designTemplate;
        }

        private static void FromPermitFormScreenDesignTemplateBE(ref SqlCommand cmd, PermitFormScreenDesignTemplateBE o)
        {
            cmd.Parameters.AddWithValue("FormID", o.FormID);
            cmd.Parameters.AddWithValue("Design", o.Design);
            cmd.Parameters.AddWithValue("Description", o.Description);
            cmd.Parameters.AddWithValue("Active", o.Active);
            cmd.Parameters.AddWithValue("CreatedDateTime", o.CreatedDateTime);
            cmd.Parameters.AddWithValue("LastUpdatedDateTime", o.LastUpdatedDateTime);
            cmd.Parameters.AddWithValue("CreatedBy", o.CreatedBy);
            cmd.Parameters.AddWithValue("UpdatedBy", o.UpdatedBy);
        }

        #endregion
    }
}