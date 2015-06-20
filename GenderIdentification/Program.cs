using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GenderIdentification
{
    class Program
    {
        static public connection connect;
        static public getAudioData gad;
        static public genderTraining gt;

        static void Main(string[] args)
        {
            connect = new connection();
            connect.connectTo();

            gad = new getAudioData();
            gad.startGetWave();
            
            //gt = new genderTraining();
            //gt.machineLearning();
        }
    }
}
