using ControleFinanceiro.Views;

namespace ControleFinanceiro
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new TransactionList());
        }
    }
}