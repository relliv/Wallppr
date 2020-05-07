using System.Windows;
using System.Windows.Input;

namespace Wallppr.ViewModel.App
{
    public class DeleteDialogViewModel : WindowViewModel
    {
        public DeleteDialogViewModel(Window window, string title, string itemTitle) : base(window)
        {
            WindowMinimumHeight = 250;
            WindowMinimumWidth = 350;

            Title = title;
            ItemTitle = itemTitle;

            CloseCommand = new RelayCommand(p => CloseWindow());
            DeleteCommand = new RelayCommand((p) => Delete());
        }

        public string ItemTitle { get; set; }

        public bool Result { get; set; }

        private void CloseWindow()
        {
            mWindow.Close();
        }

        public ICommand DeleteCommand { get; set; }

        public void Delete()
        {
            Result = true;
            CloseWindow();
        }
    }
}