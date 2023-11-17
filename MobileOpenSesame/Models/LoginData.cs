using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOpenSesame.Models
{
    public class LoginData
    {
        #region Properties
        [PrimaryKey, AutoIncrement]
        public int LoginId { get; set; }
        public string Service { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
        public int Used { get; set; } = 0;
        #endregion

        #region Constructor
        public LoginData() 
        {

        }

        public LoginData(int id, string service, string username, string password, string email, string category, string note, int used) 
        {
            this.LoginId = id;
            this.Service = service;
            this.Username = username;
            this.Password = password;
            this.Email = email;
            this.Category = category;
            this.Note = note;
            this.Used = used;
        }
        #endregion
    }
}
