using GalaSoft.MvvmLight;
using Wallppr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wallppr.ViewModel.Example
{
    public class ExampleViewModel : ViewModelBase
    {
        public ExampleViewModel()
        {
            GoToPageCommand = new RelayParameterizedCommand(GoToPage);

        }

        #region Commands

        public ICommand GoToPageCommand { get; set; }


        #endregion

        #region Public Properties



        #endregion

        #region Pagination

        public Pagination Pagination { get; set; }
        public int PageLimit { get; set; } = 24;
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }

        #endregion


        #region Pagination



        #endregion

        #region Methods

        public void LoadItems()
        {

        }

        /// <summary>
        /// Go to seleceted page
        /// </summary>
        /// <param name="sender"></param>
        public void GoToPage(object sender)
        {
            var page = (Models.Common.Page)sender;

            CurrentPage = page.PageNumber;
            LoadItems();
        }

        #endregion
    }
}