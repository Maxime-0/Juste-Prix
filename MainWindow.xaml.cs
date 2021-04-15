using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JustePrixWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int Random;
        int NombreEssais;
        int Tentative;
        DispatcherTimer _timer;
        TimeSpan _time;

        public static object This { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Menu();
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            int réponse = Antibug();

            if (réponse > 0)
            {

                if (réponse != Random)
                {
                    réponse = TryAgain(réponse);

                }
                else
                {
                    TuAsGagné();
                }
            }

        }


        void btnNouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            NouvellePartie();
        }


        void TuAsGagné()
        {
            textBlockInfo.Text = "Bravo tu as trouvé le chiffre";
        }

        int TryAgain(int réponse)
        {
            /// Partie qui gére le déroulement du jeu solo ///
            if (réponse < Random)
            {
                textBlockInfo.Text = "C'est plus grand que " + réponse;
            }
            else
            {
                textBlockInfo.Text = "C'est plus bas que " + réponse;
            }
            Tentative = Tentative + 1;
            NombreEssais = NombreEssais + 1;
            UpdateTry();
            Tentatives();

            return réponse;

        }

        int Antibug()
        {
            /// Permet de mettre un message d'erreur si autre chose qu'un chiffre est rentré dans le jeu solo ///
            string chiffre = textBoxEssai.Text;
            int réponse;
            if (int.TryParse(chiffre, out réponse) == false)
            {
                textBlockInfo.Text = "Vous n'avez pas saisit un nombre réessayé";
            }
            else
            {
                textBlockInfo.Text = string.Empty;
            }

            return réponse;
        }

        void NouvellePartie()
        {
            /// Bloque le jeu si le timer arrive à 0 ///
            if (_time > TimeSpan.Zero)
            {
                _timer.Stop();
            }
            /// Partie qui remet tout les éléments du jeu solo à 0 ///
            textEnonce.Text = " Un nombre entre 1 et 100 a été généré essaye de le trouver !";
            textBoxEssai.IsEnabled = true;
            Random = NombreAleatoire();
            textBlockInfo.Text = string.Empty;
            textBoxEssai.Text = string.Empty;
            NombreEssais = 0;
            Tentative = 0;
            btnNiveauSuivant.Visibility = Visibility.Visible;

            UpdateTry();
            Tentatives();
            Timer();
        }

        void UpdateTry()
        {
            /// Mise en place du nombre d'essai ///
            textBlockNbEssai.Text = "Nombre d'essais : " + NombreEssais;
        }

        int NombreAleatoire()
        {
            /// Génération du nombre entre 1 et 100 ///
            return new Random().Next(1, 101);
        }

        void Tentatives()
        {
            /// Mise en place du nombre de tentative + de la règle des 5 tentatives ///
            textTentative.Text = "Tentative : " + Tentative;
            if (Tentative == 5)
            {
                textBlockInfo.Text = "C'est perdu";
                textBoxEssai.IsEnabled = false;
            }
        }
        void Timer()
        {
            /// Mise en place du Compte à rebours ///
            _time = TimeSpan.FromSeconds(60);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                tbTime.Text = _time.ToString("c");
                if (_time == TimeSpan.Zero) _timer.Stop();

                _time = _time.Add(TimeSpan.FromSeconds(-1));
                if (_time == TimeSpan.Zero)
                {
                    textBlockInfo.Text = "C'est trop tard";
                    textBoxEssai.IsEnabled = false;
                }
            }, Application.Current.Dispatcher);

            _timer.Start();
        }

        private void btnNiveauSuivant_Click(object sender, RoutedEventArgs e)
        {
            Menu();
            textBlockNbEssai.Text = string.Empty;
            textBlockInfo.Text = string.Empty;
            textBoxEssai.Text = string.Empty;
            tbTime.Text = string.Empty;
            textEnonce.Text = string.Empty;
            textTentative.Text = string.Empty;
            Joueur1.Text = string.Empty;
            Joueur2.Text = string.Empty;
            _timer.Stop();

        }

        private void btnSolo_Click(object sender, RoutedEventArgs e)
        {
            PageSolo();
            NouvellePartie();
        }

        private void btnMultiJoueur_Click(object sender, RoutedEventArgs e)
        {
            PageMulti();
            textJoueur1.Text = "Joueur 1";
            textJoueur2.Text = "Joueur 2";
            textNomJoueur.Text = "Saisissez vos noms";

            textBoxJoueur1.Text = string.Empty;
            textBoxJoueur2.Text = string.Empty;

        }

        void NomJoueurs()
        {
            /// Mise en place du nom des joueurs ///
            string NomJoueur1 = textBoxJoueur1.Text;
            string NomJoueur2 = textBoxJoueur2.Text;
            Joueur1.Text = "Bonjour " + NomJoueur1;
            Joueur2.Text = "Bonjour " + NomJoueur2;
            textJoueur1.Text = NomJoueur1;
            textJoueur2.Text = NomJoueur2;
        }

        private void btnValidez1_Click(object sender, RoutedEventArgs e)
        {
            NomJoueurs();
            btnSuivant0.Visibility = Visibility.Visible;
        }

        private void btnSuivant0_Click(object sender, RoutedEventArgs e)
        {
            Pagevelo();
            NomJoueurs();
            Joueur1.Text = string.Empty;
            Joueur2.Text = string.Empty;
            textBoxJoueur1.Text = string.Empty;
            textBoxJoueur2.Text = string.Empty;
            textNomJoueur.Text = "Essayez de deviner le prix !";

        }

        void diapo1()
        {
            /// Tout le processus qui vient comparé les deux réponses des joueurs ///
            string NomJoueur1 = textJoueur1.Text;
            string NomJoueur2 = textJoueur2.Text;
            string saisiréponse = "75.90";
            int réponsevelo;
            int.TryParse(saisiréponse, out réponsevelo);

            string saisiBox1 = textBoxJoueur1.Text;
            int réponseJoueur1;
            int.TryParse(saisiBox1, out réponseJoueur1);

            string saisiBox2 = textBoxJoueur2.Text;
            int réponseJoueur2;
            int.TryParse(saisiBox2, out réponseJoueur2);

            int réponse1 = réponsevelo - réponseJoueur1;
            int réponse2 = réponsevelo - réponseJoueur1;

            if (réponseJoueur1 == réponsevelo)
            {
                textNomJoueur.Text = NomJoueur1 + " Bravo vous avez trouvé juste !";
            }
            if (réponseJoueur2 == réponsevelo)
            {
                textNomJoueur.Text = NomJoueur2 + " Bravo vous avez trouvé juste !";
            }
            if (réponseJoueur1 > réponseJoueur2)
            {
                textNomJoueur.Text = NomJoueur1 + " est le plus proche de " + saisiréponse;
            }
            else
            {
                textNomJoueur.Text = NomJoueur2 + " est le plus proche de " + saisiréponse; 
            }
        }

        private void btnValidez2_Click(object sender, RoutedEventArgs e)
        {
            diapo1();
            btnSuivant1.Visibility = Visibility.Visible;
        }

        void diapo2()
        {
            /// Tout le processus qui vient comparé les deux réponses des joueurs ///
            string NomJoueur1 = textJoueur1.Text;
            string NomJoueur2 = textJoueur2.Text;
            string saisiréponse = "189";
            int réponsevelo;
            int.TryParse(saisiréponse, out réponsevelo);

            string saisiBox1 = textBoxJoueur1.Text;
            int réponseJoueur1;
            int.TryParse(saisiBox1, out réponseJoueur1);

            string saisiBox2 = textBoxJoueur2.Text;
            int réponseJoueur2;
            int.TryParse(saisiBox2, out réponseJoueur2);

            int réponse1 = réponsevelo - réponseJoueur1;
            int réponse2 = réponsevelo - réponseJoueur1;

            if (réponseJoueur1 == réponsevelo)
            {
                textNomJoueur.Text = NomJoueur1 + " Bravo vous avez trouvé juste !";
            }
            if (réponseJoueur2 == réponsevelo)
            {
                textNomJoueur.Text = NomJoueur2 + " Bravo vous avez trouvé juste !";
            }
            if (réponseJoueur1 > réponseJoueur2)
            {
                textNomJoueur.Text = NomJoueur1 + " est le plus proche de " + saisiréponse;
            }
            else
            {
                textNomJoueur.Text = NomJoueur2 + " est le plus proche de " + saisiréponse;
            }
        }

        private void btnValidez3_Click(object sender, RoutedEventArgs e)
        {
            diapo2();
        }

        private void btnSuivant1_Click(object sender, RoutedEventArgs e)
        {
            Pagemontre();
            Joueur1.Text = string.Empty;
            Joueur2.Text = string.Empty;
            textBoxJoueur1.Text = string.Empty;
            textBoxJoueur2.Text = string.Empty;
            textNomJoueur.Text = "Essayez de deviner le prix !";
        }
        void PageSolo()
        {
            /// Tout les éléments visible dans la page Solo ///
            tbTime.Visibility = Visibility.Visible;
            textEnonce.Visibility = Visibility.Visible;
            textTentative.Visibility = Visibility.Visible;
            textBlockNbEssai.Visibility = Visibility.Visible;
            btnValider.Visibility = Visibility.Visible;
            btnNouvellePartie.Visibility = Visibility.Visible;
            textBlockInfo.Visibility = Visibility.Visible;
            textBoxEssai.Visibility = Visibility.Visible;
            btnNiveauSuivant.Visibility = Visibility.Visible;

            btnSolo.Visibility = Visibility.Collapsed;
            btnMultiJoueur.Visibility = Visibility.Collapsed;
            textBienvenue.Visibility = Visibility.Collapsed;
            textJoueur1.Visibility = Visibility.Collapsed;
            textJoueur2.Visibility = Visibility.Collapsed;
            textBoxJoueur1.Visibility = Visibility.Collapsed;
            textBoxJoueur2.Visibility = Visibility.Collapsed;
            Joueur1.Visibility = Visibility.Collapsed;
            Joueur2.Visibility = Visibility.Collapsed;
            btnValidez1.Visibility = Visibility.Collapsed;
            btnValidez2.Visibility = Visibility.Collapsed;
            btnValidez3.Visibility = Visibility.Collapsed;
            btnSuivant0.Visibility = Visibility.Collapsed;
            btnSuivant1.Visibility = Visibility.Collapsed;
            Ivelofille.Visibility = Visibility.Collapsed;
            iMontreHomme.Visibility = Visibility.Collapsed;

            textBlockNbEssai.Text = string.Empty;
            textBlockInfo.Text = string.Empty;
            textBoxEssai.Text = string.Empty;
            tbTime.Text = string.Empty;
            textEnonce.Text = string.Empty;
            textTentative.Text = string.Empty;
        }
        void Menu()
        {
            /// Tous les éléments présent au démarrage de l'application ///
            btnMultiJoueur.Visibility = Visibility.Visible;
            btnSolo.Visibility = Visibility.Visible;
            textBienvenue.Visibility = Visibility.Visible;

            textNomJoueur.Visibility = Visibility.Collapsed;
            textJoueur1.Visibility = Visibility.Collapsed;
            textJoueur2.Visibility = Visibility.Collapsed;
            textBoxJoueur1.Visibility = Visibility.Collapsed;
            textBoxJoueur2.Visibility = Visibility.Collapsed;
            btnValidez1.Visibility = Visibility.Collapsed;
            btnNiveauSuivant.Visibility = Visibility.Collapsed;
            btnValider.Visibility = Visibility.Collapsed;
            btnNouvellePartie.Visibility = Visibility.Collapsed;
            textBoxEssai.Visibility = Visibility.Collapsed;
            btnSuivant0.Visibility = Visibility.Collapsed;
            btnSuivant1.Visibility = Visibility.Collapsed;
            Ivelofille.Visibility = Visibility.Collapsed;
            iMontreHomme.Visibility = Visibility.Collapsed;
            btnValidez2.Visibility = Visibility.Collapsed;
            btnValidez3.Visibility = Visibility.Collapsed;
            tbTime.Visibility = Visibility.Collapsed;

            textBlockNbEssai.Text = string.Empty;
            textBlockInfo.Text = string.Empty;
            textBoxEssai.Text = string.Empty;
            tbTime.Text = string.Empty;
            textEnonce.Text = string.Empty;
            textTentative.Text = string.Empty;
            Timer();
            _timer.Stop();
        }
        void PageMulti()
        {
            /// Tous les éléments présent au moment de la saisit des noms ///
            btnNiveauSuivant.Visibility = Visibility.Visible;
            textJoueur1.Visibility = Visibility.Visible;
            textJoueur2.Visibility = Visibility.Visible;
            textBoxJoueur1.Visibility = Visibility.Visible;
            textBoxJoueur2.Visibility = Visibility.Visible;
            textNomJoueur.Visibility = Visibility.Visible;
            btnValidez1.Visibility = Visibility.Visible;
            Joueur1.Visibility = Visibility.Visible;
            Joueur2.Visibility = Visibility.Visible;

            tbTime.Visibility = Visibility.Collapsed;
            textEnonce.Visibility = Visibility.Collapsed;
            textTentative.Visibility = Visibility.Collapsed;
            textBlockNbEssai.Visibility = Visibility.Collapsed;
            btnValider.Visibility = Visibility.Collapsed;
            btnNouvellePartie.Visibility = Visibility.Collapsed;
            textBlockInfo.Visibility = Visibility.Collapsed;
            textBoxEssai.Visibility = Visibility.Collapsed;
            btnMultiJoueur.Visibility = Visibility.Collapsed;
            btnSolo.Visibility = Visibility.Collapsed;
            textBienvenue.Visibility = Visibility.Collapsed;
            Ivelofille.Visibility = Visibility.Collapsed;
            iMontreHomme.Visibility = Visibility.Collapsed;
        }
        void Pagevelo()
        {
            /// Tous les éléments présent sur page vélo ///
            btnNiveauSuivant.Visibility = Visibility.Visible;
            btnValidez2.Visibility = Visibility.Visible;
            textJoueur1.Visibility = Visibility.Visible;
            textJoueur2.Visibility = Visibility.Visible;
            textBoxJoueur1.Visibility = Visibility.Visible;
            textBoxJoueur2.Visibility = Visibility.Visible;
            Joueur1.Visibility = Visibility.Visible;
            Joueur2.Visibility = Visibility.Visible;
            Ivelofille.Visibility = Visibility.Visible;

            tbTime.Visibility = Visibility.Collapsed;
            textEnonce.Visibility = Visibility.Collapsed;
            textTentative.Visibility = Visibility.Collapsed;
            textBlockNbEssai.Visibility = Visibility.Collapsed;
            btnValider.Visibility = Visibility.Collapsed;
            btnNouvellePartie.Visibility = Visibility.Collapsed;
            textBlockInfo.Visibility = Visibility.Collapsed;
            textBoxEssai.Visibility = Visibility.Collapsed;
            btnMultiJoueur.Visibility = Visibility.Collapsed;
            btnSolo.Visibility = Visibility.Collapsed;
            textBienvenue.Visibility = Visibility.Collapsed;
            iMontreHomme.Visibility = Visibility.Collapsed;
            btnSuivant1.Visibility = Visibility.Collapsed;
            btnSuivant0.Visibility = Visibility.Collapsed;
        }
        void Pagemontre()
        {
            /// Tous les éléments présent sur la page Montre ///
            btnNiveauSuivant.Visibility = Visibility.Visible;
            btnValidez3.Visibility = Visibility.Visible;
            textJoueur1.Visibility = Visibility.Visible;
            textJoueur2.Visibility = Visibility.Visible;
            textBoxJoueur1.Visibility = Visibility.Visible;
            textBoxJoueur2.Visibility = Visibility.Visible;
            Joueur1.Visibility = Visibility.Visible;
            Joueur2.Visibility = Visibility.Visible;
            iMontreHomme.Visibility = Visibility.Visible;

            tbTime.Visibility = Visibility.Collapsed;
            textEnonce.Visibility = Visibility.Collapsed;
            textTentative.Visibility = Visibility.Collapsed;
            textBlockNbEssai.Visibility = Visibility.Collapsed;
            btnValider.Visibility = Visibility.Collapsed;
            btnNouvellePartie.Visibility = Visibility.Collapsed;
            textBlockInfo.Visibility = Visibility.Collapsed;
            textBoxEssai.Visibility = Visibility.Collapsed;
            btnMultiJoueur.Visibility = Visibility.Collapsed;
            btnSolo.Visibility = Visibility.Collapsed;
            textBienvenue.Visibility = Visibility.Collapsed;
            Ivelofille.Visibility = Visibility.Collapsed;
            btnSuivant1.Visibility = Visibility.Collapsed;
            btnSuivant0.Visibility = Visibility.Collapsed;
        }
    }
}