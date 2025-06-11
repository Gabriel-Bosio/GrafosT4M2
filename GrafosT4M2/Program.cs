using GrafosT4M2;
using System;
internal class Program
{
    private static void Main(string[] args)
    {
        GrafoMatriz grafoM = null;
        GrafoLista grafoL = null;

        //LeitorGrafo leitor = new LeitorGrafo(".\\..\\..\\..\\grafo.txt");
        LeitorGrafo leitor = new LeitorGrafo(".\\..\\..\\..\\grafo.txt");
        int origem = 0;

        leitor.GeraGrafo(ref grafoM);

        grafoM.ImprimeGrafo();

        ////grafoM.ImprimeBusca(grafoM.RetornarBuscaProfundidade(origem), false);

        ////grafoM.ImprimeBusca(grafoM.RetornarBuscaLargura(origem), true);

        ////grafoM.ImprimeBusca(grafoM.RetornarDijkstra(origem));

        //grafoM.ImprimeColoracao(grafoM.ColoracaoForcaBruta());

        //grafoM.ImprimeColoracao(grafoM.ColoracaoWelshPowell(), 1);

        //grafoM.ImprimeColoracao(grafoM.ColoracaoDsatur(), 2);

        //grafoM.ImprimeColoracao(grafoM.ColoracaoSemCriterio(), 3);

        grafoM.ImprimeResultadoFluxoMaximo(grafoM.OtimizarFluxoMaximo(0, 5));

        Console.WriteLine("\n\n\n\n");

        leitor.GeraGrafo(ref grafoL);

        grafoL.ImprimeGrafo();

        //grafoL.ImprimeBusca(grafoL.RetornarBuscaProfundidade(origem), false);

        //grafoL.ImprimeBusca(grafoL.RetornarBuscaLargura(origem), true);

        //grafoL.ImprimeBusca(grafoL.RetornarDijkstra(origem));

        //grafoL.ImprimeColoracao(grafoL.ColoracaoForcaBruta());

        //grafoL.ImprimeColoracao(grafoL.ColoracaoWelshPowell(), 1);

        //grafoL.ImprimeColoracao(grafoL.ColoracaoDsatur(), 2);

        //grafoL.ImprimeColoracao(grafoL.ColoracaoSemCriterio(), 3);

        grafoL.ImprimeResultadoFluxoMaximo(grafoL.OtimizarFluxoMaximo(0, 5));

        Console.WriteLine("\n\n\n\n");
    }


}