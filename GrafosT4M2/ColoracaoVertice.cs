using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafosT4M2
{
    public class ColoracaoVertice
    {
        public int Vertice { get; set; }

        public List<int> Vizinhos { get; set; }

        public int Grau {  get; set; }

        public int Saturacao { get; set; }

        public int Cor {  get; set; }
        public ColoracaoVertice(int vertice, List<int> vizinhos)
        {
            Vertice = vertice;
            Vizinhos = vizinhos;
            Grau = vizinhos.Count;
            Saturacao = 0;
            Cor = -1;
        }
    }
}
