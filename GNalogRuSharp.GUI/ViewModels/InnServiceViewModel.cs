using GNalogRuSharp.GUI.Helpers;
using GNalogRuSharp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GNalogRuSharp.GUI.ViewModels
{
    public class InnServiceViewModel : INotifyPropertyChanged
    {
        private InnService _client = new InnService();

        private DocumentType _docType;
        private RelayCommand _getInnCommand;
        private string _result;
        private DateTime? _docDate;
        private string _birthPlace;
        private string _docNumberSeries;
        private DateTime? _birthDate;
        private string _patronymic;
        private string _name;
        private string _surname;

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged("Surname");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged("Patronymic");
            }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        public string DocNumberSeries
        {
            get => _docNumberSeries;
            set
            {
                _docNumberSeries = value;
                OnPropertyChanged("DocNumberSeries");
            }
        }

        public string BirthPlace
        {
            get => _birthPlace;
            set
            {
                _birthPlace = value;
                OnPropertyChanged("BirthPlace");
            }
        }

        public DateTime? DocDate
        {
            get => _docDate;
            set
            {
                _docDate = value;
                OnPropertyChanged("DocDate");
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public DocumentType DocType
        {
            get => _docType;
            set
            {
                _docType = value;
                OnPropertyChanged("DocType");
            }
        }

        public RelayCommand GetInnCommand
        {
            get
            {
                return _getInnCommand ??
                  (_getInnCommand = new RelayCommand(x => FetchInnAsync()));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private async void FetchInnAsync()
        {
            try
            {
                var result = await _client.GetInnAsync(
                    Surname,
                    Name,
                    Patronymic,
                    BirthDate,
                    DocType,
                    DocNumberSeries,
                    BirthPlace,
                    DocDate);
                Result = result.Code == 1
                  ? "ИНН: " + result.Inn
                  : "Не получилось...";
            }
            catch (Exception ex)
            {
                Result = ex.Message + '\n' + ex.StackTrace;
            }
        }
    }
}
