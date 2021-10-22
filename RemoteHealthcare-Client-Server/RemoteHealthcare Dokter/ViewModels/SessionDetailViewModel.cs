﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Shared.DataStructs;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class SessionDetailViewModel : INotifyPropertyChanged
    {
        private readonly int MAX_GRAPH_LENGHT = 150;

        private Window window;
        private SharedPatient Patient;
        private SessionManager manager;

        public event PropertyChangedEventHandler PropertyChanged;

        public SessionDetailViewModel(Window window, SharedPatient patient)
        {
            this.window = window;
            this.Patient = patient;

            this.manager = new SessionManager(patient);
            this.MessageList = new ObservableCollection<string>();

            this.FullName = this.Patient.FirstName + " " + this.Patient.LastName;
            this.Age = "Leeftijd:\t\t" + CalculateAge();
            this.ID = "ID persoon:\t" + this.Patient.ID;

            this.Speed = "Snelheid: -- km/h";
            this.TotalW = "Totaal: -- kW";
            this.CurrentW = "Huidig: -- Watt";
            this.Distance = "Afstand: -- m";
            this.RPM = "RPM: --";
            this.BPM = "BPM: --";

            this.manager.NewDataTriggered += SetNewPatientData;

            setupGraph();
        }

        #region Binded Attributes

        private SharedPatient _SelectedSessionPatient;
        public SharedPatient SelectedSessionPatient
        {
            get { return _SelectedSessionPatient; }
            set
            {
                _SelectedSessionPatient = value;
                HandleSessionPatientClicked();
            }
        }

        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set
            {
                _FullName = value;
            }
        }

        private string _Age;
        public string Age
        {
            get { return _Age; }
            set
            {

                _Age = value;
            }
        }

        private string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
            }
        }

        private string _RPM;
        public string RPM
        {
            get { return _RPM; }
            set
            {
                _RPM = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RPM"));
            }
        }

        private string _BPM;
        public string BPM
        {
            get { return _BPM; }
            set
            {
                _BPM = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BPM"));
            }
        }

        private string _Speed;
        public string Speed
        {
            get { return _Speed; }
            set
            {
                _Speed = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speed)));
            }
        }

        private string _Distance;
        public string Distance
        {
            get { return _Distance; }
            set
            {
                _Distance = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Distance"));
            }
        }

        private string _CurrentW;
        public string CurrentW
        {
            get { return _CurrentW; }
            set
            {
                _CurrentW = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentW"));
            }
        }

        private string _TotalW;
        public string TotalW
        {
            get { return _TotalW; }
            set
            {
                _TotalW = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalW"));
            }
        }

        private ICommand _AbortCommand;
        public ICommand AbortCommand
        {
            get
            {
                if (_AbortCommand == null)
                {
                    _AbortCommand = new GeneralCommand(
                        param => SendAbort()
                        ); ;
                }
                return _AbortCommand;
            }

        }

        private ICommand _SendMessage;
        public ICommand SendMessage
        {
            get
            {
                if (_SendMessage == null)
                {
                    _SendMessage = new GeneralCommand(
                        param => SendPersonalMessage()
                        ); ;
                }
                return _SendMessage;
            }

        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
            }
        }

        private int _ResistanceValue;
        public int ResistanceValue
        {
            get { return _ResistanceValue; }
            set
            {
                _ResistanceValue = value;
                UpdateResistance();
            }
        }

        private ICommand _CloseDetailCommand;
        public ICommand CloseDetailCommand
        {
            get
            {
                if (_CloseDetailCommand == null)
                {
                    _CloseDetailCommand = new GeneralCommand(
                        param => CloseDetail()
                        ); ; ;
                }
                return _CloseDetailCommand;
            }

        }

        private ObservableCollection<string> _MessageList;
        public ObservableCollection<string> MessageList
        {
            get { return _MessageList; }
            set
            {
                _MessageList = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessageList"));
            }
        }

        #endregion

        #region Graphs

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> BPMLabels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private double _axisMax = 150;
        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AxisMax"));
            }
        }

        private double _axisMin = 0;
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AxisMin"));
            }
        }



        private void setupGraph()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "BPM",
                    Values = new ChartValues<double> {},
                    PointGeometry = null,
                    LineSmoothness = 10,
                    Fill = new SolidColorBrush(Color.FromScRgb(0.5f, 1f, 0f, 0f)),
                    Stroke = Brushes.Red
                }
            };

            BPMLabels = new List<string>();
            YFormatter = value => value.ToString();
        }

        private void setNewPoint(IChartValues list, double value, DateTime time)
        {
            // checking if the list has reached its limit
            //if (list.Count >= MAX_GRAPH_LENGHT) list.RemoveAt(0);

            AxisMax = list.Count;

            // Checking if the offet of the list is greater that 0
            int minOffset = list.Count - MAX_GRAPH_LENGHT;
            if (minOffset < 0) AxisMin = 0; 
            else AxisMin = minOffset;

            list.Add(value);
            BPMLabels.Add(time.ToString("HH:mm:ss"));
        }

        #endregion

        private void SetNewPatientData(object sender, object data)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {

                int BikeIndex = this.manager.BikeMeasurements.Count - 1;
                int HeartIndex = this.manager.HRMeasurements.Count - 1;

                if (BikeIndex >= 0)
                {
                    double speed = this.manager.BikeMeasurements[BikeIndex].CurrentSpeed;

                    // setting the labels
                    this.Speed = $"Snelheid: {speed} km/h";
                    this.TotalW = $"Totaal: {this.manager.BikeMeasurements[BikeIndex].CurrentTotalWattage / 1000f} kW";
                    this.CurrentW = $"Huidig: {this.manager.BikeMeasurements[BikeIndex].CurrentWattage} Watt";
                    this.Distance = $"Afstand: {this.manager.BikeMeasurements[BikeIndex].CurrentTotalDistance / 1000f} km";
                    this.RPM = "RPM: " + this.manager.BikeMeasurements[BikeIndex].CurrentRPM;


                    // updating the graphs
                    
                }

                if (HeartIndex >= 0)
                {
                    int BPM = this.manager.HRMeasurements[HeartIndex].CurrentHeartrate;

                    this.BPM = "BPM: " + BPM;

                    setNewPoint(this.SeriesCollection[0].Values, (double)BPM, this.manager.HRMeasurements[HeartIndex].MeasurementTime);
                }


            });
        }

        private void HandleSessionPatientClicked()
        {
            this.window.Content = new SessionDetailViewModel(this.window, SelectedSessionPatient);
        }

        private int CalculateAge()
        {
            int years = DateTime.Now.Year - this.Patient.DateOfBirth.Year;
            if (DateTime.Now.Month > this.Patient.DateOfBirth.Month)
            {
                return years;
            } else if (DateTime.Now.Month < this.Patient.DateOfBirth.Month)
            {
                return years - 1;
            } else
            {
                if (DateTime.Now.Day >= this.Patient.DateOfBirth.Day)
                {
                    return years;
                }

                return years - 1;
            }
        }

        private void SendAbort()
        {
            this.manager.AbortSession();
            this.window.Content = new DashboardViewModel(this.window);
        }

        private void SendPersonalMessage()
        {
            this.manager.PersonalMessage(Message);
            UpdateListView();
        }

        private void UpdateListView()
        {
            ObservableCollection<string> TempList = MessageList;

            TempList.Add(Message);

            MessageList = new ObservableCollection<string>(TempList);
        }
        
        private void UpdateResistance()
        {
            this.manager.SetResistance(ResistanceValue);
        }

        private void CloseDetail()
        {
            // Telling the manager this window is closing
            this.manager.CloseManager();

            // Switching from active window
            this.window.Content = new DashboardViewModel(this.window);
        }
    }
}
