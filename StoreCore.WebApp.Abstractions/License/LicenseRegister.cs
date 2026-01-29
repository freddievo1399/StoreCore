using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Abstractions;

public static class LicenseRegister
{
    public static void SyncfusionLicenseRegister(string Key)
    {
        SyncfusionLicenseProvider.RegisterLicense(Key);
    }
}
