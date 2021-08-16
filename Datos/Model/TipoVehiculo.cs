using System;
using System.Collections.Generic;

#nullable disable

namespace Datos.Model
{
    public partial class TipoVehiculo
    {
        public TipoVehiculo()
        {
            Vehiculos = new HashSet<Vehiculo>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }

        public virtual ICollection<Vehiculo> Vehiculos { get; set; }
    }
}
