using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HabitatManagement.Business
{
    public class FormLogic
    {
        static string _connectionstring = DBConfiguration.Connection;

        public static List<FormDesignTemplateBE> BlockFetchFormDesignTemplate(int pageIndex, int pageSize, out int totalRecords, string searchForm)
        {
            totalRecords = 0;
            List<FormDesignTemplateBE> list = new List<FormDesignTemplateBE>();

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplate_BlockFetch", conn);
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
                        list.Add(ToFormDesignTemplateBE(sqlDataReader));
                    }
                }
                if (cmd.Parameters["RecordCount"].Value != DBNull.Value)
                {
                    totalRecords = Convert.ToInt32(cmd.Parameters["RecordCount"].Value);
                }
            }
            return list;
        }

        public static FormDesignTemplateBE FetchFormDesignTemplate(int formId)
        {
            FormDesignTemplateBE o = null;

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplate_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToFormDesignTemplateBE(reader);
                }
            }
            return o;
        }

        public static bool AddFormDesignTemplate(FormDesignTemplateBE o, out int id)
        {
            bool success = false;
            id = 0;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplate_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<FormDesignTemplateBE>(o);
                FromFormDesignTemplateBE(ref cmd, o);
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

        public static bool UpdateFormDesignTemplate(FormDesignTemplateBE o)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplate_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<FormDesignTemplateBE>(o);
                FromFormDesignTemplateBE(ref cmd, o);
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

        public static List<FormDesignTemplateDetailBE> FetchAllFormDesignTemplateDetail(int formId)
        {
            List<FormDesignTemplateDetailBE> list = new List<FormDesignTemplateDetailBE>();
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplateDetail_FetchAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormId", formId);
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToFormDesignTemplateDetailBE(sqlDataReader));
                    }
                }
            }
            return list;
        }

        public static List<FormDesignTemplateDetailBE> BlockFetchFormDesignTemplateDetail(int formId, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            List<FormDesignTemplateDetailBE> list = new List<FormDesignTemplateDetailBE>();

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplateDetail_BlockFetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormId", formId);
                cmd.Parameters.AddWithValue("PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("PageSize", pageSize);
                cmd.Parameters.Add("RecordCount", SqlDbType.Int, 8);
                cmd.Parameters["RecordCount"].Direction = ParameterDirection.Output;
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToFormDesignTemplateDetailBE(sqlDataReader));
                    }
                }
                if (cmd.Parameters["RecordCount"].Value != DBNull.Value)
                {
                    totalRecords = Convert.ToInt32(cmd.Parameters["RecordCount"].Value);
                }
            }
            return list;
        }

        public static FormDesignTemplateDetailBE FetchFormDesignTemplateDetail(int formId, int field)
        {
            FormDesignTemplateDetailBE o = null;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplateDetail_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formId);
                cmd.Parameters.AddWithValue("Field", field);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToFormDesignTemplateDetailBE(reader);
                }
            }

            return o;
        }

        public static bool AddFormDesignTemplateDetail(FormDesignTemplateDetailBE o, out int field)
        {
            bool success = false;
            field = 0;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplateDetail_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("Field", SqlDbType.Int, 8).Direction = ParameterDirection.Output;
                BusinessEntityHelper.ReplaceNullProperties<FormDesignTemplateDetailBE>(o);
                FromFormDesignTemplateDetailBE(ref cmd, o);
                cmd.ExecuteNonQuery();
                success = true;
                if (cmd.Parameters["Field"].Value != DBNull.Value)
                {
                    field = Convert.ToInt32(cmd.Parameters["Field"].Value);
                }

            }
            return success;
        }

        public static bool UpdateFormDesignTemplateDetail(FormDesignTemplateDetailBE o)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplateDetail_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<FormDesignTemplateDetailBE>(o);
                FromFormDesignTemplateDetailBE(ref cmd, o);
                cmd.Parameters.AddWithValue("Field", o.Field);
                cmd.ExecuteNonQuery();
                success = true;
            }
            return success;
        }

        public static bool DeleteFormDesignTemplateDetail(int formID, int FieldID)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_FormDesignTemplateDetail_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formID);
                cmd.Parameters.AddWithValue("Field", FieldID);
                cmd.ExecuteNonQuery();
                success = true;
            }
            return success;
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


        public static DigitalSignatureBE FetchDigitalSignature(int surrogate)
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

        public static void AddDigitalSignature(DigitalSignatureBE o, out int createdSurrogate)
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

        public static void UpdateDigitalSignature(DigitalSignatureBE o)
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

        public static List<TemplateFormFieldDataBE> FetchAllTemplateFormFieldData(int formId, int surrogate)
        {
            List<TemplateFormFieldDataBE> list = new List<TemplateFormFieldDataBE>();
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_TemplateFormFieldData_FetchAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormId", formId);
                cmd.Parameters.AddWithValue("Surrogate", surrogate);
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToTemplateFormFieldDataBE(sqlDataReader));
                    }
                }
            }
            return list;
        }

        public static bool SaveTemplateFormFieldData(TemplateFormFieldDataBE o)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TemplateFormFieldData_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<TemplateFormFieldDataBE>(o);
                FromTemplateFormFieldDataBE(ref cmd, o);
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

        public static int GetMaxProjectFormSurroagate()
        {
            int formSurrogate = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionstring))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("usp_TemplateFormFieldData_Fetch_MaxSurrogate", conn);
                    cmd.CommandType = CommandType.StoredProcedure;                                        
                    formSurrogate = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch
            {

            }
            return formSurrogate;
        }

        public static List<TemplateFormFieldDataBE> BlockFetchByForm(int pageIndex, int pageSize, out int totalRecords, int formID)
        {
            totalRecords = 0;
            List<TemplateFormFieldDataBE> list = new List<TemplateFormFieldDataBE>();

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_TemplateFormFieldData_BlockFetch_ByForm", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formID);
                cmd.Parameters.AddWithValue("PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("PageSize", pageSize);
                cmd.Parameters.Add("RecordCount", SqlDbType.Int, 8);
                cmd.Parameters["RecordCount"].Direction = ParameterDirection.Output;
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToTemplateFormFieldDataByFormBE(sqlDataReader));
                    }
                }
                if (cmd.Parameters["RecordCount"].Value != DBNull.Value)
                {
                    totalRecords = Convert.ToInt32(cmd.Parameters["RecordCount"].Value);
                }
            }
            return list;
        }


        #region Template Form Section

        public static List<TemplateFormSectionBE> FetchAllTemplateFormSection(int formId)
        {
            List<TemplateFormSectionBE> list = new List<TemplateFormSectionBE>();
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_TemplateFormSection_FetchAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormId", formId);
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToTemplateFormSectionBE(sqlDataReader));
                    }
                }
            }
            return list;
        }

        public static TemplateFormSectionBE FetchTemplateFormSection(int formId, string section)
        {
            TemplateFormSectionBE o = null;

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TemplateFormSection_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formId);
                cmd.Parameters.AddWithValue("Section", section);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToTemplateFormSectionBE(reader);
                }
            }
            return o;
        }

        public static bool AddTemplateFormSection(TemplateFormSectionBE o)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TemplateFormSection_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<TemplateFormSectionBE>(o);
                FromTemplateFormSection(ref cmd, o);
                cmd.ExecuteNonQuery();
                success = true;
            }
            return success;
        }

        public static bool UpdateTemplateFormSection(TemplateFormSectionBE o)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TemplateFormSection_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<TemplateFormSectionBE>(o);
                FromTemplateFormSection(ref cmd, o);
                cmd.ExecuteNonQuery();
                success = true;
            }
            return success;
        }

        public static bool DeleteTemplateFormSection(int formId, string section)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TemplateFormSection_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formId);
                cmd.Parameters.AddWithValue("Section", section);
                cmd.ExecuteNonQuery();
                success = true;
            }
            return success;
        }

        #endregion


        #region Table Field Type Master

        public static TableFieldTypeMasterBE FetchTableFieldTypeMaster(int id, int field)
        {
            TableFieldTypeMasterBE o = null;

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TableFieldTypeMaster_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", id);
                cmd.Parameters.AddWithValue("Field", field);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToTableFieldTypeMasterBE(reader);
                }
            }
            return o;
        }

        public static List<TableFieldTypeMasterBE> FetchAllTableFieldTypeMaster(int field)
        {
            List<TableFieldTypeMasterBE> list = new List<TableFieldTypeMasterBE>();
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_TableFieldTypeMaster_FetchAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Field", field);
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToTableFieldTypeMasterBE(sqlDataReader));
                    }
                }
            }
            return list;
        }

        public static void AddTableFieldTypeMaster(TableFieldTypeMasterBE o)
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TableFieldTypeMaster_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<TableFieldTypeMasterBE>(o);
                FromTableFieldTypeMasterBE(ref cmd, o);
                cmd.ExecuteNonQuery();
            }
        }

        public static bool DeleteTableFieldTypeMaster(int field)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TableFieldTypeMaster_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Field", field);
                cmd.ExecuteNonQuery();
                success = true;
            }
            return success;
        }

        #endregion

        #region Table Field Type Master Data


        public static void AddTableFieldTypeMasterData(TableFieldTypeMasterDataBE o)
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TableFieldTypeMasterData_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                BusinessEntityHelper.ReplaceNullProperties<TableFieldTypeMasterDataBE>(o);
                FromTableFieldTypeMasterDataBE(ref cmd, o);
                cmd.ExecuteNonQuery();
            }
        }

        public static bool DeleteTableFieldTypeMasterData(int tableFieldTypeMasterId)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_TableFieldTypeMasterData_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("TableFieldTypeMasterId", tableFieldTypeMasterId);
                cmd.ExecuteNonQuery();
                success = true;
            }
            return success;
        }

        public static List<TableFieldTypeMasterDataBE> FetchAllTableFieldTypeMasterData(int field)
        {
            List<TableFieldTypeMasterDataBE> list = new List<TableFieldTypeMasterDataBE>();
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_TableFieldTypeMasterData_FetchAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Field", field);
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToTableFieldTypeMasterDataBE(sqlDataReader));
                    }
                }
            }
            return list;
        }

        #endregion

        #region Private Methods

        private static void FromTableFieldTypeMasterDataBE(ref SqlCommand cmd, TableFieldTypeMasterDataBE o)
        {
            cmd.Parameters.AddWithValue("Id", o.Id);
            cmd.Parameters.AddWithValue("TableFieldTypeMasterId", o.TableFieldTypeMasterId);
            cmd.Parameters.AddWithValue("RowColumnValue", o.RowColumnValue);
        }

        private static TableFieldTypeMasterDataBE ToTableFieldTypeMasterDataBE(SqlDataReader rdr)
        {
            TableFieldTypeMasterDataBE o = new TableFieldTypeMasterDataBE();
            o.Id = Functions.ToInt(rdr["Id"]);
            o.TableFieldTypeMasterId = Functions.ToInt(rdr["TableFieldTypeMasterId"]);
            o.RowColumnValue = Functions.TrimRight(rdr["RowColumnValue"]);
            return o;
        }


        private static void FromTableFieldTypeMasterBE(ref SqlCommand cmd, TableFieldTypeMasterBE o)
        {
            cmd.Parameters.AddWithValue("Field", o.Field);
            cmd.Parameters.AddWithValue("ColumnName", o.ColumnName);
            cmd.Parameters.AddWithValue("RowCount", o.RowCount);
            cmd.Parameters.AddWithValue("ColumnType", o.ColumnType);
        }

        private static TableFieldTypeMasterBE ToTableFieldTypeMasterBE(SqlDataReader rdr)
        {
            TableFieldTypeMasterBE o = new TableFieldTypeMasterBE();
            o.Id = Functions.ToInt(rdr["Id"]);
            o.Field = Functions.ToInt(rdr["Field"]);
            o.ColumnName = Functions.TrimRight(rdr["ColumnName"]);
            o.RowCount = Functions.ToInt(rdr["RowCount"]);
            o.ColumnType = (FormFieldType)Functions.ToInt(rdr["ColumnType"]);
            return o;
        }

        private static void FromTemplateFormFieldDataBE(ref SqlCommand cmd, TemplateFormFieldDataBE o)
        {
            cmd.Parameters.AddWithValue("Surrogate", o.Surrogate);
            cmd.Parameters.AddWithValue("FormID", o.FormID);            
            cmd.Parameters.AddWithValue("Field", o.Field);
            cmd.Parameters.AddWithValue("FieldValue", o.FieldValue);
            cmd.Parameters.AddWithValue("CreationDate", o.CreationDate);
        }

        private static TemplateFormSectionBE ToTemplateFormSectionBE(SqlDataReader rdr)
        {
            TemplateFormSectionBE o = new TemplateFormSectionBE();
            o.FormID = Functions.ToInt(rdr["FormID"]);
            o.Section = Functions.TrimRight(rdr["Section"]);
            o.Description = Functions.TrimRight(rdr["Description"]);
            o.Sequence = Functions.ToInt(rdr["Sequence"]);
            return o;
        }

        private static void FromTemplateFormSection(ref SqlCommand cmd, TemplateFormSectionBE o)
        {
            cmd.Parameters.AddWithValue("FormID", o.FormID);
            cmd.Parameters.AddWithValue("Section", o.Section);
            cmd.Parameters.AddWithValue("Description", o.Description);
            cmd.Parameters.AddWithValue("Sequence", o.Sequence);
        }

        private static TemplateFormFieldDataBE ToTemplateFormFieldDataBE(SqlDataReader rdr)
        {
            TemplateFormFieldDataBE o = new TemplateFormFieldDataBE();
            o.Surrogate = Convert.ToInt32(rdr["Surrogate"]);
            o.FormID = Convert.ToInt32(rdr["FormID"]);
            o.Field = Convert.ToInt32(rdr["Field"]);
            o.FieldValue = Convert.ToString(rdr["FieldValue"]);
            o.CreationDate = Functions.ToDateTime(rdr["CreationDate"]);
            return o;
        }

        private static TemplateFormFieldDataBE ToTemplateFormFieldDataByFormBE(SqlDataReader rdr)
        {
            TemplateFormFieldDataBE o = new TemplateFormFieldDataBE();
            o.Surrogate = Convert.ToInt32(rdr["Surrogate"]);
            o.FormID = Convert.ToInt32(rdr["FormID"]);
            o.Design = Functions.TrimRight(rdr["Design"]);
            o.Description = Functions.TrimRight(rdr["Description"]);
            o.CreationDate = Functions.ToDateTime(rdr["CreationDate"]);
            return o;
        }

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

        private static void FromDigitalSignatureBE(ref SqlCommand cmd, DigitalSignatureBE o)
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

        private static FormDesignTemplateDetailBE ToFormDesignTemplateDetailBE(SqlDataReader rdr)
        {
            FormDesignTemplateDetailBE o = new FormDesignTemplateDetailBE();
            o.FormID = Functions.ToInt(rdr["FormID"]);
            o.Field = Functions.ToInt(rdr["Field"]);
            o.FieldName = Functions.TrimRight(rdr["FieldName"]);
            o.FieldType = (FormFieldType)Functions.ToInt(rdr["FieldType"]);
            o.Section = Functions.TrimRight(rdr["Section"]);
            o.Sequence = Functions.ToInt(rdr["Sequence"]);
            o.SectionDescription = Functions.TrimRight(rdr["SectionDescription"]);
            o.SectionSequence = Functions.ToInt(rdr["SectionSequence"]);
            return o;
        }

        private static void FromFormDesignTemplateDetailBE(ref SqlCommand cmd, FormDesignTemplateDetailBE o)
        {
            cmd.Parameters.AddWithValue("FormID", o.FormID);
            cmd.Parameters.AddWithValue("FieldName", o.FieldName);
            cmd.Parameters.AddWithValue("FieldType", (int)o.FieldType);
            cmd.Parameters.AddWithValue("Section", o.Section);
            cmd.Parameters.AddWithValue("Sequence", o.Sequence);
        }

        private static FormDesignTemplateBE ToFormDesignTemplateBE(SqlDataReader sqlDataReader)
        {
            FormDesignTemplateBE designTemplate = new FormDesignTemplateBE();
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

        private static void FromFormDesignTemplateBE(ref SqlCommand cmd, FormDesignTemplateBE o)
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