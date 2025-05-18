using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClientShop.Model
{
    public class CustomCommand : ICommand
    {

        public Action _Execute;

        public CustomCommand(Action ExecuteMethod)
        {
            _Execute = ExecuteMethod;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _Execute();
        }

    }

    public class CustomCommand<T> : ICommand
    {

        public Action<T> _Execute;

        public CustomCommand(Action<T> ExecuteMethod)
        {
            _Execute = ExecuteMethod;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _Execute((T)parameter);
        }

    }
}
