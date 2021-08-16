using System;
using System.Collections.Generic;

#nullable disable

namespace Datos.Model
{
    public partial class Vehiculo
    {
        public string Placa { get; set; }
        public string Chasis { get; set; }
        public int Estado { get; set; }
        public int TipoVehiculo { get; set; }
        public string CedulaCliente { get; set; }

        public virtual Cliente CedulaClienteNavigation { get; set; }
        public virtual TipoVehiculo TipoVehiculoNavigation { get; set; }
    }
}
