﻿using System.ComponentModel.DataAnnotations;

namespace webApiPractica.Models
{
    public class tipoEquipo
    {
        [Key]
        public int id_tipo_equipo { get; set; }
        public string descripcion { get; set; }

        public string estado { get; set; }
    }
}
