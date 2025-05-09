using Diploma.Database.Models;
using Diploma.Database;
using MiniSoftware;
using System.IO;
using System.Diagnostics;

namespace Diploma.DataManipulation.InvoicesVM
{
    public partial class OpenWord
    {
        Invoices _invoice = new();
        int _invoiceId;
        ShowMessages showMessages = new();

        string inputFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\Templates\InvoiceTemplate_INPUT.docx";
        string outputFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\Templates\InvoiceTemplate_OUTPUT.docx";


        public OpenWord(int invoiceId)
        {
            _invoiceId = invoiceId;
        }

        public void FillDataAndGenerateDocx()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    _invoice = context.Invoices.FirstOrDefault(i => i.Id == _invoiceId);
                    if (_invoice == null)
                    {
                        showMessages.ShowErrorMessage("Счет не найден.");
                        return;
                    }

                    int price = _invoice.Quantity != 0 ? _invoice.TotalPrice / _invoice.Quantity : 0;
                    int tax = _invoice.TotalPrice / 10;
                    int priceWithTax = _invoice.TotalPrice + tax;

                    var dataReplacement = new Dictionary<string, object>
                    {
                         { "Id", _invoice.Id.ToString() },
                         { "Date", _invoice.Date.ToString("dd.MM.yyyy") },
                         { "ManufacturerName", _invoice.ManufacturerName ?? "" },
                         { "ManufacturerAddress", _invoice.ManufacturerAddress ?? "" },
                         { "ManufacturerINN", _invoice.ManufacturerINN ?? "" },
                         { "ClientName", _invoice.ClientName ?? "" },
                         { "ClientAddress", _invoice.ClientAddress ?? "" },
                         { "ClientINN", _invoice.ClientINN ?? "" },
                         { "ProductName", _invoice.ProductName ?? "" },
                         { "Quantity", _invoice.Quantity.ToString() },
                         { "Price", price.ToString() },
                         { "PriceWithoutTax", _invoice.TotalPrice.ToString() },
                         { "Tax", tax.ToString() },
                         { "PriceWithTax", priceWithTax.ToString() },
                    };

                    // Вызываем новый метод для генерации DOCX
                    GenerateDocx(dataReplacement);
                }

            }
            catch (Exception ex)
            {
                showMessages.ShowError(ex);
            }
        }


        void GenerateDocx(Dictionary<string, object> replacements)
        {
            try
            {
                // Проверяем, существует ли шаблон
                if (!File.Exists(inputFilePath))
                {
                    throw new FileNotFoundException($"Файл шаблона не найден: {inputFilePath}");
                }

                // Используем MiniWord для сохранения заполненного шаблона в новый файл
                MiniWord.SaveAsByTemplate(outputFilePath, inputFilePath, replacements);
            }
            catch (Exception ex)
            {
                // Перехватываем и показываем ошибку, можно добавить логирование
                showMessages.ShowErrorMessage($"Ошибка при создании документа: {ex.Message}\n{ex.StackTrace}");
            }

            OpenFileInWord();
        }

        void OpenFileInWord()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = outputFilePath,
                    UseShellExecute = true // Использовать оболочку системы для открытия файла
                });
            }
            catch (Exception ex)
            {
                showMessages.ShowError(ex);
            }
        }
    }
}
