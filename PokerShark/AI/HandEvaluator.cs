using HoldemHand;
using PokerShark.Poker.Deck;
using System.Text;

namespace PokerShark.AI
{
    internal class HandEvaluator
    {
        #region HandMap
        public static Dictionary<ulong, TableCard> HandRangeMap = new Dictionary<ulong, TableCard>() {
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
        #endregion

        #region PocketEquity
        private static Dictionary<string, double[]> PocketEquity = new Dictionary<string, double[]>(){
     { "AA", new double[] {85.3, 73.4, 63.9, 55.9, 49.2, 43.6, 38.8, 34.7, 31.1}},
     { "AKs", new double[] {67.0, 50.7, 41.4, 35.4, 31.1, 27.7, 25.0, 22.7, 20.7}},
     { "AK", new double[] {65.4, 48.2, 38.6, 32.4, 27.9, 24.4, 21.6, 19.2, 17.2}},
     { "AQs", new double[] {66.1, 49.4, 39.9, 33.7, 29.4, 26.0, 23.3, 21.1, 19.3}},
     { "AQ", new double[] {64.5, 46.8, 36.9, 30.4, 25.9, 22.5, 19.7, 17.5, 15.5}},
     { "AJs", new double[] {65.4, 48.2, 38.5, 32.2, 27.8, 24.5, 22.0, 19.9, 18.1}},
     { "AJ", new double[] {63.6, 45.6, 35.4, 28.9, 24.4, 21.0, 18.3, 16.1, 14.3}},
     { "ATs", new double[] {64.7, 47.1, 37.2, 31.0, 26.7, 23.5, 21.0, 18.9, 17.3}},
     { "AT", new double[] {62.9, 44.4, 34.1, 27.6, 23.1, 19.8, 17.2, 15.1, 13.4}},
     { "A9s", new double[] {63.0, 44.8, 34.6, 28.4, 24.2, 21.1, 18.8, 16.9, 15.4}},
     { "A9", new double[] {60.9, 41.8, 31.2, 24.7, 20.3, 17.1, 14.7, 12.8, 11.2}},
     { "A8s", new double[] {62.1, 43.7, 33.6, 27.4, 23.3, 20.3, 18.0, 16.2, 14.8}},
     { "A8", new double[] {60.1, 40.8, 30.1, 23.7, 19.4, 16.2, 13.9, 12.0, 10.6}},
     { "A7s", new double[] {61.1, 42.6, 32.6, 26.5, 22.5, 19.6, 17.4, 15.7, 14.3}},
     { "A7", new double[] {59.1, 39.4, 28.9, 22.6, 18.4, 15.4, 13.2, 11.4, 10.1}},
     { "A6s", new double[] {60.0, 41.3, 31.4, 25.6, 21.7, 19.0, 16.9, 15.3, 14.0}},
     { "A6", new double[] {57.8, 38.0, 27.6, 21.5, 17.5, 14.7, 12.6, 10.9, 9.6}},
     { "A5s", new double[] {59.9, 41.4, 31.8, 26.0, 22.2, 19.6, 17.5, 15.9, 14.5}},
     { "A5", new double[] {57.7, 38.2, 27.9, 22.0, 18.0, 15.2, 13.1, 11.5, 10.1}},
     { "A4s", new double[] {58.9, 40.4, 30.9, 25.3, 21.6, 19.0, 17.0, 15.5, 14.2}},
     { "A4", new double[] {56.4, 36.9, 26.9, 21.1, 17.3, 14.7, 12.6, 11.0, 9.8}},
     { "A3s", new double[] {58.0, 39.4, 30.0, 24.6, 21.0, 18.5, 16.6, 15.1, 13.9}},
     { "A3", new double[] {55.6, 35.9, 26.1, 20.4, 16.7, 14.2, 12.2, 10.7, 9.5}},
     { "A2s", new double[] {57.0, 38.5, 29.2, 23.9, 20.4, 18.0, 16.1, 14.6, 13.4}},
     { "A2", new double[] {54.6, 35.0, 25.2, 19.6, 16.1, 13.6, 11.7, 10.2, 9.1}},
     { "KK", new double[] {82.4, 68.9, 58.2, 49.8, 43.0, 37.5, 32.9, 29.2, 26.1}},
     { "KQs", new double[] {63.4, 47.1, 38.2, 32.5, 28.3, 25.1, 22.5, 20.4, 18.6}},
     { "KQ", new double[] {61.4, 44.4, 35.2, 29.3, 25.1, 21.8, 19.1, 16.9, 15.1}},
     { "KJs", new double[] {62.6, 45.9, 36.8, 31.1, 26.9, 23.8, 21.3, 19.3, 17.6}},
     { "KJ", new double[] {60.6, 43.1, 33.6, 27.6, 23.5, 20.2, 17.7, 15.6, 13.9}},
     { "KTs", new double[] {61.9, 44.9, 35.7, 29.9, 25.8, 22.8, 20.4, 18.5, 16.9}},
     { "KT", new double[] {59.9, 42.0, 32.5, 26.5, 22.3, 19.2, 16.7, 14.7, 13.1}},
     { "K9s", new double[] {60.0, 42.4, 32.9, 27.2, 23.2, 20.3, 18.1, 16.3, 14.8}},
     { "K9", new double[] {58.0, 39.5, 29.6, 23.6, 19.5, 16.5, 14.1, 12.3, 10.8}},
     { "K8s", new double[] {58.5, 40.2, 30.8, 25.1, 21.3, 18.6, 16.5, 14.8, 13.5}},
     { "K8", new double[] {56.3, 37.2, 27.3, 21.4, 17.4, 14.6, 12.5, 10.8, 9.4}},
     { "K7s", new double[] {57.8, 39.4, 30.1, 24.5, 20.8, 18.1, 16.0, 14.5, 13.2}},
     { "K7", new double[] {55.4, 36.1, 26.3, 20.5, 16.7, 13.9, 11.8, 10.2, 9.0}},
     { "K6s", new double[] {56.8, 38.4, 29.1, 23.7, 20.1, 17.5, 15.6, 14.0, 12.8}},
     { "K6", new double[] {54.3, 35.0, 25.3, 19.7, 16.0, 13.3, 11.3, 9.8, 8.6}},
     { "K5s", new double[] {55.8, 37.4, 28.2, 23.0, 19.5, 17.0, 15.2, 13.7, 12.5}},
     { "K5", new double[] {53.3, 34.0, 24.5, 19.0, 15.4, 12.9, 11.0, 9.5, 8.3}},
     { "K4s", new double[] {54.7, 36.4, 27.4, 22.3, 19.0, 16.6, 14.8, 13.4, 12.3}},
     { "K4", new double[] {52.1, 32.8, 23.4, 18.1, 14.7, 12.3, 10.5, 9.1, 8.0}},
     { "K3s", new double[] {53.8, 35.5, 26.7, 21.7, 18.4, 16.2, 14.5, 13.1, 12.1}},
     { "K3", new double[] {51.2, 31.9, 22.7, 17.6, 14.2, 11.9, 10.2, 8.9, 7.8}},
     { "K2s", new double[] {52.9, 34.6, 26.0, 21.2, 18.1, 15.9, 14.3, 13.0, 11.9}},
     { "K2", new double[] {50.2, 30.9, 21.8, 16.9, 13.7, 11.5, 9.8, 8.6, 7.6}},
     { "QQ", new double[] {79.9, 64.9, 53.5, 44.7, 37.9, 32.5, 28.3, 24.9, 22.2}},
     { "QJs", new double[] {60.3, 44.1, 35.6, 30.1, 26.1, 23.0, 20.7, 18.7, 17.1}},
     { "QJ", new double[] {58.2, 41.4, 32.6, 26.9, 22.9, 19.8, 17.3, 15.3, 13.7}},
     { "QTs", new double[] {59.5, 43.1, 34.6, 29.1, 25.2, 22.3, 19.9, 18.1, 16.6}},
     { "QT", new double[] {57.4, 40.2, 31.3, 25.7, 21.6, 18.6, 16.3, 14.4, 12.9}},
     { "Q9s", new double[] {57.9, 40.7, 31.9, 26.4, 22.5, 19.7, 17.6, 15.9, 14.5}},
     { "Q9", new double[] {55.5, 37.6, 28.5, 22.9, 19.0, 16.1, 13.8, 12.1, 10.7}},
     { "Q8s", new double[] {56.2, 38.6, 29.7, 24.4, 20.7, 18.0, 16.0, 14.4, 13.2}},
     { "Q8", new double[] {53.8, 35.4, 26.2, 20.6, 16.9, 14.1, 12.1, 10.5, 9.2}},
     { "Q7s", new double[] {54.5, 36.7, 27.9, 22.7, 19.2, 16.7, 14.8, 13.3, 12.1}},
     { "Q7", new double[] {51.9, 33.2, 24.0, 18.6, 15.1, 12.5, 10.6, 9.2, 8.0}},
     { "Q6s", new double[] {53.8, 35.8, 27.1, 21.9, 18.5, 16.1, 14.3, 12.9, 11.7}},
     { "Q6", new double[] {51.1, 32.3, 23.2, 17.9, 14.4, 12.0, 10.1, 8.8, 7.6}},
     { "Q5s", new double[] {52.9, 34.9, 26.3, 21.4, 18.1, 15.8, 14.1, 12.7, 11.6}},
     { "Q5", new double[] {50.2, 31.3, 22.3, 17.3, 13.9, 11.6, 9.8, 8.5, 7.4}},
     { "Q4s", new double[] {51.7, 33.9, 25.5, 20.7, 17.6, 15.4, 13.7, 12.4, 11.3}},
     { "Q4", new double[] {49.0, 30.2, 21.4, 16.4, 13.3, 11.0, 9.4, 8.1, 7.1}},
     { "Q3s", new double[] {50.7, 33.0, 24.7, 20.1, 17.0, 14.9, 13.3, 12.1, 11.1}},
     { "Q3", new double[] {47.9, 29.2, 20.7, 15.9, 12.8, 10.7, 9.1, 7.9, 6.9}},
     { "Q2s", new double[] {49.9, 32.2, 24.0, 19.5, 16.6, 14.6, 13.1, 11.9, 10.9}},
     { "Q2", new double[] {47.0, 28.4, 19.9, 15.3, 12.3, 10.3, 8.8, 7.7, 6.8}},
     { "JJ", new double[] {77.5, 61.2, 49.2, 40.3, 33.6, 28.5, 24.6, 21.6, 19.3}},
     { "JTs", new double[] {57.5, 41.9, 33.8, 28.5, 24.7, 21.9, 19.7, 17.9, 16.5}},
     { "JT", new double[] {55.4, 39.0, 30.7, 25.3, 21.5, 18.6, 16.3, 14.5, 13.1}},
     { "J9s", new double[] {55.8, 39.6, 31.3, 26.1, 22.4, 19.7, 17.6, 15.9, 14.6}},
     { "J9", new double[] {53.4, 36.5, 27.9, 22.5, 18.7, 15.9, 13.8, 12.1, 10.8}},
     { "J8s", new double[] {54.2, 37.5, 29.1, 24.0, 20.5, 17.9, 15.9, 14.4, 13.2}},
     { "J8", new double[] {51.7, 34.2, 25.6, 20.4, 16.8, 14.1, 12.2, 10.7, 9.5}},
     { "J7s", new double[] {52.4, 35.4, 27.1, 22.2, 18.9, 16.4, 14.6, 13.2, 12.0}},
     { "J7", new double[] {49.9, 32.1, 23.5, 18.3, 14.9, 12.4, 10.6, 9.2, 8.1}},
     { "J6s", new double[] {50.8, 33.6, 25.4, 20.6, 17.4, 15.2, 13.5, 12.1, 11.1}},
     { "J6", new double[] {47.9, 29.8, 21.4, 16.5, 13.2, 11.0, 9.3, 8.0, 7.0}},
     { "J5s", new double[] {50.0, 32.8, 24.7, 20.0, 17.0, 14.7, 13.1, 11.8, 10.8}},
     { "J5", new double[] {47.1, 29.1, 20.7, 15.9, 12.8, 10.6, 8.9, 7.7, 6.7}},
     { "J4s", new double[] {49.0, 31.8, 24.0, 19.4, 16.4, 14.3, 12.8, 11.5, 10.6}},
     { "J4", new double[] {46.1, 28.1, 19.9, 15.3, 12.3, 10.2, 8.6, 7.5, 6.5}},
     { "J3s", new double[] {47.9, 30.9, 23.2, 18.8, 16.0, 14.0, 12.5, 11.3, 10.4}},
     { "J3", new double[] {45.0, 27.1, 19.1, 14.6, 11.7, 9.8, 8.3, 7.2, 6.3}},
     { "J2s", new double[] {47.1, 30.1, 22.6, 18.3, 15.6, 13.7, 12.2, 11.1, 10.2}},
     { "J2", new double[] {44.0, 26.2, 18.4, 14.1, 11.3, 9.4, 8.0, 7.0, 6.2}},
     { "TT", new double[] {75.1, 57.7, 45.2, 36.4, 30.0, 25.3, 21.8, 19.2, 17.2}},
     { "T9s", new double[] {54.3, 38.9, 31.0, 26.0, 22.5, 19.8, 17.8, 16.2, 14.9}},
     { "T9", new double[] {51.7, 35.7, 27.7, 22.5, 18.9, 16.2, 14.1, 12.6, 11.3}},
     { "T8s", new double[] {52.6, 36.9, 29.0, 24.0, 20.6, 18.1, 16.2, 14.8, 13.6}},
     { "T8", new double[] {50.0, 33.6, 25.4, 20.4, 16.9, 14.4, 12.5, 11.0, 9.9}},
     { "T7s", new double[] {51.0, 34.9, 27.0, 22.2, 19.0, 16.6, 14.8, 13.5, 12.4}},
     { "T7", new double[] {48.2, 31.4, 23.4, 18.4, 15.1, 12.8, 11.0, 9.7, 8.6}},
     { "T6s", new double[] {49.2, 32.8, 25.1, 20.5, 17.4, 15.2, 13.6, 12.3, 11.2}},
     { "T6", new double[] {46.3, 29.2, 21.2, 16.5, 13.4, 11.2, 9.5, 8.3, 7.3}},
     { "T5s", new double[] {47.2, 30.8, 23.3, 18.9, 16.0, 13.9, 12.4, 11.2, 10.2}},
     { "T5", new double[] {44.2, 27.1, 19.3, 14.8, 11.9, 9.9, 8.4, 7.2, 6.4}},
     { "T4s", new double[] {46.4, 30.1, 22.7, 18.4, 15.6, 13.6, 12.1, 11.0, 10.0}},
     { "T4", new double[] {43.4, 26.4, 18.7, 14.3, 11.5, 9.5, 8.1, 7.0, 6.2}},
     { "T3s", new double[] {45.5, 29.3, 22.0, 17.8, 15.1, 13.2, 11.8, 10.7, 9.8}},
     { "T3", new double[] {42.4, 25.5, 18.0, 13.7, 11.0, 9.1, 7.8, 6.8, 6.0}},
     { "T2s", new double[] {44.7, 28.5, 21.4, 17.4, 14.8, 13.0, 11.6, 10.5, 9.7}},
     { "T2", new double[] {41.5, 24.7, 17.3, 13.2, 10.6, 8.8, 7.5, 6.6, 5.8}},
     { "99", new double[] {72.1, 53.5, 41.1, 32.6, 26.6, 22.4, 19.4, 17.2, 15.6}},
     { "98s", new double[] {51.1, 36.0, 28.5, 23.6, 20.2, 17.8, 15.9, 14.5, 13.4}},
     { "98", new double[] {48.4, 32.9, 25.1, 20.1, 16.6, 14.2, 12.3, 10.9, 9.9}},
     { "97s", new double[] {49.5, 34.2, 26.8, 22.1, 18.9, 16.6, 14.9, 13.6, 12.5}},
     { "97", new double[] {46.7, 30.9, 23.1, 18.4, 15.1, 12.8, 11.1, 9.8, 8.8}},
     { "96s", new double[] {47.7, 32.3, 24.9, 20.4, 17.4, 15.3, 13.7, 12.4, 11.4}},
     { "96", new double[] {44.9, 28.8, 21.2, 16.6, 13.5, 11.4, 9.8, 8.7, 7.8}},
     { "95s", new double[] {45.9, 30.4, 23.2, 18.8, 16.0, 13.9, 12.4, 11.3, 10.3}},
     { "95", new double[] {42.9, 26.7, 19.2, 14.8, 12.0, 10.0, 8.5, 7.4, 6.6}},
     { "94s", new double[] {43.8, 28.4, 21.3, 17.3, 14.6, 12.7, 11.3, 10.3, 9.4}},
     { "94", new double[] {40.7, 24.6, 17.3, 13.2, 10.5, 8.7, 7.3, 6.4, 5.6}},
     { "93s", new double[] {43.2, 27.8, 20.8, 16.8, 14.3, 12.5, 11.1, 10.1, 9.2}},
     { "93", new double[] {39.9, 23.9, 16.7, 12.7, 10.1, 8.3, 7.1, 6.1, 5.4}},
     { "92s", new double[] {42.3, 27.0, 20.2, 16.4, 13.9, 12.2, 10.9, 9.9, 9.1}},
     { "92", new double[] {38.9, 22.9, 16.0, 12.1, 9.6, 8.0, 6.8, 5.9, 5.2}},
     { "88", new double[] {69.1, 49.9, 37.5, 29.4, 24.0, 20.3, 17.7, 15.8, 14.4}},
     { "87s", new double[] {48.2, 33.9, 26.6, 22.0, 18.9, 16.7, 15.0, 13.7, 12.7}},
     { "87", new double[] {45.5, 30.6, 23.2, 18.5, 15.4, 13.1, 11.5, 10.3, 9.3}},
     { "86s", new double[] {46.5, 32.0, 25.0, 20.6, 17.6, 15.6, 14.1, 12.9, 11.9}},
     { "86", new double[] {43.6, 28.6, 21.3, 16.9, 13.9, 11.8, 10.4, 9.2, 8.3}},
     { "85s", new double[] {44.8, 30.2, 23.2, 19.1, 16.3, 14.3, 12.9, 11.8, 10.9}},
     { "85", new double[] {41.7, 26.5, 19.4, 15.2, 12.4, 10.5, 9.1, 8.1, 7.3}},
     { "84s", new double[] {42.7, 28.1, 21.4, 17.4, 14.8, 13.0, 11.7, 10.6, 9.8}},
     { "84", new double[] {39.6, 24.4, 17.5, 13.4, 10.8, 9.0, 7.8, 6.8, 6.1}},
     { "83s", new double[] {40.8, 26.3, 19.8, 16.0, 13.6, 11.9, 10.7, 9.7, 8.9}},
     { "83", new double[] {37.5, 22.4, 15.7, 11.9, 9.5, 7.9, 6.7, 5.8, 5.1}},
     { "82s", new double[] {40.3, 25.8, 19.4, 15.7, 13.3, 11.7, 10.5, 9.6, 8.8}},
     { "82", new double[] {36.8, 21.7, 15.1, 11.4, 9.1, 7.5, 6.4, 5.6, 4.9}},
     { "77", new double[] {66.2, 46.4, 34.4, 26.8, 21.9, 18.6, 16.4, 14.8, 13.7}},
     { "76s", new double[] {45.7, 32.0, 25.1, 20.8, 18.0, 15.9, 14.4, 13.2, 12.3}},
     { "76", new double[] {42.7, 28.5, 21.5, 17.1, 14.2, 12.2, 10.8, 9.6, 8.8}},
     { "75s", new double[] {43.8, 30.1, 23.4, 19.4, 16.7, 14.8, 13.4, 12.3, 11.4}},
     { "75", new double[] {40.8, 26.5, 19.7, 15.5, 12.8, 11.0, 9.7, 8.7, 7.9}},
     { "74s", new double[] {41.8, 28.2, 21.7, 17.9, 15.3, 13.5, 12.2, 11.2, 10.4}},
     { "74", new double[] {38.6, 24.5, 17.9, 13.9, 11.4, 9.7, 8.5, 7.6, 6.8}},
     { "73s", new double[] {40.0, 26.3, 20.0, 16.4, 14.0, 12.3, 11.1, 10.1, 9.3}},
     { "73", new double[] {36.6, 22.4, 16.0, 12.3, 9.9, 8.4, 7.2, 6.4, 5.7}},
     { "72s", new double[] {38.1, 24.5, 18.4, 15.0, 12.8, 11.2, 10.1, 9.2, 8.5}},
     { "72", new double[] {34.6, 20.4, 14.2, 10.7, 8.6, 7.2, 6.1, 5.4, 4.8}},
     { "66", new double[] {63.3, 43.2, 31.5, 24.5, 20.1, 17.3, 15.4, 14.0, 13.1}},
     { "65s", new double[] {43.2, 30.2, 23.7, 19.7, 17.0, 15.2, 13.8, 12.7, 11.9}},
     { "65", new double[] {40.1, 26.7, 20.0, 15.9, 13.3, 11.5, 10.2, 9.2, 8.5}},
     { "64s", new double[] {41.4, 28.5, 22.1, 18.4, 15.9, 14.2, 12.9, 11.9, 11.1}},
     { "64", new double[] {38.0, 24.7, 18.2, 14.4, 12.0, 10.3, 9.2, 8.3, 7.6}},
     { "63s", new double[] {39.4, 26.5, 20.4, 16.8, 14.5, 12.9, 11.7, 10.8, 10.0}},
     { "63", new double[] {35.9, 22.7, 16.4, 12.8, 10.6, 9.1, 8.0, 7.2, 6.5}},
     { "62s", new double[] {37.5, 24.8, 18.8, 15.4, 13.3, 11.8, 10.7, 9.8, 9.1}},
     { "62", new double[] {34.0, 20.7, 14.6, 11.2, 9.1, 7.8, 6.8, 6.0, 5.4}},
     { "55", new double[] {60.3, 40.1, 28.8, 22.4, 18.5, 16.0, 14.4, 13.2, 12.3}},
     { "54s", new double[] {41.1, 28.8, 22.6, 18.9, 16.5, 14.8, 13.5, 12.5, 11.7}},
     { "54", new double[] {37.9, 25.2, 18.8, 15.0, 12.6, 11.0, 9.8, 8.9, 8.2}},
     { "53s", new double[] {39.3, 27.1, 21.1, 17.5, 15.2, 13.7, 12.5, 11.6, 10.8}},
     { "53", new double[] {35.8, 23.3, 17.1, 13.6, 11.4, 9.9, 8.8, 8.0, 7.3}},
     { "52s", new double[] {37.5, 25.3, 19.5, 16.1, 14.0, 12.5, 11.4, 10.6, 9.8}},
     { "52", new double[] {33.9, 21.3, 15.3, 12.0, 10.0, 8.6, 7.6, 6.8, 6.2}},
     { "44", new double[] {57.0, 36.8, 26.3, 20.6, 17.3, 15.2, 13.9, 12.9, 12.1}},
     { "43s", new double[] {38.0, 26.2, 20.3, 16.9, 14.7, 13.1, 12.0, 11.1, 10.3}},
     { "43", new double[] {34.4, 22.3, 16.3, 12.8, 10.7, 9.3, 8.3, 7.5, 6.8}},
     { "42s", new double[] {36.3, 24.6, 18.8, 15.7, 13.7, 12.3, 11.2, 10.4, 9.6}},
     { "42", new double[] {32.5, 20.5, 14.7, 11.5, 9.5, 8.3, 7.3, 6.6, 6.0}},
     { "33", new double[] {53.7, 33.5, 23.9, 19.0, 16.2, 14.6, 13.5, 12.6, 12.0}},
     { "32s", new double[] {35.1, 23.6, 18.0, 14.9, 13.0, 11.7, 10.7, 9.9, 9.2}},
     { "32", new double[] {31.2, 19.5, 13.9, 10.8, 8.9, 7.7, 6.8, 6.1, 5.6}},
     { "22", new double[] {50.3, 30.7, 22.0, 17.8, 15.5, 14.2, 13.3, 12.5, 12.0}},
};

        public static double GetPocketEquity(Pocket pocket, int PlayerNumber)
        {
            if (PlayerNumber > 10)
                return 0;
            return PocketEquity[pocket.ToString()][PlayerNumber - 2];
        }
        #endregion 

        #region HandStrength
        public static double HandStrength(List<Card> Pocket, List<Card> Board, List<PlayerModel> models)
        {
            double strength = 1;
            foreach (var model in models)
            {
                strength *= HandStrength(GetMask(Pocket), GetMask(Board), model.WeightTable.Table);
            }
            return strength;
        }
        private static double HandStrength(ulong pocket, ulong board, double[] weights)
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
        #endregion

        #region MonteCarlo
        public static double MonteCarloHandStrength(List<Card> Pocket, List<Card> Board, List<PlayerModel> models)
        {
            return MonteCarloHandStrength(GetMask(Pocket), GetMask(Board), models, 0.9);
        }

        public static double MonteCarloHandStrength(ulong pocket, ulong board, List<PlayerModel> models, double duration)
        {
            // initialize win counter.
            double win = 0.0, count = 0.0;

            // store start time.
            double starttime = Hand.CurrentTime;

            // seed random. 
            Random rand = new Random();

            // check opponent count.
            if (models.Count < 1 || models.Count > 9) throw new Exception("player count has to be between 1 and 9");

            // calculate our pocket evaluation
            uint ourrank = Hand.Evaluate(pocket | board);

            // array that holds opponent hands.
            ulong[] oppcards = new ulong[models.Count];

            while ((Hand.CurrentTime - starttime) < duration)
            {
                // for each opponent generate random hand and calculate its rank.

                for (int i = 0; i < models.Count; i++)
                {
                    var dead = pocket | board;
                    // add opponent cards to dead cards.
                    for (int j = 0; j < i; j++)
                    {
                        dead |= oppcards[j];
                    }
                    // generate random opponent hand.
                    oppcards[i] = Hand.RandomHand(rand, 0UL, dead, 2);
                }

                // calculate opponent ranks.
                uint[] oppranks = new uint[models.Count];
                for (int i = 0; i < models.Count; i++)
                {
                    oppranks[i] = Hand.Evaluate(oppcards[i] | board);
                }

                // find best opponent hand.
                uint bestopp = oppranks.Max();

                // find opponent index
                int index = Array.IndexOf(oppranks, bestopp);

                // find opponent hand weight
                double weight = models[index].WeightTable.Table[(int)HandRangeMap[oppcards[index]]];

                if (ourrank > bestopp)
                {
                    win += 1.0 * weight;
                }
                else if (ourrank == bestopp)
                {
                    win += 0.5 * weight;
                }
                count += 1.0;
            }
            return win / count;
        }

        #endregion

        #region HandPotential
        public static (double ppot, double npot) HandPotential(List<Card> Pocket, List<Card> Board, List<PlayerModel> models)
        {
            double ppot = 1;
            double npot = 1;
            foreach (var model in models)
            {
                var (pp, np) = HandPotential(GetMask(Pocket), GetMask(Board), model.WeightTable.Table);
                ppot *= pp;
                npot *= np;
            }
            return (Math.Round(ppot, 4), Math.Round(npot, 4));
        }
        private static (double ppot, double npot) HandPotential(ulong pocket, ulong board, double[] weights)
        {
            double ppot = 0;
            double npot = 0;
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

            return (ppot, npot);
        }
        #endregion

        #region Helpers
        private static ulong GetMask(List<Card> cards)
        {
            return Hand.ParseHand(GetAsString(cards));
        }
        public static string GetAsString(List<Card> cards)
        {
            if (cards == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (Card card in cards)
                sb.Append(card.ToHoldemCard() + " ");
            return sb.ToString();
        }
        #endregion
    }
}
