using MobileOpenSesame.Models;
using MobileOpenSesame.Service;
using System.Collections.ObjectModel;

namespace MobileOpenSesame.Views;

public partial class LoginDataPage : ContentPage
{
    public Theme Theme { get; set; }

    #region Loadingpage
    public LoginDataPage()
	{
		InitializeComponent();
        Database.Init();
        this.Theme = Database.GetCurrentTheme();
        BackgroundColor = Color.FromArgb(this.Theme.BackgroundColor);
        MaximumWidthRequest = 600;
        MaximumHeightRequest = 800;
    }

    protected override void OnAppearing()
    {
        //reload data   
        base.OnAppearing();
        LoadData();
    }

    #endregion

    #region PageEvents
    private async void DataList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {   
        if(this.DataList.SelectedItem != null)
        {
            LoginData selected = (LoginData)e.SelectedItem;
            //increase used counter
            selected.Used++;
            LoginDataRepository.SaveData(selected.LoginId, selected);
            //go to showdata page with selected item
            await Shell.Current.GoToAsync($"{nameof(ShowDataPage)}?Id={selected.LoginId}");
        }
        return;
    }

    private void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        //deselect item
        this.DataList.SelectedItem = null;
    }

    private async void btnNew_Clicked(object sender, EventArgs e)
    {
        //go to showdata page with null
        await Shell.Current.GoToAsync($"{nameof(ShowDataPage)}?Id={null}");
    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        //search for text in LoginData, show results
        var results = new ObservableCollection<LoginData>(LoginDataRepository.Search(this.searchbar.Text));
        this.DataList.ItemsSource = results;
    }

    private async void btnSettings_Clicked(object sender, EventArgs e)
    {
        //go to settings page
        await Shell.Current.GoToAsync($"{nameof(SettingsPage)}");
    }

    #endregion

    #region Methods
    private void LoadData()
    {
        var logindata = new ObservableCollection<LoginData>(LoginDataRepository.GetLoginData());
        this.DataList.ItemsSource = logindata;
    }
    #endregion

}