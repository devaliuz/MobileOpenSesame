using MobileOpenSesame.Models;
using SQLite;

namespace MobileOpenSesame.Service
{
    public class Database
    {
        static SQLiteConnection database;

        #region Constructor
        public Database()
        {
            Init();   
        }
        #endregion

        #region Init

        public static bool Init()
        {
            //if database is already initialized, return true
            if (database != null) return true;
            //else create database
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MobileOpenSesame.db").ToString();
            database = new SQLiteConnection(databasePath);
            //create tables
            database.CreateTable<LoginData>();
            database.CreateTable<Theme>();
            return true;
        }

        #endregion

        #region Edit LoginDatabase

        #region WriteData
        public static bool AddData(LoginData edit)
        {
            //add new data to database
            database.Insert(edit);
            return true;
        }

        public static bool UpdateData(LoginData edit)
        {
            //update data in database
            database.Update(edit);
            return true;
        }
        #endregion

        #region DeleteData
        public static bool DeleteData(int id)
        {
            //delete data from database
            database.Delete<LoginData>(id);
            return true;
        }

        public static void DeleteAllData()
        {
            //delete all data from database
            database.Close();
            database.DropTable<LoginData>();
        }
        #endregion

        #endregion

        #region GetLoginData
        public static List<LoginData> GetData()
        {
            //get all data from database
            try
            {
                List<LoginData> data = database.Query<LoginData>(@"
WITH RankedRows AS (
    SELECT
        LoginID,
        Service,
        Username,
        Password,
        Email,
        Category,
        Note,
        Used,
        ROW_NUMBER() OVER (ORDER BY Used DESC, Username) AS RowNum
    FROM LoginData
)
SELECT
    LoginID,
    Service,
    Username,
    Password,
    Email,
    Category,
    Note,
    Used
FROM RankedRows
ORDER BY
    CASE
        WHEN RowNum <= 3 THEN 0     -- Top 3
        ELSE RowNum                 -- all leftover ordered by RowNum
    END,
    CASE
        WHEN RowNum > 3 THEN Username  -- all leftover ordered by Service 
    END");

                return data;
            }
            catch (Exception e)
            {
                return new List<LoginData>();
            }
        }

        #endregion

        #region Settings
        public static Theme GetCurrentTheme()
        {
            //get current theme
            var result = database.Query<Theme>(@"SELECT * FROM Theme WHERE Active = true");
            Theme theme = result.FirstOrDefault();
            return theme;
        }

        public static Theme GetThemebyId(int id) 
        {
            //get theme by id
            var result = database.Query<Theme>(@"SELECT * FROM Theme WHERE ThemeID = ?", id);
                Theme theme = result.FirstOrDefault();
                return theme;
        }

        public static List<Theme> GetThemes()
        {
            //get all themes
            var result = database.Query<Theme>(@"SELECT * FROM Theme");
            return result;
        }

        public static bool SetTheme(int id)
        {
            //set theme active by id
            database.Query<Theme>("UPDATE Theme SET Active = false");
            database.Query<Theme>("UPDATE Theme SET Active = true WHERE ThemeID = ?", id);
            return true;
        }

        public static bool AddTheme()
        {
            //add new theme for testing only
            database.Query<Theme>("Delete FROM Theme");
            database.Query<Theme>(@"
INSERT INTO Theme (Name, BackgroundColor, Active)
VALUES
('Default', '#E5D0E3', true),
('IndianRed', '#CD5C5C', false),
('LightCoral', '#F08080', false),
('Salmon', '#FA8072', false),
('DarkSalmon', '#E9967A', false),
('LightSalmon', '#FFA07A', false),
('Crimson', '#DC143C', false),
('Red', '#FF0000', false),
('FireBrick', '#B22222', false),
('Pink', '#FFC0CB', false),
('LightPink', '#FFB6C1', false),
('HotPink', '#FF69B4', false),
('DeepPink', '#FF1493', false),
('MediumVioletRed', '#C71585', false),
('PaleVioletRed', '#DB7093', false),
('MediumBlue', '#0000CD', false),
('DarkBlue', '#00008B', false),
('Navy', '#000080', false),
('RoyalBlue', '#4169E1', false),
('CornflowerBlue', '#6495ED', false),
('LightSteelBlue', '#B0C4DE', false),
('LightSlateGray', '#778899', false),
('SlateGray', '#708090', false),
('DodgerBlue', '#1E90FF', false),
('SkyBlue', '#87CEEB', false),
('LightSkyBlue', '#87CEFA', false),
('SteelBlue', '#4682B4', false),
('LightBlue', '#ADD8E6', false),
('PowderBlue', '#B0E0E6', false),
('PaleTurquoise', '#AFEEEE', false),
('DarkTurquoise', '#00CED1', false),
('MediumTurquoise', '#48D1CC', false),
('Turquoise', '#40E0D0', false),
('LightCyan', '#E0FFFF', false),
('CadetBlue', '#5F9EA0', false),
('MediumAquamarine', '#66CDAA', false),
('Aquamarine', '#7FFFD4', false),
('DarkGreen', '#006400', false),
('DarkOliveGreen', '#556B2F', false),
('DarkSeaGreen', '#8FBC8F', false),
('SeaGreen', '#2E8B57', false),
('MediumSeaGreen', '#3CB371', false),
('LightSeaGreen', '#20B2AA', false),
('PaleGreen', '#98FB98', false),
('SpringGreen', '#00FF7F', false),
('LawnGreen', '#7CFC00', false),
('Green', '#00FF00', false),
('Chartreuse', '#7FFF00', false),
('MedSpringGreen', '#00FA9A', false),
('GreenYellow', '#ADFF2F', false),
('Lime', '#00FF00', false),
('YellowGreen', '#9ACD32', false),
('ForestGreen', '#228B22', false),
('OliveDrab', '#6B8E23', false),
('DarkKhaki', '#BDB76B', false),
('PaleGoldenrod', '#EEE8AA', false),
('LightGoldenrodYellow', '#FAFAD2', false),
('LightYellow', '#FFFFE0', false),
('Yellow', '#FFFF00', false),
('Gold', '#FFD700', false),
('LightGoldenrod', '#EEDD82', false),
('Goldenrod', '#DAA520', false),
('DarkGoldenrod', '#B8860B', false),
('RosyBrown', '#BC8F8F', false),
('Purple', '#CD5C5C', false),
('MediumPurple', '#9370DB', false),
('MediumSlateBlue', '#7B68EE', false),
('DarkSlateBlue', '#483D8B', false),
('LightSlateBlue', '#8470FF', false),
('Indigo', '#4B0082', false),
('DarkViolet', '#9400D3', false),
('MediumOrchid', '#BA55D3', false),
('BlueViolet', '#8A2BE2', false),
('MediumPurple', '#9370DB', false),
('Wheat', '#F5DEB3', false),
('BurlyWood', '#DEB887', false),
('Tan', '#D2B48C', false),
('RosyBrown', '#BC8F8F', false),
('SandyBrown', '#F4A460', false);


");
            return true;
        }
        #endregion

        #region FileMethods
        public static void ExportDatabase( string path)
        {
            //export database to file
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MobileOpenSesame.db");
            File.Copy(databasePath, path);
        }

        public static void ImportDatabase(string path) 
        {
            //import database from file
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MobileOpenSesame.db");
            database.Close();
            File.Delete(databasePath);
            File.Copy(path, databasePath);
            database = new SQLiteConnection(databasePath);
        }
        #endregion
    }
}
