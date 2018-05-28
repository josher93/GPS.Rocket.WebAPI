﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamaaTech.Smpp.Net.Lib;

namespace YoVendoRecargaAPI.SVC.Models
{
    public interface ISmppConfiguration
    {
        int Id { get; }
        int AutoReconnectDelay { get; }
        string DefaultServiceType { get; }
        DataCoding Encoding { get; }
        string Host { get; }
        bool IgnoreLength { get; }
        int KeepAliveInterval { get; }
        string Name { get; }
        string Password { get; }
        int Port { get; }
        int ReconnectInteval { get; }
        string SourceAddress { get; }
        string SystemID { get; }
        string SystemType { get; }
        int TimeOut { get; }
        bool StartAutomatically { get; }
        string DestinationAddressRegex { get; }
    }
}
