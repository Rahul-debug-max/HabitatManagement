using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.BusinessEntities
{
    public class DigitalSignatureBE
    {
        public int SignatureID { get; set; }
        public string UserID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public byte[] Blob { get; set; }
        public int DigitalSignatoryTypeSurrogate { get; set; }
        public string DigitalSignatureImage64BitString
        {
            get
            {
                string base64String = string.Empty;
                if (Blob != null && Blob.Length > 0)
                {
                    try
                    {
                        using (MemoryStream mStream = new MemoryStream(Blob))
                        {
                            Image img = System.Drawing.Image.FromStream(mStream);
                            ImageFormat imageFormat = ImageFormat.Png;
                            base64String = string.Format("data:image/{0};base64,{1}", imageFormat.ToString().ToLower(), Convert.ToBase64String(Blob.ToArray(), 0, (int)Blob.Length));
                        }
                    }
                    catch (Exception e)
                    {
                        //throw (e);
                    }
                }
                return base64String;
            }

            set
            {
                this.Blob = Convert.FromBase64String(value.Replace("data:image/png;base64,", ""));
            }
        }
    }
}