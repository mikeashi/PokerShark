using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Deck = PokerShark.Core.Poker.Deck;
using HoldemHand;
using MathNet.Numerics.Integration;
using PokerShark.Core.HTN;
using PokerShark.Core.PyPoker;
using Serilog;
using PokerShark.Core.Poker;

namespace PokerShark.Core.Helpers
{
    public class EvaluatorHelper
    {
        #region Weights
        static Dictionary<ulong, TableCard> HandRangeMap = new Dictionary<ulong, TableCard>() {
                { 0xC000000000000        , TableCard._AKs       },  // AKs
                { 0xA000000000000        , TableCard._AQs       },  // AQs
                { 0x9000000000000        , TableCard._AJs       },  // AJs
                { 0x8800000000000        , TableCard._ATs       },  // ATs
                { 0x8400000000000        , TableCard._A9s       },  // A9s
                { 0x8200000000000        , TableCard._A8s       },  // A8s
                { 0x8100000000000        , TableCard._A7s       },  // A7s
                { 0x8080000000000        , TableCard._A6s       },  // A6s
                { 0x8040000000000        , TableCard._A5s       },  // A5s
                { 0x8020000000000        , TableCard._A4s       },  // A4s
                { 0x8010000000000        , TableCard._A3s       },  // A3s
                { 0x8008000000000        , TableCard._A2s       },  // A2s
                { 0x8004000000000        , TableCard._AA        },  // AA
                { 0x8002000000000        , TableCard._AK        },  // AK
                { 0x8001000000000        , TableCard._AQ        },  // AQ
                { 0x8000800000000        , TableCard._AJ        },  // AJ
                { 0x8000400000000        , TableCard._AT        },  // AT
                { 0x8000200000000        , TableCard._A9        },  // A9
                { 0x8000100000000        , TableCard._A8        },  // A8
                { 0x8000080000000        , TableCard._A7        },  // A7
                { 0x8000040000000        , TableCard._A6        },  // A6
                { 0x8000020000000        , TableCard._A5        },  // A5
                { 0x8000010000000        , TableCard._A4        },  // A4
                { 0x8000008000000        , TableCard._A3        },  // A3
                { 0x8000004000000        , TableCard._A2        },  // A2
                { 0x8000002000000        , TableCard._AA        },  // AA
                { 0x8000001000000        , TableCard._AK        },  // AK
                { 0x8000000800000        , TableCard._AQ        },  // AQ
                { 0x8000000400000        , TableCard._AJ        },  // AJ
                { 0x8000000200000        , TableCard._AT        },  // AT
                { 0x8000000100000        , TableCard._A9        },  // A9
                { 0x8000000080000        , TableCard._A8        },  // A8
                { 0x8000000040000        , TableCard._A7        },  // A7
                { 0x8000000020000        , TableCard._A6        },  // A6
                { 0x8000000010000        , TableCard._A5        },  // A5
                { 0x8000000008000        , TableCard._A4        },  // A4
                { 0x8000000004000        , TableCard._A3        },  // A3
                { 0x8000000002000        , TableCard._A2        },  // A2
                { 0x8000000001000        , TableCard._AA        },  // AA
                { 0x8000000000800        , TableCard._AK        },  // AK
                { 0x8000000000400        , TableCard._AQ        },  // AQ
                { 0x8000000000200        , TableCard._AJ        },  // AJ
                { 0x8000000000100        , TableCard._AT        },  // AT
                { 0x8000000000080        , TableCard._A9        },  // A9
                { 0x8000000000040        , TableCard._A8        },  // A8
                { 0x8000000000020        , TableCard._A7        },  // A7
                { 0x8000000000010        , TableCard._A6        },  // A6
                { 0x8000000000008        , TableCard._A5        },  // A5
                { 0x8000000000004        , TableCard._A4        },  // A4
                { 0x8000000000002        , TableCard._A3        },  // A3
                { 0x8000000000001        , TableCard._A2        },  // A2
                { 0x6000000000000        , TableCard._KQs       },  // KQs
                { 0x5000000000000        , TableCard._KJs       },  // KJs
                { 0x4800000000000        , TableCard._KTs       },  // KTs
                { 0x4400000000000        , TableCard._K9s       },  // K9s
                { 0x4200000000000        , TableCard._K8s       },  // K8s
                { 0x4100000000000        , TableCard._K7s       },  // K7s
                { 0x4080000000000        , TableCard._K6s       },  // K6s
                { 0x4040000000000        , TableCard._K5s       },  // K5s
                { 0x4020000000000        , TableCard._K4s       },  // K4s
                { 0x4010000000000        , TableCard._K3s       },  // K3s
                { 0x4008000000000        , TableCard._K2s       },  // K2s
                { 0x4004000000000        , TableCard._AK        },  // AK
                { 0x4002000000000        , TableCard._KK        },  // KK
                { 0x4001000000000        , TableCard._KQ        },  // KQ
                { 0x4000800000000        , TableCard._KJ        },  // KJ
                { 0x4000400000000        , TableCard._KT        },  // KT
                { 0x4000200000000        , TableCard._K9        },  // K9
                { 0x4000100000000        , TableCard._K8        },  // K8
                { 0x4000080000000        , TableCard._K7        },  // K7
                { 0x4000040000000        , TableCard._K6        },  // K6
                { 0x4000020000000        , TableCard._K5        },  // K5
                { 0x4000010000000        , TableCard._K4        },  // K4
                { 0x4000008000000        , TableCard._K3        },  // K3
                { 0x4000004000000        , TableCard._K2        },  // K2
                { 0x4000002000000        , TableCard._AK        },  // AK
                { 0x4000001000000        , TableCard._KK        },  // KK
                { 0x4000000800000        , TableCard._KQ        },  // KQ
                { 0x4000000400000        , TableCard._KJ        },  // KJ
                { 0x4000000200000        , TableCard._KT        },  // KT
                { 0x4000000100000        , TableCard._K9        },  // K9
                { 0x4000000080000        , TableCard._K8        },  // K8
                { 0x4000000040000        , TableCard._K7        },  // K7
                { 0x4000000020000        , TableCard._K6        },  // K6
                { 0x4000000010000        , TableCard._K5        },  // K5
                { 0x4000000008000        , TableCard._K4        },  // K4
                { 0x4000000004000        , TableCard._K3        },  // K3
                { 0x4000000002000        , TableCard._K2        },  // K2
                { 0x4000000001000        , TableCard._AK        },  // AK
                { 0x4000000000800        , TableCard._KK        },  // KK
                { 0x4000000000400        , TableCard._KQ        },  // KQ
                { 0x4000000000200        , TableCard._KJ        },  // KJ
                { 0x4000000000100        , TableCard._KT        },  // KT
                { 0x4000000000080        , TableCard._K9        },  // K9
                { 0x4000000000040        , TableCard._K8        },  // K8
                { 0x4000000000020        , TableCard._K7        },  // K7
                { 0x4000000000010        , TableCard._K6        },  // K6
                { 0x4000000000008        , TableCard._K5        },  // K5
                { 0x4000000000004        , TableCard._K4        },  // K4
                { 0x4000000000002        , TableCard._K3        },  // K3
                { 0x4000000000001        , TableCard._K2        },  // K2
                { 0x3000000000000        , TableCard._QJs       },  // QJs
                { 0x2800000000000        , TableCard._QTs       },  // QTs
                { 0x2400000000000        , TableCard._Q9s       },  // Q9s
                { 0x2200000000000        , TableCard._Q8s       },  // Q8s
                { 0x2100000000000        , TableCard._Q7s       },  // Q7s
                { 0x2080000000000        , TableCard._Q6s       },  // Q6s
                { 0x2040000000000        , TableCard._Q5s       },  // Q5s
                { 0x2020000000000        , TableCard._Q4s       },  // Q4s
                { 0x2010000000000        , TableCard._Q3s       },  // Q3s
                { 0x2008000000000        , TableCard._Q2s       },  // Q2s
                { 0x2004000000000        , TableCard._AQ        },  // AQ
                { 0x2002000000000        , TableCard._KQ        },  // KQ
                { 0x2001000000000        , TableCard._QQ        },  // QQ
                { 0x2000800000000        , TableCard._QJ        },  // QJ
                { 0x2000400000000        , TableCard._QT        },  // QT
                { 0x2000200000000        , TableCard._Q9        },  // Q9
                { 0x2000100000000        , TableCard._Q8        },  // Q8
                { 0x2000080000000        , TableCard._Q7        },  // Q7
                { 0x2000040000000        , TableCard._Q6        },  // Q6
                { 0x2000020000000        , TableCard._Q5        },  // Q5
                { 0x2000010000000        , TableCard._Q4        },  // Q4
                { 0x2000008000000        , TableCard._Q3        },  // Q3
                { 0x2000004000000        , TableCard._Q2        },  // Q2
                { 0x2000002000000        , TableCard._AQ        },  // AQ
                { 0x2000001000000        , TableCard._KQ        },  // KQ
                { 0x2000000800000        , TableCard._QQ        },  // QQ
                { 0x2000000400000        , TableCard._QJ        },  // QJ
                { 0x2000000200000        , TableCard._QT        },  // QT
                { 0x2000000100000        , TableCard._Q9        },  // Q9
                { 0x2000000080000        , TableCard._Q8        },  // Q8
                { 0x2000000040000        , TableCard._Q7        },  // Q7
                { 0x2000000020000        , TableCard._Q6        },  // Q6
                { 0x2000000010000        , TableCard._Q5        },  // Q5
                { 0x2000000008000        , TableCard._Q4        },  // Q4
                { 0x2000000004000        , TableCard._Q3        },  // Q3
                { 0x2000000002000        , TableCard._Q2        },  // Q2
                { 0x2000000001000        , TableCard._AQ        },  // AQ
                { 0x2000000000800        , TableCard._KQ        },  // KQ
                { 0x2000000000400        , TableCard._QQ        },  // QQ
                { 0x2000000000200        , TableCard._QJ        },  // QJ
                { 0x2000000000100        , TableCard._QT        },  // QT
                { 0x2000000000080        , TableCard._Q9        },  // Q9
                { 0x2000000000040        , TableCard._Q8        },  // Q8
                { 0x2000000000020        , TableCard._Q7        },  // Q7
                { 0x2000000000010        , TableCard._Q6        },  // Q6
                { 0x2000000000008        , TableCard._Q5        },  // Q5
                { 0x2000000000004        , TableCard._Q4        },  // Q4
                { 0x2000000000002        , TableCard._Q3        },  // Q3
                { 0x2000000000001        , TableCard._Q2        },  // Q2
                { 0x1800000000000        , TableCard._JTs       },  // JTs
                { 0x1400000000000        , TableCard._J9s       },  // J9s
                { 0x1200000000000        , TableCard._J8s       },  // J8s
                { 0x1100000000000        , TableCard._J7s       },  // J7s
                { 0x1080000000000        , TableCard._J6s       },  // J6s
                { 0x1040000000000        , TableCard._J5s       },  // J5s
                { 0x1020000000000        , TableCard._J4s       },  // J4s
                { 0x1010000000000        , TableCard._J3s       },  // J3s
                { 0x1008000000000        , TableCard._J2s       },  // J2s
                { 0x1004000000000        , TableCard._AJ        },  // AJ
                { 0x1002000000000        , TableCard._KJ        },  // KJ
                { 0x1001000000000        , TableCard._QJ        },  // QJ
                { 0x1000800000000        , TableCard._JJ        },  // JJ
                { 0x1000400000000        , TableCard._JT        },  // JT
                { 0x1000200000000        , TableCard._J9        },  // J9
                { 0x1000100000000        , TableCard._J8        },  // J8
                { 0x1000080000000        , TableCard._J7        },  // J7
                { 0x1000040000000        , TableCard._J6        },  // J6
                { 0x1000020000000        , TableCard._J5        },  // J5
                { 0x1000010000000        , TableCard._J4        },  // J4
                { 0x1000008000000        , TableCard._J3        },  // J3
                { 0x1000004000000        , TableCard._J2        },  // J2
                { 0x1000002000000        , TableCard._AJ        },  // AJ
                { 0x1000001000000        , TableCard._KJ        },  // KJ
                { 0x1000000800000        , TableCard._QJ        },  // QJ
                { 0x1000000400000        , TableCard._JJ        },  // JJ
                { 0x1000000200000        , TableCard._JT        },  // JT
                { 0x1000000100000        , TableCard._J9        },  // J9
                { 0x1000000080000        , TableCard._J8        },  // J8
                { 0x1000000040000        , TableCard._J7        },  // J7
                { 0x1000000020000        , TableCard._J6        },  // J6
                { 0x1000000010000        , TableCard._J5        },  // J5
                { 0x1000000008000        , TableCard._J4        },  // J4
                { 0x1000000004000        , TableCard._J3        },  // J3
                { 0x1000000002000        , TableCard._J2        },  // J2
                { 0x1000000001000        , TableCard._AJ        },  // AJ
                { 0x1000000000800        , TableCard._KJ        },  // KJ
                { 0x1000000000400        , TableCard._QJ        },  // QJ
                { 0x1000000000200        , TableCard._JJ        },  // JJ
                { 0x1000000000100        , TableCard._JT        },  // JT
                { 0x1000000000080        , TableCard._J9        },  // J9
                { 0x1000000000040        , TableCard._J8        },  // J8
                { 0x1000000000020        , TableCard._J7        },  // J7
                { 0x1000000000010        , TableCard._J6        },  // J6
                { 0x1000000000008        , TableCard._J5        },  // J5
                { 0x1000000000004        , TableCard._J4        },  // J4
                { 0x1000000000002        , TableCard._J3        },  // J3
                { 0x1000000000001        , TableCard._J2        },  // J2
                { 0xC00000000000         , TableCard._T9s       },  // T9s
                { 0xA00000000000         , TableCard._T8s       },  // T8s
                { 0x900000000000         , TableCard._T7s       },  // T7s
                { 0x880000000000         , TableCard._T6s       },  // T6s
                { 0x840000000000         , TableCard._T5s       },  // T5s
                { 0x820000000000         , TableCard._T4s       },  // T4s
                { 0x810000000000         , TableCard._T3s       },  // T3s
                { 0x808000000000         , TableCard._T2s       },  // T2s
                { 0x804000000000         , TableCard._AT        },  // AT
                { 0x802000000000         , TableCard._KT        },  // KT
                { 0x801000000000         , TableCard._QT        },  // QT
                { 0x800800000000         , TableCard._JT        },  // JT
                { 0x800400000000         , TableCard._TT        },  // TT
                { 0x800200000000         , TableCard._T9        },  // T9
                { 0x800100000000         , TableCard._T8        },  // T8
                { 0x800080000000         , TableCard._T7        },  // T7
                { 0x800040000000         , TableCard._T6        },  // T6
                { 0x800020000000         , TableCard._T5        },  // T5
                { 0x800010000000         , TableCard._T4        },  // T4
                { 0x800008000000         , TableCard._T3        },  // T3
                { 0x800004000000         , TableCard._T2        },  // T2
                { 0x800002000000         , TableCard._AT        },  // AT
                { 0x800001000000         , TableCard._KT        },  // KT
                { 0x800000800000         , TableCard._QT        },  // QT
                { 0x800000400000         , TableCard._JT        },  // JT
                { 0x800000200000         , TableCard._TT        },  // TT
                { 0x800000100000         , TableCard._T9        },  // T9
                { 0x800000080000         , TableCard._T8        },  // T8
                { 0x800000040000         , TableCard._T7        },  // T7
                { 0x800000020000         , TableCard._T6        },  // T6
                { 0x800000010000         , TableCard._T5        },  // T5
                { 0x800000008000         , TableCard._T4        },  // T4
                { 0x800000004000         , TableCard._T3        },  // T3
                { 0x800000002000         , TableCard._T2        },  // T2
                { 0x800000001000         , TableCard._AT        },  // AT
                { 0x800000000800         , TableCard._KT        },  // KT
                { 0x800000000400         , TableCard._QT        },  // QT
                { 0x800000000200         , TableCard._JT        },  // JT
                { 0x800000000100         , TableCard._TT        },  // TT
                { 0x800000000080         , TableCard._T9        },  // T9
                { 0x800000000040         , TableCard._T8        },  // T8
                { 0x800000000020         , TableCard._T7        },  // T7
                { 0x800000000010         , TableCard._T6        },  // T6
                { 0x800000000008         , TableCard._T5        },  // T5
                { 0x800000000004         , TableCard._T4        },  // T4
                { 0x800000000002         , TableCard._T3        },  // T3
                { 0x800000000001         , TableCard._T2        },  // T2
                { 0x600000000000         , TableCard._98s       },  // 98s
                { 0x500000000000         , TableCard._97s       },  // 97s
                { 0x480000000000         , TableCard._96s       },  // 96s
                { 0x440000000000         , TableCard._95s       },  // 95s
                { 0x420000000000         , TableCard._94s       },  // 94s
                { 0x410000000000         , TableCard._93s       },  // 93s
                { 0x408000000000         , TableCard._92s       },  // 92s
                { 0x404000000000         , TableCard._A9        },  // A9
                { 0x402000000000         , TableCard._K9        },  // K9
                { 0x401000000000         , TableCard._Q9        },  // Q9
                { 0x400800000000         , TableCard._J9        },  // J9
                { 0x400400000000         , TableCard._T9        },  // T9
                { 0x400200000000         , TableCard._99        },  // 99
                { 0x400100000000         , TableCard._98        },  // 98
                { 0x400080000000         , TableCard._97        },  // 97
                { 0x400040000000         , TableCard._96        },  // 96
                { 0x400020000000         , TableCard._95        },  // 95
                { 0x400010000000         , TableCard._94        },  // 94
                { 0x400008000000         , TableCard._93        },  // 93
                { 0x400004000000         , TableCard._92        },  // 92
                { 0x400002000000         , TableCard._A9        },  // A9
                { 0x400001000000         , TableCard._K9        },  // K9
                { 0x400000800000         , TableCard._Q9        },  // Q9
                { 0x400000400000         , TableCard._J9        },  // J9
                { 0x400000200000         , TableCard._T9        },  // T9
                { 0x400000100000         , TableCard._99        },  // 99
                { 0x400000080000         , TableCard._98        },  // 98
                { 0x400000040000         , TableCard._97        },  // 97
                { 0x400000020000         , TableCard._96        },  // 96
                { 0x400000010000         , TableCard._95        },  // 95
                { 0x400000008000         , TableCard._94        },  // 94
                { 0x400000004000         , TableCard._93        },  // 93
                { 0x400000002000         , TableCard._92        },  // 92
                { 0x400000001000         , TableCard._A9        },  // A9
                { 0x400000000800         , TableCard._K9        },  // K9
                { 0x400000000400         , TableCard._Q9        },  // Q9
                { 0x400000000200         , TableCard._J9        },  // J9
                { 0x400000000100         , TableCard._T9        },  // T9
                { 0x400000000080         , TableCard._99        },  // 99
                { 0x400000000040         , TableCard._98        },  // 98
                { 0x400000000020         , TableCard._97        },  // 97
                { 0x400000000010         , TableCard._96        },  // 96
                { 0x400000000008         , TableCard._95        },  // 95
                { 0x400000000004         , TableCard._94        },  // 94
                { 0x400000000002         , TableCard._93        },  // 93
                { 0x400000000001         , TableCard._92        },  // 92
                { 0x300000000000         , TableCard._87s       },  // 87s
                { 0x280000000000         , TableCard._86s       },  // 86s
                { 0x240000000000         , TableCard._85s       },  // 85s
                { 0x220000000000         , TableCard._84s       },  // 84s
                { 0x210000000000         , TableCard._83s       },  // 83s
                { 0x208000000000         , TableCard._82s       },  // 82s
                { 0x204000000000         , TableCard._A8        },  // A8
                { 0x202000000000         , TableCard._K8        },  // K8
                { 0x201000000000         , TableCard._Q8        },  // Q8
                { 0x200800000000         , TableCard._J8        },  // J8
                { 0x200400000000         , TableCard._T8        },  // T8
                { 0x200200000000         , TableCard._98        },  // 98
                { 0x200100000000         , TableCard._88        },  // 88
                { 0x200080000000         , TableCard._87        },  // 87
                { 0x200040000000         , TableCard._86        },  // 86
                { 0x200020000000         , TableCard._85        },  // 85
                { 0x200010000000         , TableCard._84        },  // 84
                { 0x200008000000         , TableCard._83        },  // 83
                { 0x200004000000         , TableCard._82        },  // 82
                { 0x200002000000         , TableCard._A8        },  // A8
                { 0x200001000000         , TableCard._K8        },  // K8
                { 0x200000800000         , TableCard._Q8        },  // Q8
                { 0x200000400000         , TableCard._J8        },  // J8
                { 0x200000200000         , TableCard._T8        },  // T8
                { 0x200000100000         , TableCard._98        },  // 98
                { 0x200000080000         , TableCard._88        },  // 88
                { 0x200000040000         , TableCard._87        },  // 87
                { 0x200000020000         , TableCard._86        },  // 86
                { 0x200000010000         , TableCard._85        },  // 85
                { 0x200000008000         , TableCard._84        },  // 84
                { 0x200000004000         , TableCard._83        },  // 83
                { 0x200000002000         , TableCard._82        },  // 82
                { 0x200000001000         , TableCard._A8        },  // A8
                { 0x200000000800         , TableCard._K8        },  // K8
                { 0x200000000400         , TableCard._Q8        },  // Q8
                { 0x200000000200         , TableCard._J8        },  // J8
                { 0x200000000100         , TableCard._T8        },  // T8
                { 0x200000000080         , TableCard._98        },  // 98
                { 0x200000000040         , TableCard._88        },  // 88
                { 0x200000000020         , TableCard._87        },  // 87
                { 0x200000000010         , TableCard._86        },  // 86
                { 0x200000000008         , TableCard._85        },  // 85
                { 0x200000000004         , TableCard._84        },  // 84
                { 0x200000000002         , TableCard._83        },  // 83
                { 0x200000000001         , TableCard._82        },  // 82
                { 0x180000000000         , TableCard._76s       },  // 76s
                { 0x140000000000         , TableCard._75s       },  // 75s
                { 0x120000000000         , TableCard._74s       },  // 74s
                { 0x110000000000         , TableCard._73s       },  // 73s
                { 0x108000000000         , TableCard._72s       },  // 72s
                { 0x104000000000         , TableCard._A7        },  // A7
                { 0x102000000000         , TableCard._K7        },  // K7
                { 0x101000000000         , TableCard._Q7        },  // Q7
                { 0x100800000000         , TableCard._J7        },  // J7
                { 0x100400000000         , TableCard._T7        },  // T7
                { 0x100200000000         , TableCard._97        },  // 97
                { 0x100100000000         , TableCard._87        },  // 87
                { 0x100080000000         , TableCard._77        },  // 77
                { 0x100040000000         , TableCard._76        },  // 76
                { 0x100020000000         , TableCard._75        },  // 75
                { 0x100010000000         , TableCard._74        },  // 74
                { 0x100008000000         , TableCard._73        },  // 73
                { 0x100004000000         , TableCard._72        },  // 72
                { 0x100002000000         , TableCard._A7        },  // A7
                { 0x100001000000         , TableCard._K7        },  // K7
                { 0x100000800000         , TableCard._Q7        },  // Q7
                { 0x100000400000         , TableCard._J7        },  // J7
                { 0x100000200000         , TableCard._T7        },  // T7
                { 0x100000100000         , TableCard._97        },  // 97
                { 0x100000080000         , TableCard._87        },  // 87
                { 0x100000040000         , TableCard._77        },  // 77
                { 0x100000020000         , TableCard._76        },  // 76
                { 0x100000010000         , TableCard._75        },  // 75
                { 0x100000008000         , TableCard._74        },  // 74
                { 0x100000004000         , TableCard._73        },  // 73
                { 0x100000002000         , TableCard._72        },  // 72
                { 0x100000001000         , TableCard._A7        },  // A7
                { 0x100000000800         , TableCard._K7        },  // K7
                { 0x100000000400         , TableCard._Q7        },  // Q7
                { 0x100000000200         , TableCard._J7        },  // J7
                { 0x100000000100         , TableCard._T7        },  // T7
                { 0x100000000080         , TableCard._97        },  // 97
                { 0x100000000040         , TableCard._87        },  // 87
                { 0x100000000020         , TableCard._77        },  // 77
                { 0x100000000010         , TableCard._76        },  // 76
                { 0x100000000008         , TableCard._75        },  // 75
                { 0x100000000004         , TableCard._74        },  // 74
                { 0x100000000002         , TableCard._73        },  // 73
                { 0x100000000001         , TableCard._72        },  // 72
                { 0x0C0000000000         , TableCard._65s       },  // 65s
                { 0x0A0000000000         , TableCard._64s       },  // 64s
                { 0x090000000000         , TableCard._63s       },  // 63s
                { 0x088000000000         , TableCard._62s       },  // 62s
                { 0x084000000000         , TableCard._A6        },  // A6
                { 0x082000000000         , TableCard._K6        },  // K6
                { 0x081000000000         , TableCard._Q6        },  // Q6
                { 0x080800000000         , TableCard._J6        },  // J6
                { 0x080400000000         , TableCard._T6        },  // T6
                { 0x080200000000         , TableCard._96        },  // 96
                { 0x080100000000         , TableCard._86        },  // 86
                { 0x080080000000         , TableCard._76        },  // 76
                { 0x080040000000         , TableCard._66        },  // 66
                { 0x080020000000         , TableCard._65        },  // 65
                { 0x080010000000         , TableCard._64        },  // 64
                { 0x080008000000         , TableCard._63        },  // 63
                { 0x080004000000         , TableCard._62        },  // 62
                { 0x080002000000         , TableCard._A6        },  // A6
                { 0x080001000000         , TableCard._K6        },  // K6
                { 0x080000800000         , TableCard._Q6        },  // Q6
                { 0x080000400000         , TableCard._J6        },  // J6
                { 0x080000200000         , TableCard._T6        },  // T6
                { 0x080000100000         , TableCard._96        },  // 96
                { 0x080000080000         , TableCard._86        },  // 86
                { 0x080000040000         , TableCard._76        },  // 76
                { 0x080000020000         , TableCard._66        },  // 66
                { 0x080000010000         , TableCard._65        },  // 65
                { 0x080000008000         , TableCard._64        },  // 64
                { 0x080000004000         , TableCard._63        },  // 63
                { 0x080000002000         , TableCard._62        },  // 62
                { 0x080000001000         , TableCard._A6        },  // A6
                { 0x080000000800         , TableCard._K6        },  // K6
                { 0x080000000400         , TableCard._Q6        },  // Q6
                { 0x080000000200         , TableCard._J6        },  // J6
                { 0x080000000100         , TableCard._T6        },  // T6
                { 0x080000000080         , TableCard._96        },  // 96
                { 0x080000000040         , TableCard._86        },  // 86
                { 0x080000000020         , TableCard._76        },  // 76
                { 0x080000000010         , TableCard._66        },  // 66
                { 0x080000000008         , TableCard._65        },  // 65
                { 0x080000000004         , TableCard._64        },  // 64
                { 0x080000000002         , TableCard._63        },  // 63
                { 0x080000000001         , TableCard._62        },  // 62
                { 0x060000000000         , TableCard._54s       },  // 54s
                { 0x050000000000         , TableCard._53s       },  // 53s
                { 0x048000000000         , TableCard._52s       },  // 52s
                { 0x044000000000         , TableCard._A5        },  // A5
                { 0x042000000000         , TableCard._K5        },  // K5
                { 0x041000000000         , TableCard._Q5        },  // Q5
                { 0x040800000000         , TableCard._J5        },  // J5
                { 0x040400000000         , TableCard._T5        },  // T5
                { 0x040200000000         , TableCard._95        },  // 95
                { 0x040100000000         , TableCard._85        },  // 85
                { 0x040080000000         , TableCard._75        },  // 75
                { 0x040040000000         , TableCard._65        },  // 65
                { 0x040020000000         , TableCard._55        },  // 55
                { 0x040010000000         , TableCard._54        },  // 54
                { 0x040008000000         , TableCard._53        },  // 53
                { 0x040004000000         , TableCard._52        },  // 52
                { 0x040002000000         , TableCard._A5        },  // A5
                { 0x040001000000         , TableCard._K5        },  // K5
                { 0x040000800000         , TableCard._Q5        },  // Q5
                { 0x040000400000         , TableCard._J5        },  // J5
                { 0x040000200000         , TableCard._T5        },  // T5
                { 0x040000100000         , TableCard._95        },  // 95
                { 0x040000080000         , TableCard._85        },  // 85
                { 0x040000040000         , TableCard._75        },  // 75
                { 0x040000020000         , TableCard._65        },  // 65
                { 0x040000010000         , TableCard._55        },  // 55
                { 0x040000008000         , TableCard._54        },  // 54
                { 0x040000004000         , TableCard._53        },  // 53
                { 0x040000002000         , TableCard._52        },  // 52
                { 0x040000001000         , TableCard._A5        },  // A5
                { 0x040000000800         , TableCard._K5        },  // K5
                { 0x040000000400         , TableCard._Q5        },  // Q5
                { 0x040000000200         , TableCard._J5        },  // J5
                { 0x040000000100         , TableCard._T5        },  // T5
                { 0x040000000080         , TableCard._95        },  // 95
                { 0x040000000040         , TableCard._85        },  // 85
                { 0x040000000020         , TableCard._75        },  // 75
                { 0x040000000010         , TableCard._65        },  // 65
                { 0x040000000008         , TableCard._55        },  // 55
                { 0x040000000004         , TableCard._54        },  // 54
                { 0x040000000002         , TableCard._53        },  // 53
                { 0x040000000001         , TableCard._52        },  // 52
                { 0x030000000000         , TableCard._43s       },  // 43s
                { 0x028000000000         , TableCard._42s       },  // 42s
                { 0x024000000000         , TableCard._A4        },  // A4
                { 0x022000000000         , TableCard._K4        },  // K4
                { 0x021000000000         , TableCard._Q4        },  // Q4
                { 0x020800000000         , TableCard._J4        },  // J4
                { 0x020400000000         , TableCard._T4        },  // T4
                { 0x020200000000         , TableCard._94        },  // 94
                { 0x020100000000         , TableCard._84        },  // 84
                { 0x020080000000         , TableCard._74        },  // 74
                { 0x020040000000         , TableCard._64        },  // 64
                { 0x020020000000         , TableCard._54        },  // 54
                { 0x020010000000         , TableCard._44        },  // 44
                { 0x020008000000         , TableCard._43        },  // 43
                { 0x020004000000         , TableCard._42        },  // 42
                { 0x020002000000         , TableCard._A4        },  // A4
                { 0x020001000000         , TableCard._K4        },  // K4
                { 0x020000800000         , TableCard._Q4        },  // Q4
                { 0x020000400000         , TableCard._J4        },  // J4
                { 0x020000200000         , TableCard._T4        },  // T4
                { 0x020000100000         , TableCard._94        },  // 94
                { 0x020000080000         , TableCard._84        },  // 84
                { 0x020000040000         , TableCard._74        },  // 74
                { 0x020000020000         , TableCard._64        },  // 64
                { 0x020000010000         , TableCard._54        },  // 54
                { 0x020000008000         , TableCard._44        },  // 44
                { 0x020000004000         , TableCard._43        },  // 43
                { 0x020000002000         , TableCard._42        },  // 42
                { 0x020000001000         , TableCard._A4        },  // A4
                { 0x020000000800         , TableCard._K4        },  // K4
                { 0x020000000400         , TableCard._Q4        },  // Q4
                { 0x020000000200         , TableCard._J4        },  // J4
                { 0x020000000100         , TableCard._T4        },  // T4
                { 0x020000000080         , TableCard._94        },  // 94
                { 0x020000000040         , TableCard._84        },  // 84
                { 0x020000000020         , TableCard._74        },  // 74
                { 0x020000000010         , TableCard._64        },  // 64
                { 0x020000000008         , TableCard._54        },  // 54
                { 0x020000000004         , TableCard._44        },  // 44
                { 0x020000000002         , TableCard._43        },  // 43
                { 0x020000000001         , TableCard._42        },  // 42
                { 0x018000000000         , TableCard._32s       },  // 32s
                { 0x014000000000         , TableCard._A3        },  // A3
                { 0x012000000000         , TableCard._K3        },  // K3
                { 0x011000000000         , TableCard._Q3        },  // Q3
                { 0x010800000000         , TableCard._J3        },  // J3
                { 0x010400000000         , TableCard._T3        },  // T3
                { 0x010200000000         , TableCard._93        },  // 93
                { 0x010100000000         , TableCard._83        },  // 83
                { 0x010080000000         , TableCard._73        },  // 73
                { 0x010040000000         , TableCard._63        },  // 63
                { 0x010020000000         , TableCard._53        },  // 53
                { 0x010010000000         , TableCard._43        },  // 43
                { 0x010008000000         , TableCard._33        },  // 33
                { 0x010004000000         , TableCard._32        },  // 32
                { 0x010002000000         , TableCard._A3        },  // A3
                { 0x010001000000         , TableCard._K3        },  // K3
                { 0x010000800000         , TableCard._Q3        },  // Q3
                { 0x010000400000         , TableCard._J3        },  // J3
                { 0x010000200000         , TableCard._T3        },  // T3
                { 0x010000100000         , TableCard._93        },  // 93
                { 0x010000080000         , TableCard._83        },  // 83
                { 0x010000040000         , TableCard._73        },  // 73
                { 0x010000020000         , TableCard._63        },  // 63
                { 0x010000010000         , TableCard._53        },  // 53
                { 0x010000008000         , TableCard._43        },  // 43
                { 0x010000004000         , TableCard._33        },  // 33
                { 0x010000002000         , TableCard._32        },  // 32
                { 0x010000001000         , TableCard._A3        },  // A3
                { 0x010000000800         , TableCard._K3        },  // K3
                { 0x010000000400         , TableCard._Q3        },  // Q3
                { 0x010000000200         , TableCard._J3        },  // J3
                { 0x010000000100         , TableCard._T3        },  // T3
                { 0x010000000080         , TableCard._93        },  // 93
                { 0x010000000040         , TableCard._83        },  // 83
                { 0x010000000020         , TableCard._73        },  // 73
                { 0x010000000010         , TableCard._63        },  // 63
                { 0x010000000008         , TableCard._53        },  // 53
                { 0x010000000004         , TableCard._43        },  // 43
                { 0x010000000002         , TableCard._33        },  // 33
                { 0x010000000001         , TableCard._32        },  // 32
                { 0x00C000000000         , TableCard._A2        },  // A2
                { 0x00A000000000         , TableCard._K2        },  // K2
                { 0x009000000000         , TableCard._Q2        },  // Q2
                { 0x008800000000         , TableCard._J2        },  // J2
                { 0x008400000000         , TableCard._T2        },  // T2
                { 0x008200000000         , TableCard._92        },  // 92
                { 0x008100000000         , TableCard._82        },  // 82
                { 0x008080000000         , TableCard._72        },  // 72
                { 0x008040000000         , TableCard._62        },  // 62
                { 0x008020000000         , TableCard._52        },  // 52
                { 0x008010000000         , TableCard._42        },  // 42
                { 0x008008000000         , TableCard._32        },  // 32
                { 0x008004000000         , TableCard._22        },  // 22
                { 0x008002000000         , TableCard._A2        },  // A2
                { 0x008001000000         , TableCard._K2        },  // K2
                { 0x008000800000         , TableCard._Q2        },  // Q2
                { 0x008000400000         , TableCard._J2        },  // J2
                { 0x008000200000         , TableCard._T2        },  // T2
                { 0x008000100000         , TableCard._92        },  // 92
                { 0x008000080000         , TableCard._82        },  // 82
                { 0x008000040000         , TableCard._72        },  // 72
                { 0x008000020000         , TableCard._62        },  // 62
                { 0x008000010000         , TableCard._52        },  // 52
                { 0x008000008000         , TableCard._42        },  // 42
                { 0x008000004000         , TableCard._32        },  // 32
                { 0x008000002000         , TableCard._22        },  // 22
                { 0x008000001000         , TableCard._A2        },  // A2
                { 0x008000000800         , TableCard._K2        },  // K2
                { 0x008000000400         , TableCard._Q2        },  // Q2
                { 0x008000000200         , TableCard._J2        },  // J2
                { 0x008000000100         , TableCard._T2        },  // T2
                { 0x008000000080         , TableCard._92        },  // 92
                { 0x008000000040         , TableCard._82        },  // 82
                { 0x008000000020         , TableCard._72        },  // 72
                { 0x008000000010         , TableCard._62        },  // 62
                { 0x008000000008         , TableCard._52        },  // 52
                { 0x008000000004         , TableCard._42        },  // 42
                { 0x008000000002         , TableCard._32        },  // 32
                { 0x008000000001         , TableCard._22        },  // 22
                { 0x006000000000         , TableCard._AKs       },  // AKs
                { 0x005000000000         , TableCard._AQs       },  // AQs
                { 0x004800000000         , TableCard._AJs       },  // AJs
                { 0x004400000000         , TableCard._ATs       },  // ATs
                { 0x004200000000         , TableCard._A9s       },  // A9s
                { 0x004100000000         , TableCard._A8s       },  // A8s
                { 0x004080000000         , TableCard._A7s       },  // A7s
                { 0x004040000000         , TableCard._A6s       },  // A6s
                { 0x004020000000         , TableCard._A5s       },  // A5s
                { 0x004010000000         , TableCard._A4s       },  // A4s
                { 0x004008000000         , TableCard._A3s       },  // A3s
                { 0x004004000000         , TableCard._A2s       },  // A2s
                { 0x004002000000         , TableCard._AA        },  // AA
                { 0x004001000000         , TableCard._AK        },  // AK
                { 0x004000800000         , TableCard._AQ        },  // AQ
                { 0x004000400000         , TableCard._AJ        },  // AJ
                { 0x004000200000         , TableCard._AT        },  // AT
                { 0x004000100000         , TableCard._A9        },  // A9
                { 0x004000080000         , TableCard._A8        },  // A8
                { 0x004000040000         , TableCard._A7        },  // A7
                { 0x004000020000         , TableCard._A6        },  // A6
                { 0x004000010000         , TableCard._A5        },  // A5
                { 0x004000008000         , TableCard._A4        },  // A4
                { 0x004000004000         , TableCard._A3        },  // A3
                { 0x004000002000         , TableCard._A2        },  // A2
                { 0x004000001000         , TableCard._AA        },  // AA
                { 0x004000000800         , TableCard._AK        },  // AK
                { 0x004000000400         , TableCard._AQ        },  // AQ
                { 0x004000000200         , TableCard._AJ        },  // AJ
                { 0x004000000100         , TableCard._AT        },  // AT
                { 0x004000000080         , TableCard._A9        },  // A9
                { 0x004000000040         , TableCard._A8        },  // A8
                { 0x004000000020         , TableCard._A7        },  // A7
                { 0x004000000010         , TableCard._A6        },  // A6
                { 0x004000000008         , TableCard._A5        },  // A5
                { 0x004000000004         , TableCard._A4        },  // A4
                { 0x004000000002         , TableCard._A3        },  // A3
                { 0x004000000001         , TableCard._A2        },  // A2
                { 0x003000000000         , TableCard._KQs       },  // KQs
                { 0x002800000000         , TableCard._KJs       },  // KJs
                { 0x002400000000         , TableCard._KTs       },  // KTs
                { 0x002200000000         , TableCard._K9s       },  // K9s
                { 0x002100000000         , TableCard._K8s       },  // K8s
                { 0x002080000000         , TableCard._K7s       },  // K7s
                { 0x002040000000         , TableCard._K6s       },  // K6s
                { 0x002020000000         , TableCard._K5s       },  // K5s
                { 0x002010000000         , TableCard._K4s       },  // K4s
                { 0x002008000000         , TableCard._K3s       },  // K3s
                { 0x002004000000         , TableCard._K2s       },  // K2s
                { 0x002002000000         , TableCard._AK        },  // AK
                { 0x002001000000         , TableCard._KK        },  // KK
                { 0x002000800000         , TableCard._KQ        },  // KQ
                { 0x002000400000         , TableCard._KJ        },  // KJ
                { 0x002000200000         , TableCard._KT        },  // KT
                { 0x002000100000         , TableCard._K9        },  // K9
                { 0x002000080000         , TableCard._K8        },  // K8
                { 0x002000040000         , TableCard._K7        },  // K7
                { 0x002000020000         , TableCard._K6        },  // K6
                { 0x002000010000         , TableCard._K5        },  // K5
                { 0x002000008000         , TableCard._K4        },  // K4
                { 0x002000004000         , TableCard._K3        },  // K3
                { 0x002000002000         , TableCard._K2        },  // K2
                { 0x002000001000         , TableCard._AK        },  // AK
                { 0x002000000800         , TableCard._KK        },  // KK
                { 0x002000000400         , TableCard._KQ        },  // KQ
                { 0x002000000200         , TableCard._KJ        },  // KJ
                { 0x002000000100         , TableCard._KT        },  // KT
                { 0x002000000080         , TableCard._K9        },  // K9
                { 0x002000000040         , TableCard._K8        },  // K8
                { 0x002000000020         , TableCard._K7        },  // K7
                { 0x002000000010         , TableCard._K6        },  // K6
                { 0x002000000008         , TableCard._K5        },  // K5
                { 0x002000000004         , TableCard._K4        },  // K4
                { 0x002000000002         , TableCard._K3        },  // K3
                { 0x002000000001         , TableCard._K2        },  // K2
                { 0x001800000000         , TableCard._QJs       },  // QJs
                { 0x001400000000         , TableCard._QTs       },  // QTs
                { 0x001200000000         , TableCard._Q9s       },  // Q9s
                { 0x001100000000         , TableCard._Q8s       },  // Q8s
                { 0x001080000000         , TableCard._Q7s       },  // Q7s
                { 0x001040000000         , TableCard._Q6s       },  // Q6s
                { 0x001020000000         , TableCard._Q5s       },  // Q5s
                { 0x001010000000         , TableCard._Q4s       },  // Q4s
                { 0x001008000000         , TableCard._Q3s       },  // Q3s
                { 0x001004000000         , TableCard._Q2s       },  // Q2s
                { 0x001002000000         , TableCard._AQ        },  // AQ
                { 0x001001000000         , TableCard._KQ        },  // KQ
                { 0x001000800000         , TableCard._QQ        },  // QQ
                { 0x001000400000         , TableCard._QJ        },  // QJ
                { 0x001000200000         , TableCard._QT        },  // QT
                { 0x001000100000         , TableCard._Q9        },  // Q9
                { 0x001000080000         , TableCard._Q8        },  // Q8
                { 0x001000040000         , TableCard._Q7        },  // Q7
                { 0x001000020000         , TableCard._Q6        },  // Q6
                { 0x001000010000         , TableCard._Q5        },  // Q5
                { 0x001000008000         , TableCard._Q4        },  // Q4
                { 0x001000004000         , TableCard._Q3        },  // Q3
                { 0x001000002000         , TableCard._Q2        },  // Q2
                { 0x001000001000         , TableCard._AQ        },  // AQ
                { 0x001000000800         , TableCard._KQ        },  // KQ
                { 0x001000000400         , TableCard._QQ        },  // QQ
                { 0x001000000200         , TableCard._QJ        },  // QJ
                { 0x001000000100         , TableCard._QT        },  // QT
                { 0x001000000080         , TableCard._Q9        },  // Q9
                { 0x001000000040         , TableCard._Q8        },  // Q8
                { 0x001000000020         , TableCard._Q7        },  // Q7
                { 0x001000000010         , TableCard._Q6        },  // Q6
                { 0x001000000008         , TableCard._Q5        },  // Q5
                { 0x001000000004         , TableCard._Q4        },  // Q4
                { 0x001000000002         , TableCard._Q3        },  // Q3
                { 0x001000000001         , TableCard._Q2        },  // Q2
                { 0x000C00000000         , TableCard._JTs       },  // JTs
                { 0x000A00000000         , TableCard._J9s       },  // J9s
                { 0x000900000000         , TableCard._J8s       },  // J8s
                { 0x000880000000         , TableCard._J7s       },  // J7s
                { 0x000840000000         , TableCard._J6s       },  // J6s
                { 0x000820000000         , TableCard._J5s       },  // J5s
                { 0x000810000000         , TableCard._J4s       },  // J4s
                { 0x000808000000         , TableCard._J3s       },  // J3s
                { 0x000804000000         , TableCard._J2s       },  // J2s
                { 0x000802000000         , TableCard._AJ        },  // AJ
                { 0x000801000000         , TableCard._KJ        },  // KJ
                { 0x000800800000         , TableCard._QJ        },  // QJ
                { 0x000800400000         , TableCard._JJ        },  // JJ
                { 0x000800200000         , TableCard._JT        },  // JT
                { 0x000800100000         , TableCard._J9        },  // J9
                { 0x000800080000         , TableCard._J8        },  // J8
                { 0x000800040000         , TableCard._J7        },  // J7
                { 0x000800020000         , TableCard._J6        },  // J6
                { 0x000800010000         , TableCard._J5        },  // J5
                { 0x000800008000         , TableCard._J4        },  // J4
                { 0x000800004000         , TableCard._J3        },  // J3
                { 0x000800002000         , TableCard._J2        },  // J2
                { 0x000800001000         , TableCard._AJ        },  // AJ
                { 0x000800000800         , TableCard._KJ        },  // KJ
                { 0x000800000400         , TableCard._QJ        },  // QJ
                { 0x000800000200         , TableCard._JJ        },  // JJ
                { 0x000800000100         , TableCard._JT        },  // JT
                { 0x000800000080         , TableCard._J9        },  // J9
                { 0x000800000040         , TableCard._J8        },  // J8
                { 0x000800000020         , TableCard._J7        },  // J7
                { 0x000800000010         , TableCard._J6        },  // J6
                { 0x000800000008         , TableCard._J5        },  // J5
                { 0x000800000004         , TableCard._J4        },  // J4
                { 0x000800000002         , TableCard._J3        },  // J3
                { 0x000800000001         , TableCard._J2        },  // J2
                { 0x000600000000         , TableCard._T9s       },  // T9s
                { 0x000500000000         , TableCard._T8s       },  // T8s
                { 0x000480000000         , TableCard._T7s       },  // T7s
                { 0x000440000000         , TableCard._T6s       },  // T6s
                { 0x000420000000         , TableCard._T5s       },  // T5s
                { 0x000410000000         , TableCard._T4s       },  // T4s
                { 0x000408000000         , TableCard._T3s       },  // T3s
                { 0x000404000000         , TableCard._T2s       },  // T2s
                { 0x000402000000         , TableCard._AT        },  // AT
                { 0x000401000000         , TableCard._KT        },  // KT
                { 0x000400800000         , TableCard._QT        },  // QT
                { 0x000400400000         , TableCard._JT        },  // JT
                { 0x000400200000         , TableCard._TT        },  // TT
                { 0x000400100000         , TableCard._T9        },  // T9
                { 0x000400080000         , TableCard._T8        },  // T8
                { 0x000400040000         , TableCard._T7        },  // T7
                { 0x000400020000         , TableCard._T6        },  // T6
                { 0x000400010000         , TableCard._T5        },  // T5
                { 0x000400008000         , TableCard._T4        },  // T4
                { 0x000400004000         , TableCard._T3        },  // T3
                { 0x000400002000         , TableCard._T2        },  // T2
                { 0x000400001000         , TableCard._AT        },  // AT
                { 0x000400000800         , TableCard._KT        },  // KT
                { 0x000400000400         , TableCard._QT        },  // QT
                { 0x000400000200         , TableCard._JT        },  // JT
                { 0x000400000100         , TableCard._TT        },  // TT
                { 0x000400000080         , TableCard._T9        },  // T9
                { 0x000400000040         , TableCard._T8        },  // T8
                { 0x000400000020         , TableCard._T7        },  // T7
                { 0x000400000010         , TableCard._T6        },  // T6
                { 0x000400000008         , TableCard._T5        },  // T5
                { 0x000400000004         , TableCard._T4        },  // T4
                { 0x000400000002         , TableCard._T3        },  // T3
                { 0x000400000001         , TableCard._T2        },  // T2
                { 0x000300000000         , TableCard._98s       },  // 98s
                { 0x000280000000         , TableCard._97s       },  // 97s
                { 0x000240000000         , TableCard._96s       },  // 96s
                { 0x000220000000         , TableCard._95s       },  // 95s
                { 0x000210000000         , TableCard._94s       },  // 94s
                { 0x000208000000         , TableCard._93s       },  // 93s
                { 0x000204000000         , TableCard._92s       },  // 92s
                { 0x000202000000         , TableCard._A9        },  // A9
                { 0x000201000000         , TableCard._K9        },  // K9
                { 0x000200800000         , TableCard._Q9        },  // Q9
                { 0x000200400000         , TableCard._J9        },  // J9
                { 0x000200200000         , TableCard._T9        },  // T9
                { 0x000200100000         , TableCard._99        },  // 99
                { 0x000200080000         , TableCard._98        },  // 98
                { 0x000200040000         , TableCard._97        },  // 97
                { 0x000200020000         , TableCard._96        },  // 96
                { 0x000200010000         , TableCard._95        },  // 95
                { 0x000200008000         , TableCard._94        },  // 94
                { 0x000200004000         , TableCard._93        },  // 93
                { 0x000200002000         , TableCard._92        },  // 92
                { 0x000200001000         , TableCard._A9        },  // A9
                { 0x000200000800         , TableCard._K9        },  // K9
                { 0x000200000400         , TableCard._Q9        },  // Q9
                { 0x000200000200         , TableCard._J9        },  // J9
                { 0x000200000100         , TableCard._T9        },  // T9
                { 0x000200000080         , TableCard._99        },  // 99
                { 0x000200000040         , TableCard._98        },  // 98
                { 0x000200000020         , TableCard._97        },  // 97
                { 0x000200000010         , TableCard._96        },  // 96
                { 0x000200000008         , TableCard._95        },  // 95
                { 0x000200000004         , TableCard._94        },  // 94
                { 0x000200000002         , TableCard._93        },  // 93
                { 0x000200000001         , TableCard._92        },  // 92
                { 0x000180000000         , TableCard._87s       },  // 87s
                { 0x000140000000         , TableCard._86s       },  // 86s
                { 0x000120000000         , TableCard._85s       },  // 85s
                { 0x000110000000         , TableCard._84s       },  // 84s
                { 0x000108000000         , TableCard._83s       },  // 83s
                { 0x000104000000         , TableCard._82s       },  // 82s
                { 0x000102000000         , TableCard._A8        },  // A8
                { 0x000101000000         , TableCard._K8        },  // K8
                { 0x000100800000         , TableCard._Q8        },  // Q8
                { 0x000100400000         , TableCard._J8        },  // J8
                { 0x000100200000         , TableCard._T8        },  // T8
                { 0x000100100000         , TableCard._98        },  // 98
                { 0x000100080000         , TableCard._88        },  // 88
                { 0x000100040000         , TableCard._87        },  // 87
                { 0x000100020000         , TableCard._86        },  // 86
                { 0x000100010000         , TableCard._85        },  // 85
                { 0x000100008000         , TableCard._84        },  // 84
                { 0x000100004000         , TableCard._83        },  // 83
                { 0x000100002000         , TableCard._82        },  // 82
                { 0x000100001000         , TableCard._A8        },  // A8
                { 0x000100000800         , TableCard._K8        },  // K8
                { 0x000100000400         , TableCard._Q8        },  // Q8
                { 0x000100000200         , TableCard._J8        },  // J8
                { 0x000100000100         , TableCard._T8        },  // T8
                { 0x000100000080         , TableCard._98        },  // 98
                { 0x000100000040         , TableCard._88        },  // 88
                { 0x000100000020         , TableCard._87        },  // 87
                { 0x000100000010         , TableCard._86        },  // 86
                { 0x000100000008         , TableCard._85        },  // 85
                { 0x000100000004         , TableCard._84        },  // 84
                { 0x000100000002         , TableCard._83        },  // 83
                { 0x000100000001         , TableCard._82        },  // 82
                { 0x0000C0000000         , TableCard._76s       },  // 76s
                { 0x0000A0000000         , TableCard._75s       },  // 75s
                { 0x000090000000         , TableCard._74s       },  // 74s
                { 0x000088000000         , TableCard._73s       },  // 73s
                { 0x000084000000         , TableCard._72s       },  // 72s
                { 0x000082000000         , TableCard._A7        },  // A7
                { 0x000081000000         , TableCard._K7        },  // K7
                { 0x000080800000         , TableCard._Q7        },  // Q7
                { 0x000080400000         , TableCard._J7        },  // J7
                { 0x000080200000         , TableCard._T7        },  // T7
                { 0x000080100000         , TableCard._97        },  // 97
                { 0x000080080000         , TableCard._87        },  // 87
                { 0x000080040000         , TableCard._77        },  // 77
                { 0x000080020000         , TableCard._76        },  // 76
                { 0x000080010000         , TableCard._75        },  // 75
                { 0x000080008000         , TableCard._74        },  // 74
                { 0x000080004000         , TableCard._73        },  // 73
                { 0x000080002000         , TableCard._72        },  // 72
                { 0x000080001000         , TableCard._A7        },  // A7
                { 0x000080000800         , TableCard._K7        },  // K7
                { 0x000080000400         , TableCard._Q7        },  // Q7
                { 0x000080000200         , TableCard._J7        },  // J7
                { 0x000080000100         , TableCard._T7        },  // T7
                { 0x000080000080         , TableCard._97        },  // 97
                { 0x000080000040         , TableCard._87        },  // 87
                { 0x000080000020         , TableCard._77        },  // 77
                { 0x000080000010         , TableCard._76        },  // 76
                { 0x000080000008         , TableCard._75        },  // 75
                { 0x000080000004         , TableCard._74        },  // 74
                { 0x000080000002         , TableCard._73        },  // 73
                { 0x000080000001         , TableCard._72        },  // 72
                { 0x000060000000         , TableCard._65s       },  // 65s
                { 0x000050000000         , TableCard._64s       },  // 64s
                { 0x000048000000         , TableCard._63s       },  // 63s
                { 0x000044000000         , TableCard._62s       },  // 62s
                { 0x000042000000         , TableCard._A6        },  // A6
                { 0x000041000000         , TableCard._K6        },  // K6
                { 0x000040800000         , TableCard._Q6        },  // Q6
                { 0x000040400000         , TableCard._J6        },  // J6
                { 0x000040200000         , TableCard._T6        },  // T6
                { 0x000040100000         , TableCard._96        },  // 96
                { 0x000040080000         , TableCard._86        },  // 86
                { 0x000040040000         , TableCard._76        },  // 76
                { 0x000040020000         , TableCard._66        },  // 66
                { 0x000040010000         , TableCard._65        },  // 65
                { 0x000040008000         , TableCard._64        },  // 64
                { 0x000040004000         , TableCard._63        },  // 63
                { 0x000040002000         , TableCard._62        },  // 62
                { 0x000040001000         , TableCard._A6        },  // A6
                { 0x000040000800         , TableCard._K6        },  // K6
                { 0x000040000400         , TableCard._Q6        },  // Q6
                { 0x000040000200         , TableCard._J6        },  // J6
                { 0x000040000100         , TableCard._T6        },  // T6
                { 0x000040000080         , TableCard._96        },  // 96
                { 0x000040000040         , TableCard._86        },  // 86
                { 0x000040000020         , TableCard._76        },  // 76
                { 0x000040000010         , TableCard._66        },  // 66
                { 0x000040000008         , TableCard._65        },  // 65
                { 0x000040000004         , TableCard._64        },  // 64
                { 0x000040000002         , TableCard._63        },  // 63
                { 0x000040000001         , TableCard._62        },  // 62
                { 0x000030000000         , TableCard._54s       },  // 54s
                { 0x000028000000         , TableCard._53s       },  // 53s
                { 0x000024000000         , TableCard._52s       },  // 52s
                { 0x000022000000         , TableCard._A5        },  // A5
                { 0x000021000000         , TableCard._K5        },  // K5
                { 0x000020800000         , TableCard._Q5        },  // Q5
                { 0x000020400000         , TableCard._J5        },  // J5
                { 0x000020200000         , TableCard._T5        },  // T5
                { 0x000020100000         , TableCard._95        },  // 95
                { 0x000020080000         , TableCard._85        },  // 85
                { 0x000020040000         , TableCard._75        },  // 75
                { 0x000020020000         , TableCard._65        },  // 65
                { 0x000020010000         , TableCard._55        },  // 55
                { 0x000020008000         , TableCard._54        },  // 54
                { 0x000020004000         , TableCard._53        },  // 53
                { 0x000020002000         , TableCard._52        },  // 52
                { 0x000020001000         , TableCard._A5        },  // A5
                { 0x000020000800         , TableCard._K5        },  // K5
                { 0x000020000400         , TableCard._Q5        },  // Q5
                { 0x000020000200         , TableCard._J5        },  // J5
                { 0x000020000100         , TableCard._T5        },  // T5
                { 0x000020000080         , TableCard._95        },  // 95
                { 0x000020000040         , TableCard._85        },  // 85
                { 0x000020000020         , TableCard._75        },  // 75
                { 0x000020000010         , TableCard._65        },  // 65
                { 0x000020000008         , TableCard._55        },  // 55
                { 0x000020000004         , TableCard._54        },  // 54
                { 0x000020000002         , TableCard._53        },  // 53
                { 0x000020000001         , TableCard._52        },  // 52
                { 0x000018000000         , TableCard._43s       },  // 43s
                { 0x000014000000         , TableCard._42s       },  // 42s
                { 0x000012000000         , TableCard._A4        },  // A4
                { 0x000011000000         , TableCard._K4        },  // K4
                { 0x000010800000         , TableCard._Q4        },  // Q4
                { 0x000010400000         , TableCard._J4        },  // J4
                { 0x000010200000         , TableCard._T4        },  // T4
                { 0x000010100000         , TableCard._94        },  // 94
                { 0x000010080000         , TableCard._84        },  // 84
                { 0x000010040000         , TableCard._74        },  // 74
                { 0x000010020000         , TableCard._64        },  // 64
                { 0x000010010000         , TableCard._54        },  // 54
                { 0x000010008000         , TableCard._44        },  // 44
                { 0x000010004000         , TableCard._43        },  // 43
                { 0x000010002000         , TableCard._42        },  // 42
                { 0x000010001000         , TableCard._A4        },  // A4
                { 0x000010000800         , TableCard._K4        },  // K4
                { 0x000010000400         , TableCard._Q4        },  // Q4
                { 0x000010000200         , TableCard._J4        },  // J4
                { 0x000010000100         , TableCard._T4        },  // T4
                { 0x000010000080         , TableCard._94        },  // 94
                { 0x000010000040         , TableCard._84        },  // 84
                { 0x000010000020         , TableCard._74        },  // 74
                { 0x000010000010         , TableCard._64        },  // 64
                { 0x000010000008         , TableCard._54        },  // 54
                { 0x000010000004         , TableCard._44        },  // 44
                { 0x000010000002         , TableCard._43        },  // 43
                { 0x000010000001         , TableCard._42        },  // 42
                { 0x00000C000000         , TableCard._32s       },  // 32s
                { 0x00000A000000         , TableCard._A3        },  // A3
                { 0x000009000000         , TableCard._K3        },  // K3
                { 0x000008800000         , TableCard._Q3        },  // Q3
                { 0x000008400000         , TableCard._J3        },  // J3
                { 0x000008200000         , TableCard._T3        },  // T3
                { 0x000008100000         , TableCard._93        },  // 93
                { 0x000008080000         , TableCard._83        },  // 83
                { 0x000008040000         , TableCard._73        },  // 73
                { 0x000008020000         , TableCard._63        },  // 63
                { 0x000008010000         , TableCard._53        },  // 53
                { 0x000008008000         , TableCard._43        },  // 43
                { 0x000008004000         , TableCard._33        },  // 33
                { 0x000008002000         , TableCard._32        },  // 32
                { 0x000008001000         , TableCard._A3        },  // A3
                { 0x000008000800         , TableCard._K3        },  // K3
                { 0x000008000400         , TableCard._Q3        },  // Q3
                { 0x000008000200         , TableCard._J3        },  // J3
                { 0x000008000100         , TableCard._T3        },  // T3
                { 0x000008000080         , TableCard._93        },  // 93
                { 0x000008000040         , TableCard._83        },  // 83
                { 0x000008000020         , TableCard._73        },  // 73
                { 0x000008000010         , TableCard._63        },  // 63
                { 0x000008000008         , TableCard._53        },  // 53
                { 0x000008000004         , TableCard._43        },  // 43
                { 0x000008000002         , TableCard._33        },  // 33
                { 0x000008000001         , TableCard._32        },  // 32
                { 0x000006000000         , TableCard._A2        },  // A2
                { 0x000005000000         , TableCard._K2        },  // K2
                { 0x000004800000         , TableCard._Q2        },  // Q2
                { 0x000004400000         , TableCard._J2        },  // J2
                { 0x000004200000         , TableCard._T2        },  // T2
                { 0x000004100000         , TableCard._92        },  // 92
                { 0x000004080000         , TableCard._82        },  // 82
                { 0x000004040000         , TableCard._72        },  // 72
                { 0x000004020000         , TableCard._62        },  // 62
                { 0x000004010000         , TableCard._52        },  // 52
                { 0x000004008000         , TableCard._42        },  // 42
                { 0x000004004000         , TableCard._32        },  // 32
                { 0x000004002000         , TableCard._22        },  // 22
                { 0x000004001000         , TableCard._A2        },  // A2
                { 0x000004000800         , TableCard._K2        },  // K2
                { 0x000004000400         , TableCard._Q2        },  // Q2
                { 0x000004000200         , TableCard._J2        },  // J2
                { 0x000004000100         , TableCard._T2        },  // T2
                { 0x000004000080         , TableCard._92        },  // 92
                { 0x000004000040         , TableCard._82        },  // 82
                { 0x000004000020         , TableCard._72        },  // 72
                { 0x000004000010         , TableCard._62        },  // 62
                { 0x000004000008         , TableCard._52        },  // 52
                { 0x000004000004         , TableCard._42        },  // 42
                { 0x000004000002         , TableCard._32        },  // 32
                { 0x000004000001         , TableCard._22        },  // 22
                { 0x000003000000         , TableCard._AKs       },  // AKs
                { 0x000002800000         , TableCard._AQs       },  // AQs
                { 0x000002400000         , TableCard._AJs       },  // AJs
                { 0x000002200000         , TableCard._ATs       },  // ATs
                { 0x000002100000         , TableCard._A9s       },  // A9s
                { 0x000002080000         , TableCard._A8s       },  // A8s
                { 0x000002040000         , TableCard._A7s       },  // A7s
                { 0x000002020000         , TableCard._A6s       },  // A6s
                { 0x000002010000         , TableCard._A5s       },  // A5s
                { 0x000002008000         , TableCard._A4s       },  // A4s
                { 0x000002004000         , TableCard._A3s       },  // A3s
                { 0x000002002000         , TableCard._A2s       },  // A2s
                { 0x000002001000         , TableCard._AA        },  // AA
                { 0x000002000800         , TableCard._AK        },  // AK
                { 0x000002000400         , TableCard._AQ        },  // AQ
                { 0x000002000200         , TableCard._AJ        },  // AJ
                { 0x000002000100         , TableCard._AT        },  // AT
                { 0x000002000080         , TableCard._A9        },  // A9
                { 0x000002000040         , TableCard._A8        },  // A8
                { 0x000002000020         , TableCard._A7        },  // A7
                { 0x000002000010         , TableCard._A6        },  // A6
                { 0x000002000008         , TableCard._A5        },  // A5
                { 0x000002000004         , TableCard._A4        },  // A4
                { 0x000002000002         , TableCard._A3        },  // A3
                { 0x000002000001         , TableCard._A2        },  // A2
                { 0x000001800000         , TableCard._KQs       },  // KQs
                { 0x000001400000         , TableCard._KJs       },  // KJs
                { 0x000001200000         , TableCard._KTs       },  // KTs
                { 0x000001100000         , TableCard._K9s       },  // K9s
                { 0x000001080000         , TableCard._K8s       },  // K8s
                { 0x000001040000         , TableCard._K7s       },  // K7s
                { 0x000001020000         , TableCard._K6s       },  // K6s
                { 0x000001010000         , TableCard._K5s       },  // K5s
                { 0x000001008000         , TableCard._K4s       },  // K4s
                { 0x000001004000         , TableCard._K3s       },  // K3s
                { 0x000001002000         , TableCard._K2s       },  // K2s
                { 0x000001001000         , TableCard._AK        },  // AK
                { 0x000001000800         , TableCard._KK        },  // KK
                { 0x000001000400         , TableCard._KQ        },  // KQ
                { 0x000001000200         , TableCard._KJ        },  // KJ
                { 0x000001000100         , TableCard._KT        },  // KT
                { 0x000001000080         , TableCard._K9        },  // K9
                { 0x000001000040         , TableCard._K8        },  // K8
                { 0x000001000020         , TableCard._K7        },  // K7
                { 0x000001000010         , TableCard._K6        },  // K6
                { 0x000001000008         , TableCard._K5        },  // K5
                { 0x000001000004         , TableCard._K4        },  // K4
                { 0x000001000002         , TableCard._K3        },  // K3
                { 0x000001000001         , TableCard._K2        },  // K2
                { 0x000000C00000         , TableCard._QJs       },  // QJs
                { 0x000000A00000         , TableCard._QTs       },  // QTs
                { 0x000000900000         , TableCard._Q9s       },  // Q9s
                { 0x000000880000         , TableCard._Q8s       },  // Q8s
                { 0x000000840000         , TableCard._Q7s       },  // Q7s
                { 0x000000820000         , TableCard._Q6s       },  // Q6s
                { 0x000000810000         , TableCard._Q5s       },  // Q5s
                { 0x000000808000         , TableCard._Q4s       },  // Q4s
                { 0x000000804000         , TableCard._Q3s       },  // Q3s
                { 0x000000802000         , TableCard._Q2s       },  // Q2s
                { 0x000000801000         , TableCard._AQ        },  // AQ
                { 0x000000800800         , TableCard._KQ        },  // KQ
                { 0x000000800400         , TableCard._QQ        },  // QQ
                { 0x000000800200         , TableCard._QJ        },  // QJ
                { 0x000000800100         , TableCard._QT        },  // QT
                { 0x000000800080         , TableCard._Q9        },  // Q9
                { 0x000000800040         , TableCard._Q8        },  // Q8
                { 0x000000800020         , TableCard._Q7        },  // Q7
                { 0x000000800010         , TableCard._Q6        },  // Q6
                { 0x000000800008         , TableCard._Q5        },  // Q5
                { 0x000000800004         , TableCard._Q4        },  // Q4
                { 0x000000800002         , TableCard._Q3        },  // Q3
                { 0x000000800001         , TableCard._Q2        },  // Q2
                { 0x000000600000         , TableCard._JTs       },  // JTs
                { 0x000000500000         , TableCard._J9s       },  // J9s
                { 0x000000480000         , TableCard._J8s       },  // J8s
                { 0x000000440000         , TableCard._J7s       },  // J7s
                { 0x000000420000         , TableCard._J6s       },  // J6s
                { 0x000000410000         , TableCard._J5s       },  // J5s
                { 0x000000408000         , TableCard._J4s       },  // J4s
                { 0x000000404000         , TableCard._J3s       },  // J3s
                { 0x000000402000         , TableCard._J2s       },  // J2s
                { 0x000000401000         , TableCard._AJ        },  // AJ
                { 0x000000400800         , TableCard._KJ        },  // KJ
                { 0x000000400400         , TableCard._QJ        },  // QJ
                { 0x000000400200         , TableCard._JJ        },  // JJ
                { 0x000000400100         , TableCard._JT        },  // JT
                { 0x000000400080         , TableCard._J9        },  // J9
                { 0x000000400040         , TableCard._J8        },  // J8
                { 0x000000400020         , TableCard._J7        },  // J7
                { 0x000000400010         , TableCard._J6        },  // J6
                { 0x000000400008         , TableCard._J5        },  // J5
                { 0x000000400004         , TableCard._J4        },  // J4
                { 0x000000400002         , TableCard._J3        },  // J3
                { 0x000000400001         , TableCard._J2        },  // J2
                { 0x000000300000         , TableCard._T9s       },  // T9s
                { 0x000000280000         , TableCard._T8s       },  // T8s
                { 0x000000240000         , TableCard._T7s       },  // T7s
                { 0x000000220000         , TableCard._T6s       },  // T6s
                { 0x000000210000         , TableCard._T5s       },  // T5s
                { 0x000000208000         , TableCard._T4s       },  // T4s
                { 0x000000204000         , TableCard._T3s       },  // T3s
                { 0x000000202000         , TableCard._T2s       },  // T2s
                { 0x000000201000         , TableCard._AT        },  // AT
                { 0x000000200800         , TableCard._KT        },  // KT
                { 0x000000200400         , TableCard._QT        },  // QT
                { 0x000000200200         , TableCard._JT        },  // JT
                { 0x000000200100         , TableCard._TT        },  // TT
                { 0x000000200080         , TableCard._T9        },  // T9
                { 0x000000200040         , TableCard._T8        },  // T8
                { 0x000000200020         , TableCard._T7        },  // T7
                { 0x000000200010         , TableCard._T6        },  // T6
                { 0x000000200008         , TableCard._T5        },  // T5
                { 0x000000200004         , TableCard._T4        },  // T4
                { 0x000000200002         , TableCard._T3        },  // T3
                { 0x000000200001         , TableCard._T2        },  // T2
                { 0x000000180000         , TableCard._98s       },  // 98s
                { 0x000000140000         , TableCard._97s       },  // 97s
                { 0x000000120000         , TableCard._96s       },  // 96s
                { 0x000000110000         , TableCard._95s       },  // 95s
                { 0x000000108000         , TableCard._94s       },  // 94s
                { 0x000000104000         , TableCard._93s       },  // 93s
                { 0x000000102000         , TableCard._92s       },  // 92s
                { 0x000000101000         , TableCard._A9        },  // A9
                { 0x000000100800         , TableCard._K9        },  // K9
                { 0x000000100400         , TableCard._Q9        },  // Q9
                { 0x000000100200         , TableCard._J9        },  // J9
                { 0x000000100100         , TableCard._T9        },  // T9
                { 0x000000100080         , TableCard._99        },  // 99
                { 0x000000100040         , TableCard._98        },  // 98
                { 0x000000100020         , TableCard._97        },  // 97
                { 0x000000100010         , TableCard._96        },  // 96
                { 0x000000100008         , TableCard._95        },  // 95
                { 0x000000100004         , TableCard._94        },  // 94
                { 0x000000100002         , TableCard._93        },  // 93
                { 0x000000100001         , TableCard._92        },  // 92
                { 0x0000000C0000         , TableCard._87s       },  // 87s
                { 0x0000000A0000         , TableCard._86s       },  // 86s
                { 0x000000090000         , TableCard._85s       },  // 85s
                { 0x000000088000         , TableCard._84s       },  // 84s
                { 0x000000084000         , TableCard._83s       },  // 83s
                { 0x000000082000         , TableCard._82s       },  // 82s
                { 0x000000081000         , TableCard._A8        },  // A8
                { 0x000000080800         , TableCard._K8        },  // K8
                { 0x000000080400         , TableCard._Q8        },  // Q8
                { 0x000000080200         , TableCard._J8        },  // J8
                { 0x000000080100         , TableCard._T8        },  // T8
                { 0x000000080080         , TableCard._98        },  // 98
                { 0x000000080040         , TableCard._88        },  // 88
                { 0x000000080020         , TableCard._87        },  // 87
                { 0x000000080010         , TableCard._86        },  // 86
                { 0x000000080008         , TableCard._85        },  // 85
                { 0x000000080004         , TableCard._84        },  // 84
                { 0x000000080002         , TableCard._83        },  // 83
                { 0x000000080001         , TableCard._82        },  // 82
                { 0x000000060000         , TableCard._76s       },  // 76s
                { 0x000000050000         , TableCard._75s       },  // 75s
                { 0x000000048000         , TableCard._74s       },  // 74s
                { 0x000000044000         , TableCard._73s       },  // 73s
                { 0x000000042000         , TableCard._72s       },  // 72s
                { 0x000000041000         , TableCard._A7        },  // A7
                { 0x000000040800         , TableCard._K7        },  // K7
                { 0x000000040400         , TableCard._Q7        },  // Q7
                { 0x000000040200         , TableCard._J7        },  // J7
                { 0x000000040100         , TableCard._T7        },  // T7
                { 0x000000040080         , TableCard._97        },  // 97
                { 0x000000040040         , TableCard._87        },  // 87
                { 0x000000040020         , TableCard._77        },  // 77
                { 0x000000040010         , TableCard._76        },  // 76
                { 0x000000040008         , TableCard._75        },  // 75
                { 0x000000040004         , TableCard._74        },  // 74
                { 0x000000040002         , TableCard._73        },  // 73
                { 0x000000040001         , TableCard._72        },  // 72
                { 0x000000030000         , TableCard._65s       },  // 65s
                { 0x000000028000         , TableCard._64s       },  // 64s
                { 0x000000024000         , TableCard._63s       },  // 63s
                { 0x000000022000         , TableCard._62s       },  // 62s
                { 0x000000021000         , TableCard._A6        },  // A6
                { 0x000000020800         , TableCard._K6        },  // K6
                { 0x000000020400         , TableCard._Q6        },  // Q6
                { 0x000000020200         , TableCard._J6        },  // J6
                { 0x000000020100         , TableCard._T6        },  // T6
                { 0x000000020080         , TableCard._96        },  // 96
                { 0x000000020040         , TableCard._86        },  // 86
                { 0x000000020020         , TableCard._76        },  // 76
                { 0x000000020010         , TableCard._66        },  // 66
                { 0x000000020008         , TableCard._65        },  // 65
                { 0x000000020004         , TableCard._64        },  // 64
                { 0x000000020002         , TableCard._63        },  // 63
                { 0x000000020001         , TableCard._62        },  // 62
                { 0x000000018000         , TableCard._54s       },  // 54s
                { 0x000000014000         , TableCard._53s       },  // 53s
                { 0x000000012000         , TableCard._52s       },  // 52s
                { 0x000000011000         , TableCard._A5        },  // A5
                { 0x000000010800         , TableCard._K5        },  // K5
                { 0x000000010400         , TableCard._Q5        },  // Q5
                { 0x000000010200         , TableCard._J5        },  // J5
                { 0x000000010100         , TableCard._T5        },  // T5
                { 0x000000010080         , TableCard._95        },  // 95
                { 0x000000010040         , TableCard._85        },  // 85
                { 0x000000010020         , TableCard._75        },  // 75
                { 0x000000010010         , TableCard._65        },  // 65
                { 0x000000010008         , TableCard._55        },  // 55
                { 0x000000010004         , TableCard._54        },  // 54
                { 0x000000010002         , TableCard._53        },  // 53
                { 0x000000010001         , TableCard._52        },  // 52
                { 0x00000000C000         , TableCard._43s       },  // 43s
                { 0x00000000A000         , TableCard._42s       },  // 42s
                { 0x000000009000         , TableCard._A4        },  // A4
                { 0x000000008800         , TableCard._K4        },  // K4
                { 0x000000008400         , TableCard._Q4        },  // Q4
                { 0x000000008200         , TableCard._J4        },  // J4
                { 0x000000008100         , TableCard._T4        },  // T4
                { 0x000000008080         , TableCard._94        },  // 94
                { 0x000000008040         , TableCard._84        },  // 84
                { 0x000000008020         , TableCard._74        },  // 74
                { 0x000000008010         , TableCard._64        },  // 64
                { 0x000000008008         , TableCard._54        },  // 54
                { 0x000000008004         , TableCard._44        },  // 44
                { 0x000000008002         , TableCard._43        },  // 43
                { 0x000000008001         , TableCard._42        },  // 42
                { 0x000000006000         , TableCard._32s       },  // 32s
                { 0x000000005000         , TableCard._A3        },  // A3
                { 0x000000004800         , TableCard._K3        },  // K3
                { 0x000000004400         , TableCard._Q3        },  // Q3
                { 0x000000004200         , TableCard._J3        },  // J3
                { 0x000000004100         , TableCard._T3        },  // T3
                { 0x000000004080         , TableCard._93        },  // 93
                { 0x000000004040         , TableCard._83        },  // 83
                { 0x000000004020         , TableCard._73        },  // 73
                { 0x000000004010         , TableCard._63        },  // 63
                { 0x000000004008         , TableCard._53        },  // 53
                { 0x000000004004         , TableCard._43        },  // 43
                { 0x000000004002         , TableCard._33        },  // 33
                { 0x000000004001         , TableCard._32        },  // 32
                { 0x000000003000         , TableCard._A2        },  // A2
                { 0x000000002800         , TableCard._K2        },  // K2
                { 0x000000002400         , TableCard._Q2        },  // Q2
                { 0x000000002200         , TableCard._J2        },  // J2
                { 0x000000002100         , TableCard._T2        },  // T2
                { 0x000000002080         , TableCard._92        },  // 92
                { 0x000000002040         , TableCard._82        },  // 82
                { 0x000000002020         , TableCard._72        },  // 72
                { 0x000000002010         , TableCard._62        },  // 62
                { 0x000000002008         , TableCard._52        },  // 52
                { 0x000000002004         , TableCard._42        },  // 42
                { 0x000000002002         , TableCard._32        },  // 32
                { 0x000000002001         , TableCard._22        },  // 22
                { 0x000000001800         , TableCard._AKs       },  // AKs
                { 0x000000001400         , TableCard._AQs       },  // AQs
                { 0x000000001200         , TableCard._AJs       },  // AJs
                { 0x000000001100         , TableCard._ATs       },  // ATs
                { 0x000000001080         , TableCard._A9s       },  // A9s
                { 0x000000001040         , TableCard._A8s       },  // A8s
                { 0x000000001020         , TableCard._A7s       },  // A7s
                { 0x000000001010         , TableCard._A6s       },  // A6s
                { 0x000000001008         , TableCard._A5s       },  // A5s
                { 0x000000001004         , TableCard._A4s       },  // A4s
                { 0x000000001002         , TableCard._A3s       },  // A3s
                { 0x000000001001         , TableCard._A2s       },  // A2s
                { 0x000000000C00         , TableCard._KQs       },  // KQs
                { 0x000000000A00         , TableCard._KJs       },  // KJs
                { 0x000000000900         , TableCard._KTs       },  // KTs
                { 0x000000000880         , TableCard._K9s       },  // K9s
                { 0x000000000840         , TableCard._K8s       },  // K8s
                { 0x000000000820         , TableCard._K7s       },  // K7s
                { 0x000000000810         , TableCard._K6s       },  // K6s
                { 0x000000000808         , TableCard._K5s       },  // K5s
                { 0x000000000804         , TableCard._K4s       },  // K4s
                { 0x000000000802         , TableCard._K3s       },  // K3s
                { 0x000000000801         , TableCard._K2s       },  // K2s
                { 0x000000000600         , TableCard._QJs       },  // QJs
                { 0x000000000500         , TableCard._QTs       },  // QTs
                { 0x000000000480         , TableCard._Q9s       },  // Q9s
                { 0x000000000440         , TableCard._Q8s       },  // Q8s
                { 0x000000000420         , TableCard._Q7s       },  // Q7s
                { 0x000000000410         , TableCard._Q6s       },  // Q6s
                { 0x000000000408         , TableCard._Q5s       },  // Q5s
                { 0x000000000404         , TableCard._Q4s       },  // Q4s
                { 0x000000000402         , TableCard._Q3s       },  // Q3s
                { 0x000000000401         , TableCard._Q2s       },  // Q2s
                { 0x000000000300         , TableCard._JTs       },  // JTs
                { 0x000000000280         , TableCard._J9s       },  // J9s
                { 0x000000000240         , TableCard._J8s       },  // J8s
                { 0x000000000220         , TableCard._J7s       },  // J7s
                { 0x000000000210         , TableCard._J6s       },  // J6s
                { 0x000000000208         , TableCard._J5s       },  // J5s
                { 0x000000000204         , TableCard._J4s       },  // J4s
                { 0x000000000202         , TableCard._J3s       },  // J3s
                { 0x000000000201         , TableCard._J2s       },  // J2s
                { 0x000000000180         , TableCard._T9s       },  // T9s
                { 0x000000000140         , TableCard._T8s       },  // T8s
                { 0x000000000120         , TableCard._T7s       },  // T7s
                { 0x000000000110         , TableCard._T6s       },  // T6s
                { 0x000000000108         , TableCard._T5s       },  // T5s
                { 0x000000000104         , TableCard._T4s       },  // T4s
                { 0x000000000102         , TableCard._T3s       },  // T3s
                { 0x000000000101         , TableCard._T2s       },  // T2s
                { 0x0000000000C0         , TableCard._98s       },  // 98s
                { 0x0000000000A0         , TableCard._97s       },  // 97s
                { 0x000000000090         , TableCard._96s       },  // 96s
                { 0x000000000088         , TableCard._95s       },  // 95s
                { 0x000000000084         , TableCard._94s       },  // 94s
                { 0x000000000082         , TableCard._93s       },  // 93s
                { 0x000000000081         , TableCard._92s       },  // 92s
                { 0x000000000060         , TableCard._87s       },  // 87s
                { 0x000000000050         , TableCard._86s       },  // 86s
                { 0x000000000048         , TableCard._85s       },  // 85s
                { 0x000000000044         , TableCard._84s       },  // 84s
                { 0x000000000042         , TableCard._83s       },  // 83s
                { 0x000000000041         , TableCard._82s       },  // 82s
                { 0x000000000030         , TableCard._76s       },  // 76s
                { 0x000000000028         , TableCard._75s       },  // 75s
                { 0x000000000024         , TableCard._74s       },  // 74s
                { 0x000000000022         , TableCard._73s       },  // 73s
                { 0x000000000021         , TableCard._72s       },  // 72s
                { 0x000000000018         , TableCard._65s       },  // 65s
                { 0x000000000014         , TableCard._64s       },  // 64s
                { 0x000000000012         , TableCard._63s       },  // 63s
                { 0x000000000011         , TableCard._62s       },  // 62s
                { 0x00000000000C         , TableCard._54s       },  // 54s
                { 0x00000000000A         , TableCard._53s       },  // 53s
                { 0x000000000009         , TableCard._52s       },  // 52s
                { 0x000000000006         , TableCard._43s       },  // 43s
                { 0x000000000005         , TableCard._42s       },  // 42s
                { 0x000000000003         , TableCard._32s       },  // 32s
        };
        
        public static void printHandRangeMap()
        {
            var range = new RangeTable();
            foreach (var mask in Hand.TwoCardMaskTable)
            {
                var ph = Hand.MaskToPockerShark(mask);
                // Console.WriteLine("{ " + String.Format("0x{0:X12} \t , new Tuple<int, int>{1}  \t", mask, RangeTable.GetPosition(FromMask(mask))) + "},  // " + RangeTable.GetName(FromMask(mask)));
                Console.WriteLine("{ " + String.Format("0x{0:X12} \t , TableCard._{1}  \t", mask, RangeTable.GetName(FromMask(mask))) + "},  // " + RangeTable.GetName(FromMask(mask)));
            }

        }
        #endregion

        #region HandPotential

        public static void WHandPotential(ulong pocket, ulong board, out double ppot, out double npot, double[] weights)
        {
            const int ahead = 2;
            const int tied = 1;
            const int behind = 0;

            double[,] HP = new double[3, 3];
            double[] HPTotal = new double[3];
            int ncards = Hand.BitCount(pocket | board);
            double mult = (ncards == 5 ? 990.0 : 44.0);
            uint ourbest, oppbest;

            // Rank our mask
            uint ourrank = Hand.Evaluate(pocket | board, ncards);

            // Iterate through all possible opponent pocket cards
            foreach (ulong oppPocket in Hand.Hands(0UL, pocket | board, 2))
            {
                uint opprank = Hand.Evaluate(oppPocket | board, ncards);
                int index = (ourrank > opprank ? ahead : (ourrank == opprank ? tied : behind));
                var position = HandRangeMap[oppPocket];
                double weight = weights[(int)position];
                foreach (ulong boardmask in Hand.Hands(board, pocket | oppPocket, 5))
                {
                    ourbest = Hand.Evaluate(pocket | boardmask, 7);
                    oppbest = Hand.Evaluate(oppPocket | boardmask, 7);
                    if (ourbest > oppbest)
                        HP[index, ahead] += weight;
                    else if (ourbest == oppbest)
                        HP[index, tied] += weight;
                    else
                        HP[index, behind] += weight;
                }
                HPTotal[index] += weight;
            }

            double den1 = (mult * (HPTotal[behind] + (HPTotal[tied] / 2.0)));
            double den2 = (mult * (HPTotal[ahead] + (HPTotal[tied] / 2.0)));
            if (den1 > 0)
                ppot = (HP[behind, ahead] + (HP[behind, tied] / 2) + (HP[tied, ahead] / 2)) / den1;
            else
                ppot = 0;
            if (den2 > 0)
                npot = (HP[ahead, behind] + (HP[ahead, tied] / 2) + (HP[tied, behind] / 2)) / den2;
            else
                npot = 0;
        }
        
        public static (double ppot, double npot) HandPotential(List<Deck.Card> Pocket, List<Deck.Card> Board)
        {
            double ppot = 0;
            double npot = 0;
            Hand.HandPotential(GetMask(Pocket), GetMask(Board), out ppot, out npot);
            return (ppot, npot);
        }

        public static (double ppot, double npot) WeightedHandPotential(List<Deck.Card> Pocket, List<Deck.Card> Board, List<double[]> weights)
        {
            double ppot = 0;
            double npot = 0;
            foreach (var w in weights)
            {
                var (pp, np) = WeightedHandPotential(Pocket, Board, w);
                ppot += pp;
                npot += np;
            }
            return (Math.Pow(ppot / weights.Count, weights.Count), Math.Pow(npot / weights.Count, weights.Count));
        }
        public static (double ppot, double npot) WeightedHandPotential(List<Deck.Card> Pocket, List<Deck.Card> Board, double[] weights)
        {
            double ppot = 0;
            double npot = 0;
            WHandPotential(GetMask(Pocket), GetMask(Board), out ppot, out npot, weights);
            return (ppot, npot);
        }
        
        #endregion

        #region HandStrength
        public static double WHandStrength(ulong pocket, ulong board, double[] weights)
        {
            double win = 0.0, count = 0.0;

            uint ourrank = Hand.Evaluate(pocket | board);
            foreach (ulong oppcards in Hand.Hands(0UL, pocket | board, 2))
            {
                uint opprank = Hand.Evaluate(oppcards | board);
                var position = HandRangeMap[oppcards];
                double weight = weights[(int)position];
                if (ourrank > opprank)
                {
                    win += 1.0 * weight;
                }
                else if (ourrank == opprank)
                {
                    win += 0.5 * weight;
                }
                count += 1.0 * weight;
            }
            return win / count;
        }
        
        public static double WeightedHandStrength(List<Deck.Card> Pocket, List<Deck.Card> Board, List<double[]> weights)
        {
            double strength = 0;
            foreach(var w in weights)
            {
                strength += WeightedHandStrength(Pocket, Board, w);
            }
            return Math.Pow(strength/weights.Count, weights.Count);
        }

        public static double WeightedHandStrength(List<Deck.Card> Pocket, List<Deck.Card> Board, double[] weights)
        {
            return WHandStrength(GetMask(Pocket), GetMask(Board), weights);
        }

        public static double RawHandStrength(List<Deck.Card> Pocket, List<Deck.Card> Board, int OpponentsCount)
        {
            return Math.Pow(Hand.HandStrength(GetMask(Pocket), GetMask(Board)), OpponentsCount);
        }

        public static double RawHandStrength(List<Deck.Card> Pocket, List<Deck.Card> Board)
        {
            return Hand.HandStrength(GetMask(Pocket), GetMask(Board));
        }
        #endregion

        #region Outs
        public static int Outs(List<Deck.Card> pocket, List<Deck.Card> board)
        {
            return Hand.Outs(GetMask(pocket), GetMask(board));
        }

        public static List<Deck.Card> OutsCards(List<Deck.Card> Pocket, List<Deck.Card> Board)
        {
            return FromMask(Hand.OutsMask(GetMask(Pocket), GetMask(Board)));
        }
        #endregion

        #region Helpers
        public static List<Deck.Card> FromMask(ulong mask, List<Deck.Card> board)
        {
            return FromMask(mask, GetMask(board));
        }

        public static List<Deck.Card> FromMask(ulong mask, ulong board = 0UL)
        {
            if (board != 0UL) mask |= board;
            List<Deck.Card> cards = new List<Deck.Card>();
            foreach (var card in Hand.MaskToPockerShark(mask))
            {
                cards.Add(new Deck.Card(card));
            }
            return cards;
        }

        public static ulong GetMask(List<Deck.Card> cards)
        {
            return Hand.ParseHand(GetAsString(cards));
        }

        public static string GetAsString(List<Deck.Card> cards)
        {
            if (cards == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (Deck.Card card in cards)
                sb.Append(card.ToString() + " ");
            return sb.ToString();
        }

        public static String CardsToString(List<Deck.Card> cards)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            stringBuilder.Append(String.Join(", ", cards));
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        #endregion

    }
}
