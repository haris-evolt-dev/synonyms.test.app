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
    public class WordDetails : ViewModelBase
    {
        #region === Fields ===

        string _name;
        string _definition;
        string _error;

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
                    OnPropertyChanged("HasNoError");
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

        public string Error
        {
            get => _error;
            set
            {
                if (_error != value)
                {
                    _error = value;
                    OnPropertyChanged();
                    OnPropertyChanged("HasNoError");
                }
            }
        }

        public bool HasNoError
        {
            get => !string.IsNullOrEmpty(_name) && string.IsNullOrEmpty(_error);
        }

        public bool HasNoSynonyms
        {
            get => Synonyms.Count == 0;
        }

        public bool HasSynonyms
        {
            get => Synonyms.Count != 0;
        }

        public ObservableCollection<string> Synonyms { get; set; } = new ObservableCollection<string>();

        #endregion
    }
}
