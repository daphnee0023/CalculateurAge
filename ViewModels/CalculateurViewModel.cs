using CalculateurAge.Views;

namespace CalculateurAge.ViewModels;


public class CalculateurViewModel : BaseViewModel
{
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private bool _resultatVisible;
    private string _message = "";           // Fonctionnalité 1 : Majeur / Mineur
    private string _joursAnniversaire = "";  // Fonctionnalité 3 : jours avant l'anniversaire
    private string _erreur = "";            // Fonctionnalité 4 : date future refusée
    private int _age;

    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value)) RafraichirCommandes(); }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set
        {
            if (SetField(ref _dateNaissance, value))
            {
                // Fonctionnalité 4 : une date future est refusée
                Erreur = value.Date > DateTime.Today
                    ? "La date de naissance ne peut pas être dans le futur."
                    : "";
                RafraichirCommandes();
            }
        }
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    public string JoursAnniversaire
    {
        get => _joursAnniversaire;
        set => SetField(ref _joursAnniversaire, value);
    }

    public string Erreur
    {
        get => _erreur;
        set => SetField(ref _erreur, value);
    }

    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }       // Fonctionnalité 2
    public RelayCommand VoirDetailCommand { get; }    

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom) && DateNaissance.Date <= DateTime.Today);

        EffacerCommand = new RelayCommand(Effacer);

        VoirDetailCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync(
                $"{nameof(ResultatPage)}?nom={Uri.EscapeDataString(Nom)}&age={_age}"),
            () => ResultatVisible);
    }

    private void RafraichirCommandes()
    {
        CalculerCommand.Rafraichir();
        VoirDetailCommand.Rafraichir();
    }

        private void Calculer()
    {
        DateTime aujourdhui = DateTime.Today;

        int age = aujourdhui.Year - DateNaissance.Year;
        if (DateNaissance.Date > aujourdhui.AddYears(-age)) age--;
        _age = age;

        Resultat = $"{Nom}, vous avez {age} ans";
        Message = age >= 18 ? "Majeur" : "Mineur";

        
        DateTime prochain = DateNaissance.Date.AddYears(age + 1);
        bool anniversaireAujourdhui = DateNaissance.Month == aujourdhui.Month
                                   && DateNaissance.Day == aujourdhui.Day;
        JoursAnniversaire = anniversaireAujourdhui
            ? "Bon anniversaire !"
            : $"Prochain anniversaire dans {(prochain - aujourdhui).Days} jour(s)";

        ResultatVisible = true;
        VoirDetailCommand.Rafraichir();
    }

    // Fonctionnalité 2 : remet tous les champs à zéro.
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        Message = "";
        JoursAnniversaire = "";
        Erreur = "";
        ResultatVisible = false;
        RafraichirCommandes();
    }
}