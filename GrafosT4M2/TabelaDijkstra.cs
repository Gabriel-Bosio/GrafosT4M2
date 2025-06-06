using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GrafosT4M2
{
    internal class TabelaDijkstra
    {
        private float _distancia;
        private int _anterior;
        private bool _fechado;

        public float Distancia { get => _distancia; set => _distancia = value; }

        public int Anterior { get => _anterior; set => _anterior = value; }

        public bool Fechado { get => _fechado; set => _fechado = value; }

        public TabelaDijkstra(float distancia, int anterior, bool fechado)
        {
            _distancia = distancia;
            _anterior = anterior;
            _fechado = fechado;
        }

        public static List<int> ObterCaminho(List<TabelaDijkstra> tabela, int destino)
        {
            List<int> Indices = new List<int>();

            if (tabela[destino].Distancia == 0)
                return new List<int>() { destino };

            if (tabela[destino].Anterior == -1)
                return new List<int>();

            int indiceAtual = destino;
            Indices.Add(indiceAtual);

            while (tabela[indiceAtual].Distancia != 0)
            {
                indiceAtual = tabela[indiceAtual].Anterior;
                Indices.Add(indiceAtual);
            }

            Indices.Reverse();

            return Indices;
        }
    }
}
