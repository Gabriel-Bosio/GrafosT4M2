using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafosT4M2
{
    internal class GrafoLista : Grafo
    {
        #region Atributos, propriedades e construtor

        private List<List<(int destino, float peso)>> _arestas;

        public List<List<(int destino, float peso)>> Arestas
        {
            get { return _arestas; }
            set { _arestas = value; }
        }

        public GrafoLista(bool ponderado, bool direcionado) : base(ponderado, direcionado)
        {
            _arestas = new List<List<(int destino, float peso)>>();
        }

        public GrafoLista(GrafoLista grafo) : base(grafo)
        {
            _arestas = new List<List<(int destino, float peso)>>();
            grafo.Arestas.ForEach(ar => _arestas.Add(new List<(int destino, float peso)>(ar)));
        }

        #endregion

        #region Controle vértices
        public bool InserirVertice(string label)
        {
            // Adiciona o vértice no método pai e ajusta o grafo caso sucedido
            if (base.InserirVertice(label))
            {
                Arestas.Add(new List<(int destino, float peso)>());
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

                // Remove todas as arestas que apontam para o vértice removido
                foreach (var lista in Arestas)
                {
                    lista.RemoveAll(aresta => aresta.destino == indice);
                }

                // Atualiza os índices dos destinos das arestas
                for (int i = 0; i < Arestas.Count; i++)
                {
                    for (int j = 0; j < Arestas[i].Count; j++)
                    {
                        if (Arestas[i][j].destino > indice)
                        {
                            Arestas[i][j] = (Arestas[i][j].destino - 1, Arestas[i][j].peso);
                        }
                    }
                }

                return true;
            }
            return false;

        }

        #endregion

        #region Grafo e controle de arestas
        public override void ImprimeGrafo()
        {

            for (int i = 0; i < Vertices.Count; i++)
            {
                Console.Write(Vertices[i] + " -> ");
                foreach (var aresta in Arestas[i])
                {
                    Console.Write($"({Vertices[aresta.destino]}, {aresta.peso}) ");
                }
                Console.WriteLine();
            }
        }

        public override bool InserirAresta(int origem, int destino, float peso = 1)
        {
            if (ExisteAresta(origem, destino)) return false; // Não insere caso já exista

            float val = !Ponderado ? 1 : peso;

            Arestas[origem].Add((destino, val));

            // Se for um self-loop, não insere a aresta novamente
            if (!Direcionado && origem != destino)
            {
                Arestas[destino].Add((origem, val));
            }

            return true;
        }

        public override bool RemoverAresta(int origem, int destino)
        {
            if (!ExisteAresta(origem, destino)) return false; // Não remove caso não exista

            Arestas[origem].RemoveAll(aresta => aresta.destino == destino);

            if (!Direcionado)
            {
                Arestas[destino].RemoveAll(aresta => aresta.destino == origem);
            }

            return true;
        }

        public override bool ExisteAresta(int origem, int destino)
        {
            return Arestas[origem].Any(aresta => aresta.destino == destino);
        }

        public override float PesoAresta(int origem, int destino)
        {
            var aresta = Arestas[origem].FirstOrDefault(a => a.destino == destino);
            return aresta.peso;
        }

        public override List<int> RetornarVizinhos(int vertice)
        {
            return Arestas[vertice].Where(aresta=> aresta.peso > 0).Select(aresta => aresta.destino).ToList();
        }

        #endregion

        #region Fluxo Maximo
        public override GrafoLista Clonar()
        {
            return new GrafoLista(this);
        }

        #endregion
    }
}
