using ControleFinanceiro.Views;

namespace ControleFinanceiro
{
    public partial class App : Application
    {
        public App(TransactionList listPage)
        {
            InitializeComponent();

            MainPage = new NavigationPage(listPage);
        }
    }
}