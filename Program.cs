using System;
using System.IO;
using MatFileHandler;

namespace NeuralNetCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            int nodesInputLayer = -1;
            int[] nodesHiddenLayers = null;
            int nodesOutputLayer = -1;
            bool matlabFile = false;

            NeuralNetworkCreator neuralNetworkCreator = new NeuralNetworkCreator();

            nodesInputLayer = defineInputLayer();
            nodesOutputLayer = defineOutputLayer();
            nodesHiddenLayers = defineHiddenLayer();

            neuralNetworkCreator.NodesInputLayer = nodesInputLayer;
            neuralNetworkCreator.NodesOutputLayer = nodesOutputLayer;

            if(nodesHiddenLayers != null){
                foreach(int nodes in nodesHiddenLayers){
                    neuralNetworkCreator.addHiddenLayerNodes(nodes);
                }
            }

            matlabFile = defineFileOutputType();

            neuralNetworkCreator.createNetwork(matlabFile);

            Console.WriteLine("Network created and saved.");
        }

        private static int defineInputLayer(){
            int nodes = -1;
            //Ask for nodes of input layer
            while(nodes <= 0){
                Console.Write("Enter number of nodes for the input layer (amount nodes > 0): ");
                String input = Console.ReadLine();
                try
                {
                    nodes = Int32.Parse(input);
                }
                catch (FormatException)
                {
                    //Nothing to do
                }
            }
            return nodes;
        }

        private static int defineOutputLayer(){
            int nodes = -1;
            //Ask for nodes of output layer
            while(nodes <= 0){
                Console.Write("Enter number of nodes for the output layer (amount nodes > 0): ");
                String input = Console.ReadLine();
                try
                {
                    nodes = Int32.Parse(input);
                }
                catch (FormatException)
                {
                    //Nothing to do
                }
            }
            return nodes;
        }

        private static int[] defineHiddenLayer(){
            int numberHiddenLayers = -1;
            int[] nodesHiddenLayers = null;
            //Ask for number of hidden layers
            while(numberHiddenLayers < 0){
                Console.Write("Enter number hidden layers: ");
                String input = Console.ReadLine();
                try
                {
                    numberHiddenLayers = Int32.Parse(input);
                }
                catch (FormatException)
                {
                    //Nothing to do
                }
            }

            if(numberHiddenLayers > 0){
                nodesHiddenLayers = new int[numberHiddenLayers];

                //Set the hidden layer nodes
                for(int i = 0; i < nodesHiddenLayers.Length; i++){
                    while(nodesHiddenLayers[i] <= 0){
                        Console.Write("Enter number of nodes for hidden layer " + (i + 1) + ": ");
                        String input = Console.ReadLine();
                        try
                        {
                            nodesHiddenLayers[i] = Int32.Parse(input);
                        }
                        catch (FormatException)
                        {
                            //Nothing to do
                        }
                    }
                }
            }

            return nodesHiddenLayers;
        }

        private static bool defineFileOutputType(){
            bool matlabFile = false;
            bool falidInput = false;
            //Ask for nodes of output layer
            while(!falidInput){
                Console.Write("Should the network be saved as a mat-file (y | n): ");
                String input = Console.ReadLine();
                try
                {
                    if(input == "y"){
                        matlabFile = true;
                        falidInput = true;
                    } else if(input == "n"){
                        falidInput = true;
                    }
                }
                catch (FormatException)
                {
                    //Nothing to do
                }
            }
            return matlabFile;
        }
    }
}
