using GNalogRuSharp.GUI.Helpers;
using GNalogRuSharp.Services;
using System;

namespace GNalogRuSharp.GUI.ViewModels
{
    public class TaxpayerStatusServiceViewModel : ViewModelBase
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
