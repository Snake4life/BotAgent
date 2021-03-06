﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BotGUIClient
{
    public class BotItem :INotifyPropertyChanged
    {
        private string _BotName;
        private string _Key;
        private string _UserName;
        private string _Password;
        private Int32 _XPBoost;
        private string _Game;
        private string _Spell1;
        private string _Spell2;
        private string _Summoner;
        private int _Lvl;
        private Int32 _TotalIP;
        private Int32 _TotalRP;
        private string _Status;
        private string _StatusBar;
        private string _EventDT;


        public string EventDT
        {
            get { return _EventDT; }
            set { _EventDT = value; OnPropertyChanged("EventDT"); }
        }


        public string StatusBar
        {
            get { return _StatusBar; }
            set { _StatusBar = value; OnPropertyChanged("StatusBar"); }
        }


        public string Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged("Status"); }
        }


        public Int32 TotalRP
        {
            get { return _TotalRP; }
            set { _TotalRP = value; OnPropertyChanged("TotalRP"); }
        }

        
        public Int32 TotalIP
        {
            get { return _TotalIP; }
            set { _TotalIP = value; OnPropertyChanged("TotalIP"); }
        }



        public Int32 Lvl
        {
            get { return _Lvl; }
            set { _Lvl = value; OnPropertyChanged("Lvl"); }
        }



        public string Summoner
        {
            get { return _Summoner; }
            set { _Summoner = value; OnPropertyChanged("Summoner"); }
        }



        public string Spell2
        {
            get { return _Spell2; }
            set { _Spell2 = value; OnPropertyChanged("Spell2"); }
        }

        
        public string Spell1
        {
            get { return _Spell1; }
            set { _Spell1 = value; OnPropertyChanged("Spell1"); }
        }

        
        public string Game
        {
            get { return _Game; }
            set { _Game = value; OnPropertyChanged("Game"); }
        }

        
        public Int32 XPBoost
        {
            get { return _XPBoost; }
            set { _XPBoost = value; OnPropertyChanged("XPBoost"); }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged("Password"); }
        }




        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged("UserName"); }
        }


        public string Key
        {
            get { return _Key; }
            set { _Key = value; OnPropertyChanged("Key"); }
        }


        public string BotName
        {
            get { return _BotName; }
            set { _BotName = value; OnPropertyChanged("BotName"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
