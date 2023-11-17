using CommunityToolkit.Maui.Storage;
using MobileOpenSesame.Models;
using MobileOpenSesame.Service;

namespace MobileOpenSesame.Views;

public partial class SettingsPage : ContentPage
{
    public Theme Theme { get; set; }

    #region Loadingpage
    public SettingsPage()
	{
		InitializeComponent();
        this.Theme = Database.GetCurrentTheme();
        BackgroundColor = Color.FromArgb(this.Theme.BackgroundColor);
        MaximumWidthRequest = 600;
        MaximumHeightRequest = 800;

        //triggers Exception TODO DEBUG
        /*var themes = Database.GetThemes();
        foreach (var t in themes)
        {
            this.pickTheme.Items.Add(t.Name);
        }
        this.pickTheme.SelectedIndex = this.Theme.ThemeID;*/
    }
    #endregion

    #region PageEvents
    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        //go back
        Shell.Current.GoToAsync("..");
    }

    private async void btnDelDB_Clicked(object sender, EventArgs e)
    {
        //delete database and close app to prevent errors and force rebuild of database
        bool answer1 = await DisplayAlert("Löschen", $"Soll die komplette Datenbank wirklich gelöscht werden?", "Ja", "Nein");
        if (answer1 == true)
        {
            bool answer2 = await DisplayAlert("Löschen", $"Dieser Prozess lässt sich nicht rückgängig machen. Möchten Sie Fortfahren?", "Ja", "Nein");
            if (answer2 == true)
            {
                await DisplayAlert("Löschen", $"Datenbank wird gelöscht. Die App wird anschließend geschlossen. ", "OK");

                if (LoginDataRepository.DeleteAllData() == "Success")
                {
                    Application.Current.Quit();
                }
            }
        }
    }

    private async void btnImportDB_Clicked(object sender, EventArgs e)
    {
        //import database from file
        var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
        {
            // Add the platform specific MIME types.
            { DevicePlatform.iOS, new[] { "public.database" } },
            { DevicePlatform.Android, new[] { "application/db" } },
            { DevicePlatform.WinUI, new[] { ".db" } },

        });
        var result = await FilePicker.PickAsync(new PickOptions()
        {
            FileTypes = customFileType,
            PickerTitle = "Wählen Sie eine .db-Datei aus!"
        }) ;

        // Process picked file
        if(result == null) return;
        string path = result.FullPath;
        Database.ImportDatabase(path);
    }

    private async void btnExportDB_Clicked(object sender, EventArgs e)
    {
        //export database to file
        try
        {
            
            var folder = await FolderPicker.PickAsync(default);
            if (folder != null)
            {
                // Save the file
                string path = folder.Folder.Path+$"/MobileOpenSesame.db";
                Database.ExportDatabase(path);
            }
           
        }
        catch(Exception ex)
        {
            // Catch any errors that may occur and display them
            await DisplayAlert("Fehler", ex.Message, "OK");
        }
    }
    /*
    private void pickTheme_SelectedIndexChanged(object sender, EventArgs e)
    {
        //change theme -> leads to Exceptions TODO DEBUG
        int themeid = this.pickTheme.SelectedIndex;
        this.Theme = Database.GetThemebyId(themeid);
        BackgroundColor = Color.FromArgb(this.Theme.BackgroundColor);

    }

    private void btnSaveTheme_Clicked(object sender, EventArgs e)
    {
        Database.SetTheme(this.Theme.ThemeID);
    }*/
    #endregion
}