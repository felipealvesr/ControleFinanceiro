using CommunityToolkit.Mvvm.Messaging;
using ControleFinanceiro.Models;
using ControleFinanceiro.Repositories;

namespace ControleFinanceiro.Views;

public partial class TransactionList : ContentPage
{
    private TransactionAdd _transactionAdd;
    private TransactionEdit _transactionEdit;
    private ITransactionRepository _repository;


    public TransactionList(TransactionAdd transactionAdd, TransactionEdit transactionEdit, ITransactionRepository repository)
    {
        this._transactionAdd = transactionAdd;
        this._transactionEdit = transactionEdit;
        this._repository = repository;
        InitializeComponent();
        Reload();
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            Reload();
        });
    }

    private void Reload()
    {
        var items = _repository.GetAll();
        CollectionViewTransactions.ItemsSource = items;

        double income = items.Where(x => x.Type == Models.TransactionType.Income).Sum(a => a.Value);
        double expense = items.Where(x => x.Type == Models.TransactionType.Expenses).Sum(a => a.Value);
        double balance = income - expense;
        LabelIncome.Text = income.ToString("C");
        LabelExpense.Text = expense.ToString("C");
        LabelBalance.Text = balance.ToString("C");
    }

    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs args)
    {
        var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
        Navigation.PushModalAsync(transactionAdd);
    }

    private void TapGestureRecognizerTapped_To_TransactionEdit(object sender, TappedEventArgs e)
    {
        var grid = (Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        Transaction transaction = (Transaction)gesture.CommandParameter;

        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        transactionEdit.SetTransactionForEdit(transaction);
        Navigation.PushModalAsync(transactionEdit);
    }

    private async void TapGestureRecognizerTapped_ToDelete(object sender, TappedEventArgs e)
    {
        var result = await App.Current.MainPage.DisplayAlert("Excluir!", "Tem certeza que deseja excluir?", "Sim", "Não");
        if (result)
        {
            Transaction transaction = (Transaction) e.Parameter;
            _repository.Delete(transaction);
            Reload();
        }
    
    }
}