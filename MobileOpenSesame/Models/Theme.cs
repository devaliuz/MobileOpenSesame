using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOpenSesame.Models
{
    public class Theme 
    {
        #region Properties
        [PrimaryKey, AutoIncrement]
        public int ThemeID { get; set;}
        public string Name { get; set; }    
        public bool Active { get; set; } = false;
        public string BackgroundColor { get; set; }
        #endregion

        #region Constructor
        public Theme()
        {

        }

        public Theme(int id, string name, bool active, string back)
        {
            this.ThemeID = id;
            this.Name = name;
            this.Active = active;
            this.BackgroundColor = back;
        }
        #endregion
    }
}
