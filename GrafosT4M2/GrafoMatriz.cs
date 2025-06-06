using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GrafosT4M2
{
    internal class GrafoMatriz : Grafo
    {

        #region Atributos, propriedades e construtor

        private List<List<float>> _arestas;

        public List<List<float>> Arestas
        {
            get { return _arestas; }
            set { _arestas = value; }
        }

        public GrafoMatriz(bool ponderado, bool direcionado) : base(ponderado, direcionado)
        {
            _arestas = new List<List<float>>();
        }

        #endregion

        #region Controle vértices
        public bool InserirVertice(string label)
        {
            // Adiciona o vértice no método pai e ajusta o grafo caso sucedido
            if (base.InserirVertice(label))
            {
                int index = Vertices.IndexOf(label);

                Arestas.ForEach(x => x.Add(0));
                Arestas.Add(new List<float>());
                for (int i = 0; i <= index; i++) Arestas[index].Add(0);
                return true;
            }
            return false;
        }

        public bool RemoverVertice(int indice)
        {
            // Remove o vértice no método pai e ajusta o grafo caso sucedido
            if (base.RemoverVertice(indice))
            {
                Arestas.RemoveAt(indice);
                Arestas.ForEach(x => x.RemoveAt(indice));

                return true;
            }
            return false;
        }

        #endregion

        #region Grafo e controle de arestas
        public override void ImprimeGrafo() // Em processo
        {

            // Define o espaçamento entre colunas
            int maxS = Vertices.MaxBy(x => x.Length).Length + 2;

            // Gera primeira linha com labels
            GeraEspaco(maxS);
            for (int i = 0; i < Vertices.Count; i++)
            {
                Console.Write(Vertices[i]);
                GeraEspaco(maxS - Vertices[i].Length);
            }

            Console.Write("\n\n");

            for (int i = 0; i < Vertices.Count; i++)
            {
                Console.Write(Vertices[i]);
                GeraEspaco(maxS - Vertices[i].Length);

                // Imprime coluna
                for (int j = 0; j < Vertices.Count; j++)
                {
                    Console.Write(Arestas[i][j]);
                    GeraEspaco(maxS - Arestas[i][j].ToString().Length);
                }
                Console.Write("\n\n");
            }
        }

        private void GeraEspaco(int size)
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write(" ");
            }

        }

        public override bool InserirAresta(int origem, int destino, float peso = 1)
        {
            if (ExisteAresta(origem, destino) || peso < 1) return false; // Não insere caso já exista

            float val = !Ponderado ? 1 : peso;

            Arestas[origem][destino] = val;

            if (!Direcionado) Arestas[destino][origem] = val;

            return true;
        }

        public override bool RemoverAresta(int origem, int destino)
        {
            if (!ExisteAresta(origem, destino)) return false; // Não remove caso não exista

            Arestas[origem][destino] = 0;

            if (!Direcionado) Arestas[destino][origem] = 0;

            return true;
        }

        public override bool ExisteAresta(int origem, int destino)
        {
            return Arestas[origem][destino] == 0 ? false : true;
        }

        public override float PesoAresta(int origem, int destino)
        {
            return Arestas[origem][destino];
        }

        public override List<int> RetornarVizinhos(int vertice)
        {
            List<int> vizinhos = new List<int>();
            for (int i = 0; i < Arestas[vertice].Count; i++)
            {
                if (Arestas[vertice][i] != 0) vizinhos.Add(i);
            }

            return vizinhos;
        }
        #endregion
    }
}
