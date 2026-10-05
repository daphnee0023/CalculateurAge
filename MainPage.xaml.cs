namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    // sender = le contrôle cliqué ; e = données de l'évènement.
    private async void OnCalculerClicked(object sender, EventArgs e)
    {
        // Validation : on refuse un nom vide.
        if (string.IsNullOrWhiteSpace(entryNom.Text))
        {
            await DisplayAlertAsync("Erreur", "Entrez un nom", "OK");
            return; // on sort sans rien calculer
        }

        DateTime d = pickerDate.Date ?? DateTime.Today;
        int age = DateTime.Today.Year - d.Year;
        // Si l'anniversaire n'est pas encore passé cette année, on retire une année.
        if (d.Date > DateTime.Today.AddYears(-age)) age--;

        // On écrit DIRECTEMENT dans les contrôles (ce que le MVVM va supprimer).
        lblResultat.Text = $"{entryNom.Text}, vous avez {age} ans";
        lblResultat.IsVisible = true;
    }
}