using MobileOpenSesame.Models;
using MobileOpenSesame.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MobileOpenSesame.Models
{
    public static class LoginDataRepository
    {
        public static List<LoginData> logins;

        #region LoadingData
        public static List<LoginData> GetLoginData() 
        {
            //get data from database
            logins = Database.GetData();
            return logins;
        }

        public static LoginData GetLoginById(int id)
        {
            //get data from database
            return logins.Where(x => x.LoginId == id).FirstOrDefault();
        }

        public static List<LoginData> Search(string filter)
        {
            //search for text in LoginData, show results
            var result = logins.Where(x => x.Service != null && x.Service.ToLower().Contains(filter.ToLower())).ToList();
            result.AddRange(logins.Where(x => x.Username != null && x.Username.ToLower().Contains(filter.ToLower())).ToList());
            result.AddRange(logins.Where(x => x.Email != null && x.Email.ToLower().Contains(filter.ToLower())).ToList());
            result.AddRange(logins.Where(x => x.Category != null && x.Category.ToLower().Contains(filter.ToLower())).ToList());
            return result.Distinct().ToList();
        }

        #endregion

        #region DataManipulation
        public static string SaveData(int id, LoginData edit) 
        {
            //save data to database
            LoginData login = logins.Where(x => x.LoginId == id).FirstOrDefault();
            if (id!=0)
            {
                if (!Database.UpdateData(edit)) return "Error";
            }
            else
            {
                if (!Database.AddData(edit)) return "Error";
            }
            return "Success";
           
        }
        public static string DeleteData(int id) 
        {
            //delete data from database
            LoginData login = logins.Where(x => x.LoginId == id).FirstOrDefault();
            if(!Database.DeleteData(id)) return "Error";
            logins.Remove(login);
            return "Success";
        }

        public static string DeleteAllData()
        {
            //delete all data from database
            Database.DeleteAllData();
            logins.Clear();
            return "Success";
        }

        #endregion

    }
}
