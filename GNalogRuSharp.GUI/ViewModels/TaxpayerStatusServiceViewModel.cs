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
    public class TaxpayerStatusServiceViewModel : INotifyPropertyChanged
    {
        private TaxpayerStatusService _client = new TaxpayerStatusService();

        private RelayCommand _getInnCommand;
        private string _inn;
        private DateTime? _requestDate = DateTime.Today;
        private bool _status;
        private string _message;

        public string Inn
        {
            get => _inn;
            set
            {
                _inn = value;
                OnPropertyChanged("Inn");
            }
        }

        public DateTime? RequestDate
        {
            get => _requestDate;
            set
            {
                _requestDate = value;
                OnPropertyChanged("RequestDate");
            }
        }

        public bool Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public RelayCommand GetTaxpayerStatusCommand
        {
            get
            {
                return _getInnCommand ??
                  (_getInnCommand = new RelayCommand(x => GetTaxpayerStatusAsync()));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private async void GetTaxpayerStatusAsync()
        {
            try
            {
                var result = await _client.GetStatusAsync(Inn, RequestDate);
                Status = result.Status;
                Message = result.Message;
            }
            catch (Exception ex)
            {
                Status = false;
                Message = ex.Message;
            }
        }
    }
}
