using HoldemHand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI
{
    internal class Architect
    {
        //public static double MonteCarloHandStrength(ulong pocket, ulong board, double duration)
        //{
        //    // initialize win counter.
        //    double win = 0.0, count = 0.0;

        //    // store start time.
        //    double starttime = Hand.CurrentTime;

        //    // seed random. 
        //    Random rand = new Random();

        //    // calculate our pocket evaluation
        //    uint ourrank = Hand.Evaluate(pocket | board);


        //    while ((Hand.CurrentTime - starttime) < duration)
        //    {
        //        // for each opponent generate random hand and calculate its rank.

        //        for (int i = 0; i < models.Count; i++)
        //        {
        //            var dead = pocket | board;
        //            // add opponent cards to dead cards.
        //            for (int j = 0; j < i; j++)
        //            {
        //                dead |= oppcards[j];
        //            }
        //            // generate random opponent hand.
        //            oppcards[i] = Hand.RandomHand(rand, 0UL, dead, 2);
        //        }

        //        // calculate opponent ranks.
        //        uint[] oppranks = new uint[models.Count];
        //        for (int i = 0; i < models.Count; i++)
        //        {
        //            oppranks[i] = Hand.Evaluate(oppcards[i] | board);
        //        }

        //        // find best opponent hand.
        //        uint bestopp = oppranks.Max();

        //        // find opponent index
        //        int index = Array.IndexOf(oppranks, bestopp);

        //        // find opponent hand weight
        //        double weight = models[index].WeightTable.Table[(int)HandEvaluator.HandRangeMap[oppcards[index]]];

        //        if (ourrank > bestopp)
        //        {
        //            win += 1.0 * weight;
        //        }
        //        else if (ourrank == bestopp)
        //        {
        //            win += 0.5 * weight;
        //        }
        //        count += 1.0;
        //    }
        //    return win / count;
        //}
    }
}
