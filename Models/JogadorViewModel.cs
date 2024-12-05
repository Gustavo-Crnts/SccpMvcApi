using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SccpMvcApi.Models.Enuns;

namespace SccpMvcApi.Models
{
    public class JogadorViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Salario { get; set; }
        public int Partidas { get; set; }
        public int Gols { get; set; }
        public int Assistencias { get; set; }
        public PosicaoEnum Posicao { get; set; }
    }
}