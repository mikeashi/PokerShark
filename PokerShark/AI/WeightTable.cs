using Newtonsoft.Json;
using PokerShark.Poker;
using PokerShark.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = PokerShark.Poker.Action;

namespace PokerShark.AI
{
    public class TableToJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
                throw new Exception("can not convert card to json");

            if (value is WeightTable table)
                writer.WriteValue(table.ToCSV());
            else
                throw new Exception("can not convert a none card object to json");
        }
    }
    
    internal enum TableCard : int
    {
        // A
        _AA,
        _AKs,
        _AQs,
        _AJs,
        _ATs,
        _A9s,
        _A8s,
        _A7s,
        _A6s,
        _A5s,
        _A4s,
        _A3s,
        _A2s,
        // K
        _AK,
        _KK,
        _KQs,
        _KJs,
        _KTs,
        _K9s,
        _K8s,
        _K7s,
        _K6s,
        _K5s,
        _K4s,
        _K3s,
        _K2s,
        // Q
        _AQ,
        _KQ,
        _QQ,
        _QJs,
        _QTs,
        _Q9s,
        _Q8s,
        _Q7s,
        _Q6s,
        _Q5s,
        _Q4s,
        _Q3s,
        _Q2s,
        // J
        _AJ,
        _KJ,
        _QJ,
        _JJ,
        _JTs,
        _J9s,
        _J8s,
        _J7s,
        _J6s,
        _J5s,
        _J4s,
        _J3s,
        _J2s,
        // T
        _AT,
        _KT,
        _QT,
        _JT,
        _TT,
        _T9s,
        _T8s,
        _T7s,
        _T6s,
        _T5s,
        _T4s,
        _T3s,
        _T2s,
        // 9
        _A9,
        _K9,
        _Q9,
        _J9,
        _T9,
        _99,
        _98s,
        _97s,
        _96s,
        _95s,
        _94s,
        _93s,
        _92s,
        // 8
        _A8,
        _K8,
        _Q8,
        _J8,
        _T8,
        _98,
        _88,
        _87s,
        _86s,
        _85s,
        _84s,
        _83s,
        _82s,
        // 7
        _A7,
        _K7,
        _Q7,
        _J7,
        _T7,
        _97,
        _87,
        _77,
        _76s,
        _75s,
        _74s,
        _73s,
        _72s,
        // 6
        _A6,
        _K6,
        _Q6,
        _J6,
        _T6,
        _96,
        _86,
        _76,
        _66,
        _65s,
        _64s,
        _63s,
        _62s,
        // 5
        _A5,
        _K5,
        _Q5,
        _J5,
        _T5,
        _95,
        _85,
        _75,
        _65,
        _55,
        _54s,
        _53s,
        _52s,
        // 4
        _A4,
        _K4,
        _Q4,
        _J4,
        _T4,
        _94,
        _84,
        _74,
        _64,
        _54,
        _44,
        _43s,
        _42s,
        // 3
        _A3,
        _K3,
        _Q3,
        _J3,
        _T3,
        _93,
        _83,
        _73,
        _63,
        _53,
        _43,
        _33,
        _32s,
        // 2
        _A2,
        _K2,
        _Q2,
        _J2,
        _T2,
        _92,
        _82,
        _72,
        _62,
        _52,
        _42,
        _32,
        _22,
    }

    [JsonConverter(typeof(TableToJsonConverter))]
    public class WeightTable
    {
        #region Properties
        // standard weight table based on card groups.
        private double[] TableMap = new double[13 * 13] { 
            1, 1, 2, 2, 3, 5, 5, 5, 5, 5, 5, 5, 5,
            2, 1, 2, 3, 4, 6, 7, 7, 7, 7, 7, 7, 7,
            3, 4, 1, 3, 4, 5, 7, 9, 9, 9, 9, 9, 9,
            4, 5, 5, 1, 3, 4, 6, 8, 9, 9, 9, 9, 9,
            6, 6, 6, 5, 2, 4, 5, 7, 9, 9, 9, 9, 9,
            8, 8, 8, 7, 7, 3, 4, 5, 8, 9, 9, 9, 9,
            10, 10, 10, 8, 8, 7, 4, 5, 6, 8, 9, 9, 9,
            10, 10, 10, 10, 10, 10, 8, 5, 5, 6, 8, 9, 9,
            10, 10, 10, 10, 10, 10, 10, 8, 6, 5, 7, 9, 9,
            10, 10, 10, 10, 10, 10, 10, 10, 8, 6, 6, 7, 9,
            10, 10, 10, 10, 10, 10, 10, 10, 10, 8, 7, 7, 8,
            10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 7, 8,
            10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 7
        };

        // group weights map
        //private Dictionary<int, double> GroupWeightMap = new Dictionary<int, double>() {
        //    { 1, 1 },
        //    { 2, 0.99 },
        //    { 3, 0.85 },
        //    { 4, 0.80 },
        //    { 5, 0.75 },
        //    { 6, 0.7 },
        //    { 7, 0.6 },
        //    { 8, 0.5 },
        //    { 9, 0.3 },
        //    { 10, 0.2 }
        //};

        private Dictionary<int, double> GroupWeightMap = new Dictionary<int, double>() {
            { 1, 1 },
            { 2, 1 },
            { 3, 1 },
            { 4, 1 },
            { 5, 1 },
            { 6, 1 },
            { 7, 1 },
            { 8, 1 },
            { 9, 1 },
            { 10, 1 }
        };

        // actual weight table
        public double[] Table = new double[13 * 13];

        #endregion

        #region Constructors
        public WeightTable()
        {
            Reset();
        }
        #endregion

        #region Methods
        public void ReceiveAction(PlayerModel model, Action action)
        {
            // reset weight table on each new hand
            if (action.Stage == RoundState.Preflop)
            {
               //Reset();
            }


            var factors = new double[10];

            if (action.Type == ActionType.Call)
            {
                //ReceiveCall(model);
                factors = Test.GetCallFactors(model.VPIP, model.PFF, model.WSD);
            }
            else if (action.Type == ActionType.Raise)
            {
                //ReceiveRaise(model);
                factors = Test.GetCallFactors(model.VPIP, model.PFF, model.WSD);
            }
            else
            {
                // 
                factors = Test.GetFoldFactors(model.VPIP, model.PFF, model.WSD);
            }


            TableMap.CopyTo(Table, 0);
            for (int i = 0; i < 169; i++)
            {
                var n = factors[(int)Table[i] - 1];
                if (n > 1)
                    n = 1;
                if (n < 0.1)
                    n = 0.1;
                Table[i] = n;
            }

        }
        public void Reset()
        {
            // Reset table
            TableMap.CopyTo(Table, 0);
            // replace each group with its weight
            for (int i = 0; i < 169; i++)
            {
                Table[i] = GroupWeightMap[(int)Table[i]];
            }
        }
        public string ToCSV()
        {
            StringBuilder sb = new StringBuilder();
            int j = 1;
            for (int i = 0; i < 169; i++)
            {
                sb.Append(Table[i].ToString(CultureInfo.InvariantCulture));
                if (j == 13)
                {
                    sb.AppendLine();
                    j = 0;
                }
                else
                {
                    sb.Append(",");
                }
                j++;
            }
            return sb.ToString();
        }
        private void ReceiveCall(PlayerModel model)
        {
            List<TableCard> increase = new List<TableCard>();
            if (model.WSD > 50 && model.PFF > 75 && model.PFR > 60 || model.VPIP < 20 )
            {
                // 8% JJ-22,AQs-AJs,KQs,AQo-AJo,KQo
                increase = new List<TableCard>() { TableCard._KQ, TableCard._KQs, TableCard._AJ, TableCard._AJs, TableCard._AQ, TableCard._AQs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, TableCard._JJ };
            }
            else if (model.WSD > 40 && model.PFF > 70 && model.PFR > 50 || model.VPIP < 30)
            {
                // 13% JJ-22,AQs-ATs,KJs+,QJs,JTs,T9s,98s,87s,76s,65s,54s,AQo-ATo,KJo+
                increase = new List<TableCard>() { TableCard._54s, TableCard._65s, TableCard._76s, TableCard._87s, TableCard._98s, TableCard._T9s, TableCard._JTs, TableCard._QJs, TableCard._KJ, TableCard._KJs, TableCard._KQ, TableCard._KQs, TableCard._AT, TableCard._ATs, TableCard._AJ, TableCard._AJs, TableCard._AQ, TableCard._AQs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, TableCard._JJ, };
            }
            else if (model.WSD > 30 &&  model.PFF > 40 && model.PFR > 40 || model.VPIP < 50)
            {
                // 16% TT-22,AJs-A9s,KTs+,QTs+,J9s+,T8s+,98s,87s,76s,65s,54s,AJo-ATo,KTo+,QTo+,JTo
                increase = new List<TableCard>() { TableCard._54s, TableCard._65s, TableCard._76s, TableCard._87s, TableCard._98s, TableCard._T8s, TableCard._T9s, TableCard._J9s, TableCard._JT, TableCard._JTs, TableCard._QT, TableCard._QTs, TableCard._QJ, TableCard._QJs, TableCard._KT, TableCard._KTs, TableCard._KJ, TableCard._KJs, TableCard._KQ, TableCard._KQs, TableCard._A9s, TableCard._AT, TableCard._ATs, TableCard._AJ, TableCard._AJs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, };
            }
            else if (model.WSD > 20 && model.PFF > 40 && model.PFR > 30 || model.VPIP < 70)
            {
                // 22% TT-22,AJs-A2s,K9s+,Q9s+,J9s+,T8s+,97s+,86s+,75s+,64s+,53s+,43s,AJo-A9o,KTo+,QTo+,JTo
                increase = new List<TableCard>() { TableCard._43s, TableCard._53s, TableCard._54s, TableCard._64s, TableCard._65s, TableCard._75s, TableCard._76s, TableCard._86s, TableCard._87s, TableCard._97s, TableCard._98s, TableCard._T8s, TableCard._T9s, TableCard._J9s, TableCard._JT, TableCard._JTs, TableCard._Q9s, TableCard._QT, TableCard._QTs, TableCard._QJ, TableCard._QJs, TableCard._K9s, TableCard._KT, TableCard._KTs, TableCard._KJ, TableCard._KJs, TableCard._KQ, TableCard._KQs, TableCard._A2s, TableCard._A3s, TableCard._A4s, TableCard._A5s, TableCard._A6s, TableCard._A7s, TableCard._A8s, TableCard._A9, TableCard._A9s, TableCard._AT, TableCard._ATs, TableCard._AJ, TableCard._AJs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, };
            }
            else
            {
                // 30% 88-22,ATs-A2s,KJs-K8s,Q8s+,J8s+,T7s+,96s+,85s+,74s+,63s+,53s+,43s,ATo-A7o,KJo-K9o,Q9o+,J9o+,T9o,98o,87o,76o,65o
                increase = new List<TableCard>() { TableCard._43s, TableCard._53s, TableCard._54s, TableCard._63s, TableCard._64s, TableCard._65, TableCard._65s, TableCard._74s, TableCard._75s, TableCard._76, TableCard._76s, TableCard._85s, TableCard._86s, TableCard._87, TableCard._87s, TableCard._96s, TableCard._97s, TableCard._98, TableCard._98s, TableCard._T7s, TableCard._T8s, TableCard._T9, TableCard._T9s, TableCard._J8s, TableCard._J9, TableCard._J9s, TableCard._JT, TableCard._JTs, TableCard._Q8s, TableCard._Q9, TableCard._Q9s, TableCard._QT, TableCard._QTs, TableCard._QJ, TableCard._QJs, TableCard._K8s, TableCard._K9, TableCard._K9s, TableCard._KT, TableCard._KTs, TableCard._KJ, TableCard._KJs, TableCard._A2s, TableCard._A3s, TableCard._A4s, TableCard._A5s, TableCard._A6s, TableCard._A7, TableCard._A7s, TableCard._A8, TableCard._A8s, TableCard._A9, TableCard._A9s, TableCard._AT, TableCard._ATs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, };
            }

            List<TableCard> decrease = Except(increase);
            Increase(increase, 0.3);
            Decrease(decrease, 0.2);
        }
        private void ReceiveRaise(PlayerModel model)
        {
            List<TableCard> increase = new List<TableCard>();
            if (model.WSD > 50 && model.PFF > 75 && model.PFR > 70 || model.VPIP < 10)
            {
                // 9% 66+,AJs+,KQs,AJo+,KQo
                increase = new List<TableCard>() { TableCard._KQ, TableCard._KQs, TableCard._AJ, TableCard._AJs, TableCard._AQ, TableCard._AQs, TableCard._AK, TableCard._AKs, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, TableCard._JJ, TableCard._QQ, TableCard._KK, TableCard._AA, };
            }
            else if (model.WSD > 40 && model.WTSD < 70 && model.PFF > 60 && model.PFR > 60 || model.VPIP < 30)
            {
                // 15% 22+, ATs+, KJs+, QJs, JTs, T9s, 98s, 87s, 76s, 65s, AJo+, KJo+, QJo
                increase = new List<TableCard>() { TableCard._65s, TableCard._76s, TableCard._87s, TableCard._98s, TableCard._T9s, TableCard._JTs, TableCard._QJ, TableCard._QJs, TableCard._KJ, TableCard._KJs, TableCard._KQ, TableCard._KQs, TableCard._ATs, TableCard._AJ, TableCard._AJs, TableCard._AQ, TableCard._AQs, TableCard._AK, TableCard._AKs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, TableCard._JJ, TableCard._QQ, TableCard._KK, TableCard._AA, };
            }
            else if (model.WSD > 20 && model.WTSD < 70 && model.PFF > 55 && model.PFR > 50 || model.VPIP < 60)
            {
                // 20% 22+, ATs+, KTs+, QTs+, J9s+, T8s+, 98s, 87s, 76s, 65s, 54s, ATo+, KTo+, QTo+, JTo
                increase = new List<TableCard>() { TableCard._54s, TableCard._65s, TableCard._76s, TableCard._87s, TableCard._98s, TableCard._T8s, TableCard._T9s, TableCard._J9s, TableCard._JT, TableCard._JTs, TableCard._QT, TableCard._QTs, TableCard._QJ, TableCard._QJs, TableCard._KT, TableCard._KTs, TableCard._KJ, TableCard._KJs, TableCard._KQ, TableCard._KQs, TableCard._AT, TableCard._ATs, TableCard._AJ, TableCard._AJs, TableCard._AQ, TableCard._AQs, TableCard._AK, TableCard._AKs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, TableCard._JJ, TableCard._QQ, TableCard._KK, TableCard._AA, };
            }
            else if (model.WSD > 20 && model.WTSD < 80 && model.PFF > 50 || model.VPIP < 70)
            {
                // 25% 22+,A7s+,K9s+,Q9s+,J9s+,T8s+,97s+,86s+,75s+,64s+,54s,A9o+,KTo+,QTo+,JTo,T9o
                increase = new List<TableCard>() { TableCard._54s, TableCard._64s, TableCard._65s, TableCard._75s, TableCard._76s, TableCard._86s, TableCard._87s, TableCard._97s, TableCard._98s, TableCard._T8s, TableCard._T9, TableCard._T9s, TableCard._J9s, TableCard._JT, TableCard._JTs, TableCard._Q9s, TableCard._QT, TableCard._QTs, TableCard._QJ, TableCard._QJs, TableCard._K9s, TableCard._KT, TableCard._KTs, TableCard._KJ, TableCard._KJs, TableCard._KQ, TableCard._KQs, TableCard._A7s, TableCard._A8s, TableCard._A9, TableCard._A9s, TableCard._AT, TableCard._ATs, TableCard._AJ, TableCard._AJs, TableCard._AQ, TableCard._AQs, TableCard._AK, TableCard._AKs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, TableCard._JJ, TableCard._QQ, TableCard._KK, TableCard._AA, };
            }
            else if (model.WSD > 10 && model.WTSD < 80 && model.PFF > 50 || model.VPIP < 80)
            {
                // 35% 22+,A2s+,K8s+,Q8s+,J8s+,T7s+,97s+,86s+,75s+,64s+,54s,43s,A8o+,A5o-A2o,K9o+,Q9o+,J9o+,T9o
                increase = new List<TableCard>() { TableCard._43s, TableCard._54s, TableCard._64s, TableCard._65s, TableCard._75s, TableCard._76s, TableCard._86s, TableCard._87s, TableCard._97s, TableCard._98s, TableCard._T7s, TableCard._T8s, TableCard._T9, TableCard._T9s, TableCard._J8s, TableCard._J9, TableCard._J9s, TableCard._JT, TableCard._JTs, TableCard._Q8s, TableCard._Q9, TableCard._Q9s, TableCard._QT, TableCard._QTs, TableCard._QJ, TableCard._QJs, TableCard._K8s, TableCard._K9, TableCard._K9s, TableCard._KT, TableCard._KTs, TableCard._KJ, TableCard._KJs, TableCard._KQ, TableCard._KQs, TableCard._A2, TableCard._A2s, TableCard._A3, TableCard._A3s, TableCard._A4, TableCard._A4s, TableCard._A5, TableCard._A5s, TableCard._A6s, TableCard._A7s, TableCard._A8, TableCard._A8s, TableCard._A9, TableCard._A9s, TableCard._AT, TableCard._ATs, TableCard._AJ, TableCard._AJs, TableCard._AQ, TableCard._AQs, TableCard._AK, TableCard._AKs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, TableCard._JJ, TableCard._QQ, TableCard._KK, TableCard._AA, };
            }
            else
            {
                // 50% 22+,A2s+,K2s+,Q7s+,J7s+,T7s+,96s+,86s+,75s+,64s+,53s+,43s,A2o+,K5o+,Q8o+,J8o+,T8o+,98o,87o,76o,65o
                increase = new List<TableCard>() { TableCard._43s, TableCard._53s, TableCard._54s, TableCard._64s, TableCard._65, TableCard._65s, TableCard._75s, TableCard._76, TableCard._76s, TableCard._86s, TableCard._87, TableCard._87s, TableCard._96s, TableCard._97s, TableCard._98, TableCard._98s, TableCard._T7s, TableCard._T8, TableCard._T8s, TableCard._T9, TableCard._T9s, TableCard._J7s, TableCard._J8, TableCard._J8s, TableCard._J9, TableCard._J9s, TableCard._JT, TableCard._JTs, TableCard._Q7s, TableCard._Q8, TableCard._Q8s, TableCard._Q9, TableCard._Q9s, TableCard._QT, TableCard._QTs, TableCard._QJ, TableCard._QJs, TableCard._K2s, TableCard._K3s, TableCard._K4s, TableCard._K5, TableCard._K5s, TableCard._K6, TableCard._K6s, TableCard._K7, TableCard._K7s, TableCard._K8, TableCard._K8s, TableCard._K9, TableCard._K9s, TableCard._KT, TableCard._KTs, TableCard._KJ, TableCard._KJs, TableCard._KQ, TableCard._KQs, TableCard._A2, TableCard._A2s, TableCard._A3, TableCard._A3s, TableCard._A4, TableCard._A4s, TableCard._A5, TableCard._A5s, TableCard._A6, TableCard._A6s, TableCard._A7, TableCard._A7s, TableCard._A8, TableCard._A8s, TableCard._A9, TableCard._A9s, TableCard._AT, TableCard._ATs, TableCard._AJ, TableCard._AJs, TableCard._AQ, TableCard._AQs, TableCard._AK, TableCard._AKs, TableCard._22, TableCard._33, TableCard._44, TableCard._55, TableCard._66, TableCard._77, TableCard._88, TableCard._99, TableCard._TT, TableCard._JJ, TableCard._QQ, TableCard._KK, TableCard._AA, };
            }

            List<TableCard> decrease = Except(increase);
            Increase(increase, 0.3);
            Decrease(decrease, 0.2);
        }
        private List<TableCard> Except(List<TableCard> range)
        {
            List<TableCard> table = Enum.GetValues(typeof(TableCard)).Cast<TableCard>().ToList();
            return table.Except(range).ToList();
        }
        private void Increase(List<TableCard> range, double weight)
        {
            foreach (int card in range)
            {
                Table[card] += weight;
                if (Table[card] >= 1) Table[card] = 1;
            }
        }
        private void Decrease(List<TableCard> range, double weight)
        {
            foreach (int card in range)
            {
                Table[card] -= weight;
                if (Table[card] <= 0) Table[card] = 0.1;
            }
        }
        #endregion
    }
}
