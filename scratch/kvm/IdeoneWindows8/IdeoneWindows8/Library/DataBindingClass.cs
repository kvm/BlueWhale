using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
using IdeoneWindows8.Common;
namespace IdeoneWindows8.Library
{
    public class DataBindingClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isIndeterminate;
        public bool isIndeterminate
        {
            get
            {
                return _isIndeterminate;
            }
            set
            {
                _isIndeterminate = value;
                if(PropertyChanged != null)
                    PropertyChanged(this,new PropertyChangedEventArgs("isIndeterminate"));
            }
        }
    }
}
