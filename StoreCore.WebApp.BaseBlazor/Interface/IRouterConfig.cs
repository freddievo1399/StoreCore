using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.BaseBlazor
{
    public interface IRouterConfig
    {
        public Task OnNavigateAsync(NavigationContext args);
    }
}
