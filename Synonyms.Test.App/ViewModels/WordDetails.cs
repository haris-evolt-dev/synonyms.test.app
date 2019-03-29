using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synonyms.Test.App.ViewModels
{
    /// <summary>
    /// Details for word that is shown on view
    /// </summary>
    public class WordDetails: ViewModelBase
    {
        #region === Fields ===

        string _name;
        string _definition;

        #endregion

        #region === Properties ===

        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Definition
        {
            get => _definition;
            set
            {
                if (value != _definition)
                {
                    _definition = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Synonyms { get; set; } = new ObservableCollection<string>();

        #endregion
    }
}
