using System;
using System.Collections;
using System.IO;
using MatFileHandler;

namespace NeuralNetCreator
{
    public class NeuralNetworkCreator
    {
        
        private int _nodesInputLayer = 0;
        public int NodesInputLayer{
            get{
                return _nodesInputLayer;
            }

            set{
                if(value <= 0){
                    throw new Exception("Amount of input layer nodes can not be 0 or less.");
                }
                _nodesInputLayer = value;
            }
        }
        private ArrayList _nodesHiddenLayers = null;
        
        private int _nodesOutputLayer = 0;
        public int NodesOutputLayer {
            get{
                return _nodesOutputLayer;
            }
            set{
                if(value <= 0){
                    throw new Exception("Amount of output layer nodes can not be 0 or less.");
                }
                _nodesOutputLayer = value;
            }
        }

        public NeuralNetworkCreator(){
            _nodesHiddenLayers = new ArrayList();
        }

        public void addHiddenLayerNodes(int nodes){
            if(nodes <= 0){
                throw new Exception("Amount of nodes can not be 0 or less.");
            }
            _nodesHiddenLayers.Add(nodes);
        }

        public void createNetwork(bool matlabFile){
            if(_nodesInputLayer <= 0 || _nodesOutputLayer <= 0){
                throw new Exception("Invalid amount of input or output layer nodes");
            }

            //For creating txt
            FileStream outFile = null;
            StreamWriter fileWriterTxt = null;
            //For creating mat-file
            MatFileWriter fileWriterMatlab = null;
            DataBuilder dataBuilder = null;
            IArrayOf<int> array = null;

            //Total number of nodes
            int nodesTotal = _nodesInputLayer + _nodesOutputLayer;
            foreach(int nodes in _nodesHiddenLayers){
                nodesTotal+=nodes;
            }

            try{
                if(!matlabFile){
                    outFile = File.Create(Path.Combine(Directory.GetCurrentDirectory(),"network.nn"));
                    fileWriterTxt = new System.IO.StreamWriter(outFile);
                } else {
                    outFile = new System.IO.FileStream("network.mat", System.IO.FileMode.Create);
                    fileWriterMatlab = new MatFileWriter(outFile);
                    dataBuilder = new DataBuilder();
                    //Create the two dimensional array
                    array = dataBuilder.NewArray<int>(nodesTotal, nodesTotal);
                }

                int layerVertical = 0;
                int layerHorizontal = 0;

                if(!matlabFile){
                    fileWriterTxt.WriteLine("network = [");
                }

                for(int row = 0; row < nodesTotal; row++)
                {
                    layerVertical = currentLayer(row);

                    for(int column = 0; column < nodesTotal; column++){
                        layerHorizontal = currentLayer(column);
                        /*
                        Sets node to one if when following condition is full filed

                              L0 | L1 | L2 | L3 |
                        ----+----+----+----+----+
                         L0 |    | X  |    |
                        ----+----+----+----+----+
                         L1 |    |    | X  |
                        ----+----+----+----+----+
                         L2 |    |    |    | X
                        ----+----+----+----+----+
                         L3 |    |    |    |
                        ----+----+----+----+----+
                         */
                        if(layerVertical == (layerHorizontal - 1)){
                            if(!matlabFile){
                                fileWriterTxt.Write(" 1");
                            } else {
                                array[row,column] = 1;
                            }
                        } else {
                            if(!matlabFile){
                                fileWriterTxt.Write(" 0");
                            } else {
                                array[row,column] = 0;
                            }
                        }
                    }
                    if(!matlabFile && row != nodesTotal){
                        fileWriterTxt.Write(";\n");
                    }
                }
                
                if(!matlabFile){
                    fileWriterTxt.WriteLine("];");
                } else {
                    //Create the variable
                    IVariable network = dataBuilder.NewVariable("network", array);
                    //Create the matfile
                    IMatFile matFile = dataBuilder.NewFile(new[] {network});
                    //Write to the file
                    fileWriterMatlab.Write(matFile);
                }

            }catch(Exception ex){
                Console.WriteLine(ex);
            } finally{
                if(!matlabFile){
                    fileWriterTxt.Dispose();
                }
                outFile.Dispose();
            }
        }

        private int currentLayer(int node){
            //Check if node is in layer 0
            if(node < _nodesInputLayer){
                return 0;
            }

            int nodesLayerBegin = _nodesInputLayer;
            int nodesLayerEnd = _nodesInputLayer;
            int nodesHiddenLayersTotal = 0;


            if(_nodesHiddenLayers != null){
                //Calculate start and end of hiden layer and check if node is in the layer
                int layer = 0;
                foreach(int nodesHiddenLayer in _nodesHiddenLayers){
                    layer++;
                    nodesLayerEnd = nodesLayerBegin + nodesHiddenLayer;
                    nodesHiddenLayersTotal += nodesHiddenLayer;
                    if(node >= nodesLayerBegin && node < nodesLayerEnd){
                        return layer;
                    }
                    nodesLayerBegin = nodesLayerEnd;
                }
            }
            
            int nodesTotal = _nodesInputLayer + nodesHiddenLayersTotal + _nodesOutputLayer;

            //As node is not in layer 0 and not in a hidden layer it must be in the output layer
            if(node >= (nodesTotal - _nodesOutputLayer) && node < nodesTotal){
                if(_nodesHiddenLayers != null){
                    return _nodesHiddenLayers.Count + 1;
                } else {
                    return 1;
                }  
            } else {
                return -1;
            }
        }
    }
}