using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GrafosT4M2
{
    internal abstract class Grafo
    {

        #region Atributos, propriedades e construtor

        private bool _ponderado;
        private bool _direcionado;
        private List<string> _vertices;

        public bool Ponderado
        {
            get { return _ponderado; }
        }

        public bool Direcionado
        {
            get { return _direcionado; }
        }

        public List<string> Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }

        public Grafo(bool ponderado, bool direcionado)
        {
            this._ponderado = ponderado;
            this._direcionado = direcionado;
            _vertices = new List<string>();
        }

        #endregion

        #region Controle vértices
        protected bool InserirVertice(string label)
        {
            // Não insere caso label já existir
            if (Vertices.Any(x => x.Equals(label)))
            {
                return false;
            }
            Vertices.Add(label);
            return true;
        }

        protected bool RemoverVertice(int indice)
        {
            //Não remove caso índice inválido
            if (indice < 0 || indice >= Vertices.Count)
                return false;

            Vertices.RemoveAt(indice);

            return true;
        }

        public string LabelVertice(int index)
        {
            return Vertices[index];
        }

        #endregion

        #region Grafo e controle de arestas
        public abstract void ImprimeGrafo();

        public abstract bool InserirAresta(int origem, int destino, float peso = 1);

        public abstract bool RemoverAresta(int origem, int destino);

        public abstract bool ExisteAresta(int origem, int destino);

        public abstract float PesoAresta(int origem, int destino);

        public abstract List<int> RetornarVizinhos(int vertice);

        #endregion

        #region Buscas
        public List<int> RetornarBuscaProfundidade(int origem)
        {
            List<int> verticesVisitadas = new List<int>();

            BuscaProfundidade(origem, ref verticesVisitadas); //Inicia a busca de forma recursiva

            return verticesVisitadas;
        }

        private void BuscaProfundidade(int origem, ref List<int> verticesVisitadas)
        {
            verticesVisitadas.Add(origem);
            List<int> vizinhos = RetornarVizinhos(origem);
            foreach (int vizinho in vizinhos)
            {
                if (!verticesVisitadas.Any(v => v == vizinho))
                {
                    BuscaProfundidade(vizinho, ref verticesVisitadas);
                }
            }
        }

        public List<int> RetornarBuscaLargura(int origem)
        {

            List<int> verticesVisitadas = new List<int>();
            List<int> fila = new List<int>();

            //Visita o vertice de origem (Adiciona na fila e adiciona na lista de visitados)
            verticesVisitadas.Add(origem);
            fila.Add(origem);


            while (fila.Count > 0)
            {
                int atual = fila[0]; //Seleciona o próximo vértice da fila
                List<int> vizinhos = RetornarVizinhos(atual);

                foreach (int vizinho in vizinhos)
                {
                    if (!verticesVisitadas.Any(v => v == vizinho)) //Visita todos os vizinhos que ainda não foram vizitados 
                    {
                        verticesVisitadas.Add(vizinho);
                        fila.Add(vizinho);
                    }
                }
                fila.RemoveAt(0); //Remove da fila o vértice que acabou de ser selecionado
            }

            return verticesVisitadas;
        }

        public List<(float Distancia, List<int> Caminho)> RetornarDijkstra(int origem)
        {
            var tabela = new List<TabelaDijkstra>();
            Vertices.ForEach(vertice => tabela.Add(new TabelaDijkstra(-1, -1, false))); // Composto por distância e verificador de fechamento

            //Inicia distância 0 para origem e seleciona como vertice atual
            int verticeAtual = origem;
            tabela[verticeAtual].Distancia = 0;

            do
            {
                //Verifica a distância dos vizinhos do vértice atual e fecha o vértice em seguida
                List<int> vizinhos = RetornarVizinhos(verticeAtual);
                foreach (int verticeDestino in vizinhos)
                {
                    float distancia = tabela[verticeAtual].Distancia + PesoAresta(verticeAtual, verticeDestino);
                    if (distancia < tabela[verticeDestino].Distancia || tabela[verticeDestino].Distancia < 0)
                    {
                        tabela[verticeDestino].Distancia = distancia; //Atualiza a distância caso seja a primeira ou a menor
                        tabela[verticeDestino].Anterior = verticeAtual;
                    }
                }
                tabela[verticeAtual].Fechado = true;

                verticeAtual = tabela.IndexOf(tabela.FirstOrDefault(vertice => !vertice.Fechado && vertice.Distancia >= 0)); //Seleciona próximo vértice aberto com distância conhecida

            } while (verticeAtual >= 0); //Encerra laço caso não tenha mais vértices acessíveis

            List<(float Distancia, List<int>)> caminhos = new();
            for (int i = 0; i < tabela.Count; i++)
            {
                float distancia = tabela[i].Distancia;
                List<int> caminho = TabelaDijkstra.ObterCaminho(tabela, i);

                caminhos.Add(new(distancia, caminho));
            }

            return caminhos;
        }

        //Sobrecarga que imprime ordem de acesso em buscas de profundidade e largura
        public void ImprimeBusca(List<int> vizitas, bool isLargura = false)
        {
            if (isLargura)
                Console.Write("\nBusca por Largura: ");
            else
                Console.Write("\nBusca por Profundidade: ");

            for (int i = 0; i < vizitas.Count; i++)
            {
                Console.Write($"{vizitas[i]}");
                if (i < vizitas.Count - 1) Console.Write($" -> ");
            }
            Console.WriteLine();
        }

        //Sobrecarga que imprime distâncias de um ponto de origem para cada vértice com base em busca de Dijkstra
        public void ImprimeBusca(List<(float Distancia, List<int> Caminho)> caminhos)
        {
            Console.WriteLine($"\nMenores caminhos a partir de {LabelVertice(caminhos.IndexOf(caminhos.FirstOrDefault(x => x.Distancia == 0)))}: ");
            for (int i = 0; i < caminhos.Count; i++)
            {
                string dist = caminhos[i].Distancia != -1 ? Convert.ToString(caminhos[i].Distancia) : "Infinito";
                Console.Write($"\n{LabelVertice(i)} : Distancia = {dist}     Caminho -- ");
                if (caminhos[i].Caminho.Count > 0)
                {
                    for (int j = 0; j < caminhos[i].Caminho.Count; j++)
                    {
                        if (j < caminhos[i].Caminho.Count - 1)
                        {
                            Console.Write($"{LabelVertice(caminhos[i].Caminho[j])} -> ");
                        }
                        else
                        {
                            Console.Write($"{LabelVertice(caminhos[i].Caminho[j])}\n");
                        }
                    }
                }
                else Console.Write("Nenhum\n");

            }
        }
        #endregion

        #region Coloracao

        public (double Tempo, int QtdCores, Dictionary<int, int> Cores) ColoracaoForcaBruta()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Dictionary<int, int> corVertices = new Dictionary<int, int>();
            for (int i = 0; i < Vertices.Count; i++) { corVertices.Add(i, 0); }

            //Incializa com no máximo 2 cores: Lógica de vetor, 2 cores são cor 0 e cor 1
            int corCount = 2;

            do
            {
                //Laço que segue todas as combinações possíveis seguindo uma lógica de tabela verdade
                int indice = 0;
                while (++corVertices[indice] >= corCount)
                {
                    corVertices[indice] = 0;
                    indice++;

                    if (indice >= corVertices.Count)
                    {
                        corCount++;
                        break;
                    }
                }

            } while (!VerificaColoracao(corVertices));

            sw.Stop();

            return(sw.Elapsed.TotalSeconds, corCount, corVertices);
        }

        private bool VerificaColoracao(Dictionary<int, int> cores) 
        {
            if(cores.Any(x => x.Value == -1) || !cores.Any(x => x.Value != 0))
                return false;

            for(int i = 0; i < Vertices.Count; i++)
            {
                List<int> vizinhos = RetornarVizinhos(i);

                foreach(int vizinho in vizinhos)
                {
                    if (cores[i] == cores[vizinho]) return false;
                }
            }

            return true;
        }

        public (double Tempo, int QtdCores, List<ColoracaoVertice> Cores) ColoracaoWelshPowell()
        {
            Stopwatch sw = Stopwatch.StartNew();
            int corAtual = 0;
            List<ColoracaoVertice> coresVertices = new List<ColoracaoVertice>();

            for (int i = 0; i < Vertices.Count; i++) { coresVertices.Add(new  ColoracaoVertice(i, RetornarVizinhos(i))); }
            coresVertices = coresVertices.OrderByDescending(c => c.Grau).ToList();

            while (coresVertices.Any(c => c.Cor == -1))
            {
                foreach (var corVertice in coresVertices) // Percorre cada vértice e aplica a cor atual se ela não for usada pelos vizinhos 
                {
                    if(corVertice.Cor == -1)
                    {
                        if (!coresVertices.Any(corVizinho => corVizinho.Cor == corAtual && corVertice.Vizinhos.Contains(corVizinho.Vertice)))
                        {
                            corVertice.Cor = corAtual;
                        }
                    }
                }
                corAtual++;
            }

            sw.Stop();

            coresVertices = coresVertices.OrderBy(c => c.Vertice).ToList();
            return (sw.Elapsed.TotalSeconds, corAtual, coresVertices);
        }

        public (double Tempo, int QtdCores, List<ColoracaoVertice> Cores) ColoracaoDsatur() 
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<ColoracaoVertice> coresVertices = new List<ColoracaoVertice>();
            int qtdCores = 1;
            for (int i = 0; i < Vertices.Count; i++) { coresVertices.Add(new ColoracaoVertice(i, RetornarVizinhos(i))); }

            while (coresVertices.Any(c => c.Cor == -1))
            {
                // Seleciona o vértice não colorido com maior saturação, desempatando por saturação
                ColoracaoVertice corVertice = coresVertices.Where(v => v.Cor == -1).OrderByDescending(v => v.Saturacao).ThenByDescending(v => v.Grau).First();

                int corAtual = 0;
                while(corVertice.Cor == -1) // Verifica cada cor para o vértice até achar um não usado pelos vizinhos
                {
                    if (!coresVertices.Any(corVizinho => corVizinho.Cor == corAtual && corVertice.Vizinhos.Contains(corVizinho.Vertice)))
                    {
                        corVertice.Cor = corAtual;
                        CalcularSaturacao(coresVertices);
                    }
                    corAtual++;
                }
                if (corAtual > qtdCores) qtdCores = corAtual;
            }

            sw.Stop();

            return (sw.Elapsed.TotalSeconds, qtdCores, coresVertices);
        }

        public void CalcularSaturacao(List<ColoracaoVertice> coresVertice)
        {
            foreach(ColoracaoVertice corVertice in coresVertice)
            {
                List<int> vizinhos = RetornarVizinhos(corVertice.Vertice);
                corVertice.Saturacao = coresVertice.Where(c => vizinhos.Contains(c.Vertice)).Select(c => c.Cor).Distinct().Count();
            }
        }

        public (double Tempo, int QtdCores, List<ColoracaoVertice> Cores) ColoracaoSemCriterio()
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<ColoracaoVertice> coresVertices = new List<ColoracaoVertice>();
            int qtdCores = 1;
            for (int i = 0; i < Vertices.Count; i++) { coresVertices.Add(new ColoracaoVertice(i, RetornarVizinhos(i))); }

            foreach(ColoracaoVertice corVertice in coresVertices) //Percorre cada vértice
            {
                int corAtual = 0;
                while (corVertice.Cor == -1) // Verifica cada cor para o vértice até achar um não usado pelos vizinhos
                {
                    if (!coresVertices.Any(corVizinho => corVizinho.Cor == corAtual && corVertice.Vizinhos.Contains(corVizinho.Vertice)))
                    {
                        corVertice.Cor = corAtual;
                    }
                    corAtual++;
                }
                if (corAtual > qtdCores) qtdCores = corAtual;
            }

            sw.Stop();

            return (sw.Elapsed.TotalSeconds, qtdCores, coresVertices);
        }

        public void ImprimeColoracao((double Tempo, int QtdCores, Dictionary<int, int> Cores) resultado)
        {
            Console.WriteLine("\nResultado da Coloracao por Força Bruta:\n");
            Console.WriteLine($"Tempo de Exução: {resultado.Tempo.ToString("F8")} segundos\n");
            Console.WriteLine($"Cores usadas: {resultado.QtdCores}\n");
            if(resultado.Cores.Count < 10)
            {
                Console.WriteLine("Cores de cada vértice:\n");
                foreach(var cor in resultado.Cores)
                {
                    Console.WriteLine($"{LabelVertice(cor.Key)} -> Cor {cor.Value}");
                }
            }

        }

        public void ImprimeColoracao((double Tempo, int QtdCores, List<ColoracaoVertice> Cores) resultado, int tipoColoracao = 0)
        {
            switch (tipoColoracao)
            {
                case 1:
                    Console.WriteLine("\nResultado da Coloracao por Welsh Powell:\n");
                    break;
                case 2:
                    Console.WriteLine("\nResultado da Coloracao por Dsatur:\n");
                    break;
                case 3:
                    Console.WriteLine("\nResultado da Coloracao sem Critérios:\n");
                    break;
                default:
                    Console.WriteLine("\nResultado da Coloracao:\n");
                    break;
            }
            Console.WriteLine($"Tempo de Exução: {resultado.Tempo.ToString("F8")} segundos\n");
            Console.WriteLine($"Cores usadas: {resultado.QtdCores}\n");
            if (resultado.Cores.Count < 10)
            {
                Console.WriteLine("Cores de cada vértice:\n");
                foreach (var cor in resultado.Cores)
                {
                    Console.WriteLine($"{LabelVertice(cor.Vertice)} -> Cor {cor.Cor}");
                }
            }

        }
        #endregion


    }
}
