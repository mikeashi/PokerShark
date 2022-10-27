using FluidHTN.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark
{
    internal class Test
    {
        private static Dictionary<int, double[]> VPIP_Call = new Dictionary<int, double[]>()
        {
			{ 100,   new double[]{ 1  ,  1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  1    ,  1    ,  1   }},
			{ 80,    new double[]{ 1  ,  1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  0.7  ,  0.7  ,  0.7 }},
			{ 70,    new double[]{ 1  ,  1  ,  1  ,  1    ,  1    ,  1    ,  0.5  ,  0.5  ,  0.5  ,  0.5 }},
			{ 60,    new double[]{ 1  ,  1  ,  1  ,  1    ,  1    ,  0.5  ,  0.5  ,  0.5  ,  0.5  ,  0.5 }},
			{ 50,    new double[]{ 1  ,  1  ,  1  ,  0.7  ,  0.7  ,  0.4  ,  0.4  ,  0.2  ,  0.2  ,  0.2 }},
        };

        private static Dictionary<int, double[]> VPIP_Fold = new Dictionary<int, double[]>()
        {
            { 100,   new double[]{ 1  ,  1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  1    ,  1    ,  1   }},
            { 80,    new double[]{ 1  ,  1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  0.6  ,  0.6  ,  0.6 }},
            { 70,    new double[]{ 1  ,  1  ,  1  ,  1    ,  1    ,  0.6  ,  0.5  ,  0.5  ,  0.5  ,  0.5 }},
            { 60,    new double[]{ 1  ,  1  ,  1  ,  1    ,  0.8  ,  0.5  ,  0.5  ,  0.5  ,  0.5  ,  0.5 }},
            { 50,    new double[]{ 1  ,  1  ,  1  ,  0.8  ,  0.8  ,  0.4  ,  0.4  ,  0.2  ,  0.2  ,  0.2 }},
        };


        private static Dictionary<int, double[]> PFF_Call = new Dictionary<int, double[]>()
        {
            { 70,    new double[]{ 1  ,  1  ,  1  ,  1  ,  1  ,  0.5  ,  0.2  ,  0.2  ,  0.2  ,  0.2 }},
            { 60,    new double[]{ 1  ,  1  ,  1  ,  1  ,  1  ,  0.5  ,  0.5  ,  0.5  ,  0.5  ,  0.5 }},
            { 40,    new double[]{ 1  ,  1  ,  1  ,  1  ,  1  ,  0.6  ,  0.6  ,  0.6  ,  0.6  ,  0.6 }},
            { 20,    new double[]{ 1  ,  1  ,  1  ,  1  ,  1  ,  0.9  ,  0.9  ,  0.9  ,  0.9  ,  0.9 }},
            { 0 ,    new double[]{ 1  ,  1  ,  1  ,  1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  1   }},
        };

        
        private static Dictionary<int, double[]> PFF_Fold = new Dictionary<int, double[]>()
        {
            { 70,    new double[]{ 1  ,  1  ,  1  ,  1  , 0.8 ,  0.5  ,  0.2  ,  0.2  ,  0.2  ,  0.2 }},
            { 60,    new double[]{ 1  ,  1  ,  1  ,  1  , 0.7 ,  0.5  ,  0.5  ,  0.5  ,  0.5  ,  0.5 }},
            { 40,    new double[]{ 1  ,  1  ,  1  ,  1  , 0.8 ,  0.6  ,  0.6  ,  0.6  ,  0.6  ,  0.6 }},
            { 20,    new double[]{ 1  ,  1  ,  1  ,  1  ,  1  ,  0.9  ,  0.9  ,  0.9  ,  0.9  ,  0.5 }},
            { 0 ,    new double[]{ 1  ,  1  ,  1  ,  1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  0.9 }},
        };


        private static Dictionary<int, double[]> WSD_Call = new Dictionary<int, double[]>()
        {
            { 60,    new double[]{ 1  ,  1  ,  0.8  ,  0.5  ,  0.5  ,  0.2  ,  0.2  ,  0.2  ,  0.2  ,  0.2}},
            { 50,    new double[]{ 1  ,  1  ,  1    ,  0.5  ,  0.5  ,  0.2  ,  0.2  ,  0.2  ,  0.2  ,  0.2}},
            { 40,    new double[]{ 1  ,  1  ,  1    ,  1    ,  1    ,  0.5  ,  0.5  ,  0.2  ,  0.2  ,  0.2}},
            { 10,    new double[]{ 1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  1    ,  1    ,  0.6  ,  0.6}},
            { 0 ,    new double[]{ 1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  1    ,  1    ,  1    ,  1  }},
        };

        private static Dictionary<int, double[]> WSD_Fold = new Dictionary<int, double[]>()
        {
            { 60,    new double[]{ 1  ,  1  ,  0.7  ,  0.5  ,  0.5  ,  0.2  ,  0.2  ,  0.2  ,  0.2  ,  0.2}},
            { 50,    new double[]{ 1  ,  1  ,  1    ,  0.6  ,  0.5  ,  0.2  ,  0.2  ,  0.2  ,  0.2  ,  0.2}},
            { 40,    new double[]{ 1  ,  1  ,  1    ,  1    ,  1    ,  0.5  ,  0.5  ,  0.2  ,  0.2  ,  0.2}},
            { 10,    new double[]{ 1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  1    ,  1    ,  0.6  ,  0.6}},
            { 0 ,    new double[]{ 1  ,  1  ,  1    ,  1    ,  1    ,  1    ,  1    ,  1    ,  1    ,  1  }},
        };


        public static double[] GetCallFactors(double VPIP, double PFF, double WSD)
        {
            var factors = new double[10];
            var vpip = GetFactorsArray((int)VPIP, VPIP_Call);
            var pff = GetFactorsArray((int)PFF, PFF_Call);
            var wwsf = GetFactorsArray((int)WSD, WSD_Call);
            for(int i=0; i<10;i++)
                factors[i] = (vpip[i] + pff[i] + wwsf[i])/3;
            return factors;
        }

        public static double[] GetFoldFactors(double VPIP, double PFF, double WSD)
        {
            var factors = new double[10];
            var vpip = GetFactorsArray((int)VPIP, VPIP_Fold);
            var pff = GetFactorsArray((int)PFF, PFF_Fold);
            var wwsf = GetFactorsArray((int)WSD, WSD_Fold);
            for (int i = 0; i < 10; i++)
                factors[i] = (vpip[i] + pff[i] + wwsf[i]) / 3;
            return factors;
        }


        private static double[] GetFactorsArray(int value, Dictionary<int, double[]> source)
        {
            try { 
                var keys = source.Keys.ToList();
                for(int i=0; i< keys.Count; i++)
                {
                    if (value >= keys[i])
                        return source[keys[i]];
                }
                return source[keys[keys.Count - 1]];
            }catch(Exception e)
            {
                Console.Write(1);
                return new double[1];
            }
           
        }
    }
}
