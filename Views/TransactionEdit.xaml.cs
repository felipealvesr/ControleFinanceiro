using ControleFinanceiro.Models;

namespace ControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{
    private Transaction _transaction;
    public TransactionEdit()
    {
        InitializeComponent();
    }

    public void SetTransactionForEdit(Transaction transaction)
    {
        _transaction = transaction;

        if (transaction.Type == TransactionType.Income)

            RadioIncome.IsChecked = true;
        else RadioExpense.IsChecked = false;

        EntryName.Text = transaction.Name;
        DatePickerDate.Date = transaction.Date.Date;
        EntryValue.Text = transaction.Value.ToString();
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }
}