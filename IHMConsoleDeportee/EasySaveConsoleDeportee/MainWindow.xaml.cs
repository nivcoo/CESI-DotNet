using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySaveConsoleDeportee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
 
    public partial class MainWindow : Window
    {
        Client Client = new Client();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Connexion(object sender, RoutedEventArgs e)
        {
            Client.SeConnecter();
            string response = Client.EcouterReseau("");
            sauvegarde_en_selectionnee.Content = response;
        }
        private void Deconnexion(object sender, RoutedEventArgs e)
        {
            Client.Deconnecter();
            sauvegarde_en_selectionnee.Content = "deconnexion";
        }

        private void Lancer(object sender, RoutedEventArgs e)
        {
            string response = Client.RequeteLancer();
            sauvegarde_en_cours.Content = response;
        }

        private void Stopper(object sender, RoutedEventArgs e)
        {
            string response = Client.RequeteStopper();
            sauvegarde_en_cours.Content = response;
        }

        private void Reprendre(object sender, RoutedEventArgs e)
        {
            string response = Client.RequeteReprendre();
            sauvegarde_en_cours.Content = response;
        }

        private void Arreter(object sender, RoutedEventArgs e)
        {
            string response = Client.RequeteArreter();
            sauvegarde_en_cours.Content = response;
        }

    }
}
