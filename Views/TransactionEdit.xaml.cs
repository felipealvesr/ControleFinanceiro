using CommunityToolkit.Mvvm.Messaging;
using ControleFinanceiro.Models;
using ControleFinanceiro.Repositories;
using System.Text;

namespace ControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{
    private Transaction _transaction;
    private ITransactionRepository _repository;
    public TransactionEdit(ITransactionRepository repository)
    {
        InitializeComponent();
        _repository = repository;
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
    private void TapGestureRecognizer_Tapped_ToClose(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }
    private void OnButtonClicked_Save(object sender, EventArgs e)
    {

        if (!IsValid())
        {
            return;
        }

        SaveTransactionInDatabase();
        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);

    }

    private bool IsValid()
    {
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O campo 'Nome' deve ser preenchido.");
            valid = false;
        }
        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            valid = false;
            sb.AppendLine("O campo 'Valor' deve ser preenchido.");
        }
        double result;
        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
        {
            valid = false;
            sb.AppendLine("O campo 'Valor' é inválido.");
        }

        if (!valid)
        {
            LabelError.IsVisible = true;
            LabelError.Text = sb.ToString();
        }

        return valid;
    }

    private void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Id = _transaction.Id,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = double.Parse(EntryValue.Text),
        };

        _repository.Update(transaction);
    }
}