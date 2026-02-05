using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure;

public class SaveWithJsonAttribute:Attribute
{
    public string? JsonName { get; set; }
}
