using System;

namespace DecodeWebApp.Models
{
    public class Token
    {
        public string Value { get; set; }
        public string Type { get; set; }

        private DateTime created;
        public string Created
        {
            get { return created.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { created = Convert.ToDateTime(value); }
        }

        private DateTime expiration;

        public string Expiration
        {
            get { return expiration.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { expiration = Convert.ToDateTime(value); }
        }

        public bool Authenticated { get; set; }
    }
}
