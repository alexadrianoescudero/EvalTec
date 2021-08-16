using System;
using System.Collections.Generic;

#nullable disable

namespace Datos.Model
{
    public partial class Cliente
    {
        public Cliente()
        {
            Vehiculos = new HashSet<Vehiculo>();
        }

        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public int Estado { get; set; }

        public virtual ICollection<Vehiculo> Vehiculos { get; set; }
    }
}
