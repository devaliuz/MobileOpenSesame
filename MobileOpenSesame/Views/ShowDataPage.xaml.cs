using MobileOpenSesame.Models;
using MobileOpenSesame.Service;

namespace MobileOpenSesame.Views;

[QueryProperty(nameof(LoginId), "Id")]
public partial class ShowDataPage : ContentPage
{
    #region variables
    private LoginData? _loginData;
    private bool _editMode = false;
    private bool _showPassword = false;
    #endregion

    #region Properties
    public string LoginId
    {
        //get LoginId from QueryProperty
        set
        {
            if(value != null)
            {
                //if LoginId is not null
                if(value == "")
                {
                    //if LoginId is empty, switch to edit mode
                    EditMode = true;
                    this.setFrontend();
                    return;
                }
                //get LoginData from LoginId
                _loginData = LoginDataRepository.GetLoginById(int.Parse(value));

                if (_loginData != null)
                {
                    //set frontend
                    this.txtUsername.Text = _loginData.Username;
                    this.txtPassword.Text = _loginData.Password;
                    this.txtMail.Text = _loginData.Email;
                    this.txtCategory.Text = _loginData.Category;
                    this.txtNote.Text = _loginData.Note;
                    if (_loginData.Service.Contains("."))
                    {
                        this.txtService.WidthRequest = 140;
                        this.btnService.IsVisible = true;
                    }
                    else 
                    {
                        this.txtService.WidthRequest = 230;
                        this.btnService.IsVisible = false;
                    }
                    this.txtService.Text = _loginData.Service;
                    this.setFrontend();
                        return;
                }
            }
        }
    }
    public bool EditMode 
    {
        get 
        { 
            return _editMode; 
        }
        set 
        {
            _editMode = value;
        }
    }
    public bool ShowPassword
    {
        get
        {
            return _showPassword;
        }
        set
        {
            _showPassword = value;
        }
    }
    public Theme Theme { get; set; }


    #endregion

    #region ShowDataPage
    public ShowDataPage()
	{
        InitializeComponent();
        this.Theme = Database.GetCurrentTheme();
        BackgroundColor = Color.FromArgb(this.Theme.BackgroundColor);
        MaximumWidthRequest = 600;
        MaximumHeightRequest = 800;
    }
    #endregion

    #region Button Events
    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        //("..") to parent
        Shell.Current.GoToAsync("..");
    }

    private void btnEdit_Clicked(object sender, EventArgs e)
    {
        // 2 Modes: Edit and Save for less Pages
        switch (EditMode) 
        {
            case true:
                //Validation
                string errors = "";
                if (valService.IsNotValid)
                {
                    errors += "\nBitte einen Service eingeben!";
                }
                if(valMail.IsValid)
                {
                    //if mail is not empty, check if mail is valid
                    if (valMailformat.IsNotValid)
                    {
                        errors += "\nBitte eine gültige E-Mail eingeben!";
                    }
                }

                if(valUser.IsNotValid && valMail.IsNotValid)
                {
                    //ensure that atleast email OR username is filled
                    errors += "\nBitte einen Benutzernamen oder eine E-Mail eingeben!";
                }

                if(valUser.IsNotValid && valMail.IsValid && valMailformat.IsValid) 
                {
                    //if username is empty, use mail as username
                    this.txtUsername.Text = this.txtMail.Text;
                }

                if(valPassword1.IsValid)
                {
                    //if password is not empty, check if password is valid
                    if(valPassword8.IsNotValid)
                    {
                        errors += "\nDas Passwort muss mindestens 8 Zeichen lang sein. \nEine Kombination aus Groß - und Kleinbuchstaben sowie Ziffern und Sonderzeichen wird empfohlen";
                    }
                }

                if (!string.IsNullOrEmpty(errors))
                {
                    //if errors are found, display them
                    DisplayAlert("Fehler", errors, "OK");
                    return;
                }
                else
                {
                    //if no errors are found, save data

                    int id;
                    int used;
                    if (this._loginData == null)
                    {
                        //if loginData is null, create new entry
                        id = 0;
                        used = 0;
                    }
                    else
                    {
                        id = this._loginData.LoginId;
                        used = this._loginData.Used;
                    }

                    LoginData edit = new LoginData(id, this.txtService.Text, this.txtUsername.Text, this.txtPassword.Text, this.txtMail.Text, this.txtCategory.Text, this.txtNote.Text ,used);

                    if (edit != this._loginData)
                    {
                        //if data has changed, save data
                        if (LoginDataRepository.SaveData(id, edit) == "Success")
                        {
                            this.btnEdit.Text = "Bearbeiten";
                            this.EditMode = false;
                            Shell.Current.GoToAsync("..");
                        }
          
                    }
                    else
                    {
                        //if data has not changed, go back
                        this.btnEdit.Text = "Bearbeiten";
                        this.EditMode = false;
                    }
                }
                break;

            case false:
                //switch to edit mode
                this.EditMode = true;
                this.btnEdit.Text = "Speichern";
                break;
        }
        //set frontend
        setFrontend();
    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        //ask for confirmation
        bool answer = await DisplayAlert("Löschen", $"Soll der Eintrag für '{this._loginData.Service}' wirklich gelöscht werden?", "Ja", "Nein");
        if (answer == true)
        {
            //delete data
            if (LoginDataRepository.DeleteData(this._loginData.LoginId) == "Success") Shell.Current.GoToAsync("..");
        }
        
    }

    private void btnService_Clicked(object sender, EventArgs e)
    {
        //open service in browser
        Launcher.OpenAsync(new Uri("https://" + this.txtService.Text));
    }

    private void btnUsername_Clicked(object sender, EventArgs e)
    {
        //copy username to clipboard
        Clipboard.SetTextAsync(this.txtUsername.Text);
        this.btnUsername.Animate("btnUsername", new Animation((d) => this.btnUsername.BackgroundColor = Color.FromArgb("#E5D0E3"), 0, 1), 16, 500, Easing.Linear, (d, b) => this.btnUsername.BackgroundColor = Color.FromArgb("#A99ABD"), () => false);
        this.btnUsername.Text = "Kopiert!";
    }

    private void btnMail_Clicked(object sender, EventArgs e)
    {
        //copy mail to clipboard
        Clipboard.SetTextAsync(this.txtMail.Text);
        this.btnMail.Animate("btnMail", new Animation((d) => this.btnMail.BackgroundColor = Color.FromArgb("#E5D0E3"), 0, 1), 16, 500, Easing.Linear, (d, b) => this.btnMail.BackgroundColor = Color.FromArgb("#A99ABD"), () => false);
        this.btnMail.Text = "Kopiert!";
    }

    private void btnPassword_Clicked(object sender, EventArgs e)
    {
        //copy password to clipboard
        Clipboard.SetTextAsync(this.txtPassword.Text);
        this.btnPassword.Animate("btnPassword", new Animation((d) => this.btnPassword.BackgroundColor = Color.FromArgb("#E5D0E3"), 0, 1), 16, 500, Easing.Linear, (d, b) => this.btnPassword.BackgroundColor = Color.FromArgb("#A99ABD"), () => false);
        this.btnPassword.Text = "Kopiert!";
    }

    private void btnCreate_Clicked(object sender, EventArgs e)
    {
        //generate random password with preset values
        int lenght = 12;
        string pw = "";                                                                                                             
        Random rd = new Random();                                                                                                   
        while (pw.Length < lenght)                                                                                           //repeats while Password is shorter than wanted value
        {
            int rdascii = rd.Next(33, 123);                                                                                          //gives random number between 33 and 123
            if (rdascii >= 33 && rdascii != 34 && rdascii <= 38 || rdascii >= 47 && rdascii <= 57 || rdascii >= 60 && rdascii <= 90 || rdascii >= 97)    //checks if rnd number could be converted to useable char
            {
                char c = (char)rdascii;                                                                                             //converts rdascii into a readable char
                pw += c;                                                                                                           
            }
        }
        txtPassword.Text = pw;
    }

    private void btnShow_Clicked(object sender, EventArgs e)
    {
        //show or hide password
        this.txtPassword.IsPassword = this.ShowPassword;

        if (this.ShowPassword == true)
        {
            this.btnShow.Text = "Anzeigen";
            this.ShowPassword = false;
        }
        else
        {
            this.btnShow.Text = "Verbergen";
            this.ShowPassword = true;

        }
    }
    #endregion

    #region Methods
    private void setFrontend()
    {
        //set frontend
        this.btnCreate.IsVisible = this.EditMode;
        this.btnDelete.IsVisible = !this.EditMode;
        this.txtService.IsEnabled = EditMode;
        this.txtUsername.IsEnabled = EditMode;
        this.txtMail.IsEnabled = EditMode;
        this.txtPassword.IsEnabled = EditMode;
        this.txtPassword.IsPassword = true;
        this.txtCategory.IsEnabled = EditMode;
        this.txtNote.IsEnabled = EditMode;
        
        if (EditMode == true)
        {
            this.btnEdit.Text = "Speichern";
        }
        else
        {
            this.btnEdit.Text = "Bearbeiten";
        }
    }
    #endregion
}