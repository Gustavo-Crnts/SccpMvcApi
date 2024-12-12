using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SccpMvcApi.Models
{
    public class PartidasViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set;}
        public string Dia { get; set; }
        public string Horario { get; set; }
        public string Estadio { get; set; }
        public string Competicao { get; set; }
        public string Adversario { get; set; }
        public string Resultado { get; set; }
        public string Placar { get; set; }
    }
}