﻿using Prism.Commands;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Window window;
        private DashboardManager manager;

        public DashboardViewModel(Window window)
        {
            this.window = window;

            this.manager = new DashboardManager();
            this.manager.OnPatientUpdated += (s, d) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MakeList(d);
                });
            };
        }

        private ICommand _AddSessionCommand;
        public ICommand AddSessionCommand
        {
            get
            {
                if (_AddSessionCommand == null)
                {
                    _AddSessionCommand = new GeneralCommand(
                        param => ShowPopUp()
                        );
                }
                return _AddSessionCommand;
            }

        }

        private void ShowPopUp()
        {
            SessionPopUp s = new SessionPopUp();
            s.Show();
        }

        private ICommand _SendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                if (_SendMessageCommand == null)
                {
                    _SendMessageCommand = new GeneralCommand(
                        param => SendMessage()
                        );
                }
                return _SendMessageCommand;
            }

        }

        private void SendMessage()
        {
            this.manager.BroadcastMessage(MessageBoxText);

        }


        private ObservableCollection<SharedPatient> mAllPatients;
        public ObservableCollection<SharedPatient> AllPatients
        {
            get { return mAllPatients; }
            set
            {
                mAllPatients = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllPatients"));
            }
        }

        private ObservableCollection<SharedPatient> _InSessionPatients;
        public ObservableCollection<SharedPatient> InSessionPatients
        {
            get { return _InSessionPatients; }
            set
            {
                _InSessionPatients = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InSessionPatients"));
            }
        }

        private ICommand _SwitchToPatientView;
        public ICommand SwitchToPatientView
        {
            get
            {
                if (_SwitchToPatientView == null)
                {
                    _SwitchToPatientView = new GeneralCommand(
                        param => SwitchView()
                        );
                }
                return _SwitchToPatientView;
            }

        }

        private void SwitchView()
        {
            this.window.Content = new PatientListViewModel(this.window);
        }

        private string _MessageBoxText;
        public string MessageBoxText
        {
            get { return _MessageBoxText; }
            set
            {
                _MessageBoxText = value;
            }
        }

        private void MakeList(List<SharedPatient> d)
        {
            List<SharedPatient> ActiveSessionPatients = new List<SharedPatient>();
            List<SharedPatient> AllPatients = new List<SharedPatient>();

            foreach (SharedPatient p in d)
            {
                if (p.InSession)
                {
                    ActiveSessionPatients.Add(p);
                } else
                {
                    AllPatients.Add(p);
                }


            }

            this.AllPatients = new ObservableCollection<SharedPatient>(AllPatients);
            this.InSessionPatients = new ObservableCollection<SharedPatient>(ActiveSessionPatients);
        }

        private SharedPatient _SelectedPatientWithoutSession;
        public SharedPatient SelectedPatientWithoutSession
        {
            get { return _SelectedPatientWithoutSession; }
            set
            {
                _SelectedPatientWithoutSession = value;
                StartSessionPopUp();
            }
        }

        private void StartSessionPopUp()
        {
            if (MessageBox.Show("Start sessie met " + SelectedPatientWithoutSession.FirstName + " " + SelectedPatientWithoutSession.LastName, "Sessie", MessageBoxButton.YesNo) != MessageBoxResult.No)
            {
                this.manager.StartSession(SelectedPatientWithoutSession);
                this.manager.RequestActiveClients();
            }
        }

        private SharedPatient _SelectedPatientWithSession;
        public SharedPatient SelectedPatientWithSession
        {
            get { return _SelectedPatientWithSession; }
            set
            {
                _SelectedPatientWithSession = value;
                ShowDetailWindow();
            }
        }

        private void ShowDetailWindow()
        {
            this.window.Content = new SessionDetailViewModel(this.window, SelectedPatientWithSession);
        }
    }
}
