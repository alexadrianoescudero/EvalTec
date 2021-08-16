using System;
using System.Collections.Generic;

#nullable disable

namespace Datos.Model
{
    public partial class Parqueo
    {
        public int Codigo { get; set; }
        public string PlacaVehiculo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public int Estado { get; set; }
    }
}
