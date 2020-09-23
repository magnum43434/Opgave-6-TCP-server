using System;
using System.Collections.Generic;
using System.Text;

namespace FanOutputModelLib
{
    public class FanOutputModel
    {
        private string _name;
        private int _temp;
        private int _humidity;

        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length >= 2)
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Navn", "Navnet skal være minimum 2 karakterer lang");
                }
            }
        }

        public int Temp
        {
            get { return _temp; }
            set
            {
                if (15 <= value && value <= 25)
                {
                    _temp = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Temp", "Temp skal være et tal mellem 15 og 25");
                }
            }
        }

        public int Humidity
        {
            get { return _humidity; }
            set
            {
                if (30 <= value && value <= 80)
                {
                    _humidity = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Fugt", "Fugt skal være  et tal mellem 30 og 80");
                }
            }
        }

        public FanOutputModel(int id, string name, int temp, int humidity)
        {
            Id = id;
            Name = name;
            Temp = temp;
            Humidity = humidity;
        }
    }
}
