using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafosT4M2
{
    internal class Controle
    {
        private GrafoMatriz grafoMatriz;
        private GrafoLista grafoLista;

        public Controle() { }

        public void Inicio()
        {

            Console.WriteLine("Criacao do grafo: \n");
            Console.WriteLine("Grafo Direcionado? (Arestas poderao ter uma só direcao) -- (y/n)");
            bool direcionado = Console.ReadLine().Equals("y") ? true : false;

            Console.WriteLine("Grafo Ponderado? (Arestas poderao ter diferentes pesos)  -- (y/n)");
            bool ponderado = Console.ReadLine().Equals("y") ? true : false;

            grafoMatriz = new GrafoMatriz(ponderado, direcionado);
            grafoLista = new GrafoLista(ponderado, direcionado);

            Manipulacao();
        }

        private void Manipulacao()
        {
            bool sair = false;
            int escolha;

            while (!sair)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Opcoes:\n");
                    Console.WriteLine("1 - Exibir Grafo em Matriz");
                    Console.WriteLine("2 - Exibir Grafo em Lista");
                    Console.WriteLine("3 - Inserir Vertice");
                    Console.WriteLine("4 - Remover Vertice");
                    Console.WriteLine("5 - Exibir Label com base no indice");
                    Console.WriteLine("6 - Inserir aresta");
                    Console.WriteLine("7 - Remover aresta");
                    Console.WriteLine("8 - Verificar aresta");
                    Console.WriteLine("9 - Lista vizinhos");
                    Console.WriteLine("0 - Sair\n");
                    Console.WriteLine("-");

                    escolha = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("\n\n");

                    switch (escolha)
                    {
                        case 0:
                            Console.WriteLine("Saindo ...");
                            sair = true;
                            break;
                        case 1:
                            Console.Clear();
                            ExibeMatriz();
                            break;
                        case 2:
                            Console.Clear();
                            ExibeLista();
                            break;
                        case 3:
                            AdicionarVertice();
                            break;
                        case 4:
                            RemoverVertice();
                            break;
                        case 5:
                            ExibirLabel();
                            break;
                        case 6:
                            AdicionarAresta();
                            break;
                        case 7:
                            RemoverAresta();
                            break;
                        case 8:
                            VerificarAresta();
                            break;
                        case 9:
                            ExibirVizinhos();
                            break;
                    }
                }
                catch { }
            }
        }

        private void ExibeMatriz()
        {
            if (grafoMatriz.Vertices.Count > 0)
            {
                grafoMatriz.ImprimeGrafo();
            }
            else
            {
                Console.WriteLine("Grafo de matriz não possui vertices");
            }

            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

        private void ExibeLista()
        {
            if (grafoLista.Vertices.Count > 0)
            {
                grafoLista.ImprimeGrafo();
            }
            else
            {
                Console.WriteLine("Grafo de lista não possui vertices");
            }
            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

        private void AdicionarVertice()
        {
            Console.WriteLine("Informe um nome para a vertice: ");
            string label = Console.ReadLine();
            if (grafoMatriz.InserirVertice(label))
            {
                Console.WriteLine("\nVertice inserida na matriz!");
            }
            else { Console.WriteLine("\nVertice com este nome já existe na matriz!"); }

            if (grafoLista.InserirVertice(label))
            {
                Console.WriteLine("\nVertice inserida na lista!");
            }
            else { Console.WriteLine("\nVertice com este nome já existe na lista!"); }

            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

        private void RemoverVertice()
        {
            Console.WriteLine("Informe o indice do vertice: ");
            int indice = Convert.ToInt32(Console.ReadLine());
            if (indice < grafoMatriz.Vertices.Count && indice >= 0 && grafoMatriz.RemoverVertice(indice))
            {
                Console.WriteLine("\nVertice removida na matriz!");
            }
            else { Console.WriteLine("\nIndice mencionado não existe na matriz!"); }

            if (indice < grafoLista.Vertices.Count && indice >= 0 && grafoLista.RemoverVertice(indice))
            {
                Console.WriteLine("\nVertice removida na lista!");
            }
            else { Console.WriteLine("\nIndice mencionado não existe na lista!"); }

            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

        private void ExibirLabel()
        {
            Console.WriteLine("Informe o indice do vertice: ");
            int indice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"\nVertice {indice} da matriz: {grafoMatriz.LabelVertice(indice)}");

            Console.WriteLine($"\nVertice {indice} da lista: {grafoLista.LabelVertice(indice)}");

            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

        private void AdicionarAresta()
        {
            Console.WriteLine("Informe o indice da vertice de origem: ");
            int indiceO = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Informe o indice da vertice de destino: ");
            int indiceD = Convert.ToInt32(Console.ReadLine());

            int peso;
            if (grafoMatriz.Ponderado)
            {
                Console.WriteLine("Informe o peso da aresta: ");
                peso = Convert.ToInt32(Console.ReadLine());
            }
            else peso = 1;

            if (grafoMatriz.InserirAresta(indiceO, indiceD, peso))
            {
                Console.WriteLine($"\nInserida Aresta de {grafoMatriz.LabelVertice(indiceO)} para {grafoMatriz.LabelVertice(indiceD)} na matriz!");
            }

            if (grafoLista.InserirAresta(indiceO, indiceD, peso))
            {
                Console.WriteLine($"\nInserida Aresta de {grafoLista.LabelVertice(indiceO)} para {grafoLista.LabelVertice(indiceD)} na lista!");
            }

            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

        private void RemoverAresta()
        {
            Console.WriteLine("Informe o indice da vertice de origem: ");
            int indiceO = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Informe o indice da vertice de destino: ");
            int indiceD = Convert.ToInt32(Console.ReadLine());


            if (grafoMatriz.RemoverAresta(indiceO, indiceD))
            {
                Console.WriteLine($"\nRemovida Aresta de {grafoMatriz.LabelVertice(indiceO)} para {grafoMatriz.LabelVertice(indiceD)} na matriz!");
            }

            if (grafoLista.RemoverAresta(indiceO, indiceD))
            {
                Console.WriteLine($"\nRemovida Aresta de {grafoLista.LabelVertice(indiceO)} para {grafoLista.LabelVertice(indiceD)} na lista!");
            }

            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

        private void VerificarAresta()
        {
            Console.WriteLine("Informe o indice da vertice de origem: ");
            int indiceO = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Informe o indice da vertice de destino: ");
            int indiceD = Convert.ToInt32(Console.ReadLine());


            if (grafoMatriz.ExisteAresta(indiceO, indiceD))
            {
                Console.WriteLine($"\nAresta de {grafoMatriz.LabelVertice(indiceO)} para {grafoMatriz.LabelVertice(indiceD)} possui o valor {grafoMatriz.PesoAresta(indiceO, indiceD)} na matriz!");
            }
            else
            {
                Console.WriteLine($"\nAresta de {grafoMatriz.LabelVertice(indiceO)} para {grafoMatriz.LabelVertice(indiceD)} não existe na matriz!");
            }

            if (grafoLista.ExisteAresta(indiceO, indiceD))
            {
                Console.WriteLine($"\nAresta de {grafoLista.LabelVertice(indiceO)} para {grafoLista.LabelVertice(indiceD)} possui o valor {grafoLista.PesoAresta(indiceO, indiceD)} na lista!");
            }
            else
            {
                Console.WriteLine($"\nAresta de {grafoLista.LabelVertice(indiceO)} para {grafoLista.LabelVertice(indiceD)} não existe na lista!");
            }

            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

        private void ExibirVizinhos()
        {
            Console.WriteLine("Informe o indice do vertice: ");
            int indice = Convert.ToInt32(Console.ReadLine());

            List<int> vizinhosMatriz = grafoMatriz.RetornarVizinhos(indice);
            if (vizinhosMatriz.Count > 0)
            {
                Console.WriteLine($"\nVizinhos de {grafoMatriz.LabelVertice(indice)} na matriz:\n");
                vizinhosMatriz.ForEach(x => Console.Write($"{grafoMatriz.LabelVertice(x)}  "));
            }
            else
            {
                Console.WriteLine($"\nVertice {grafoMatriz.LabelVertice(indice)} não possui vizinhos na matriz!\n");
            }

            Console.WriteLine("\n\n");

            List<int> vizinhosLista = grafoLista.RetornarVizinhos(indice);
            if (vizinhosLista.Count > 0)
            {
                Console.WriteLine($"\nVizinhos de {grafoLista.LabelVertice(indice)} na lista:\n");
                vizinhosLista.ForEach(x => Console.Write($"{grafoLista.LabelVertice(x)}  "));
            }
            else
            {
                Console.WriteLine($"\nVertice {grafoLista.LabelVertice(indice)} não possui vizinhos na lista!\n");
            }

            Console.WriteLine("\nPrecione Enter...");
            Console.ReadLine();
        }

    }
}
