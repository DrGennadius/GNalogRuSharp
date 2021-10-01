using GNalogRuSharp.GUI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GNalogRuSharp.GUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
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
                  (_getInnCommand = new RelayCommand(obj =>
                  {
                      NalogRu client = new NalogRu();
                      client.SetData(
                          Surname,
                          Name,
                          Patronymic,
                          BirthDate,
                          DocType,
                          DocNumberSeries,
                          BirthPlace,
                          DocDate);
                      bool isSucces = client.FetchINN();
                      Result = isSucces
                        ? "ИНН: " + client.FNSInfo.INN
                        : $"Не получилось...\n{client.ErrorString}\n\nПроверьте правильность данных.";
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
