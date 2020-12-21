using System;
using System.Collections.Generic;
using System.Text;

namespace KanbanBoard.ViewModels
{
    public class MessageBoxViewModel : BaseViewModel
    {
        private string title;
        private string message;

        public MessageBoxViewModel(string title, string message)
        {
            this.title = title;
            this.message = message;
        }

        public Action<string> CloseDialog { get; set; }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                FirePropertyChanged();
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                FirePropertyChanged();
            }
        }

        public void Yes()
        {
            CloseDialog?.Invoke("Yes");
        }

        public void No()
        {
            CloseDialog?.Invoke("No");
        }
    }
}
