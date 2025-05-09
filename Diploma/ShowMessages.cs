using System.Windows;

namespace Diploma
{
    public class ShowMessages
    {
        public void ShowError(Exception ex)
        {
            MessageBox.Show
            (
                $"{ex.Message}",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        public void ShowErrorMessage(string str)
        {
            MessageBox.Show
            (
                $"{str}",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        //public void ShowMessage(string str)
        //{
        //    MessageBox.Show
        //    (
        //        $"{str}",
        //        "Уведомление",
        //        MessageBoxButton.OK,
        //        MessageBoxImage.Error
        //    );
        //}

        //public void ShowGoodMessage(string str)
        //{
        //    MessageBox.Show
        //    (
        //        $"{str}",
        //        "Уведомление",
        //        MessageBoxButton.OK,
        //        MessageBoxImage.Information
        //    );
        //}

        public MessageBoxResult ShowQuestion(string question) =>
            MessageBox.Show(
                $"{question}",
                "Уведомление",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
    }
}
