#region Using directives

using System;
using System.Reflection;
using System.Runtime.InteropServices;

#endregion

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "config/log4net.config")]