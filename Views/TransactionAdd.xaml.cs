namespace ControleFinanceiro.Views;
using ControleFinanceiro.Models;
using ControleFinanceiro.Repositories;
using System.Text;
using CommunityToolkit.Mvvm.Messaging;

public partial class TransactionAdd : ContentPage
{

	private ITransactionRepository _repository;

	public TransactionAdd(ITransactionRepository repository)
	{
		this._repository = repository;
		InitializeComponent();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		Navigation.PopModalAsync();
    }

	private void OnButtonClicked_Save(object sender,  EventArgs e)
	{

		if (!IsValid())
		{
			return;
		}

		SaveTransactionInDatabase();
		Navigation.PopModalAsync();
		WeakReferenceMessenger.Default.Send<string>(string.Empty);

        var count = _repository.GetAll().Count();
		App.Current.MainPage.DisplayAlert("Mensagem!", $"Existem {count} registro(s) no banco.", "Ok");
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
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
			Date = DatePickerDate.Date,
			Value = double.Parse(EntryValue.Text),
        };

        _repository.Add(transaction);
    }

}