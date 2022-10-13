using PokerShark.Core.HTN;
using PokerShark.Core.PyPoker;

namespace PokerShark.UnitTests.Core
{
    [TestClass]
    public class WeightedSelectionTest
    {
        [TestMethod]
        public void TestCallDecision()
        {
            // setup
            (float Fold, float Call, float Raise) decision = (Fold: 0, Call: 1, Raise: 0);
            List<PyAction> actions = new List<PyAction>();
            actions.Add(new FoldAction());
            actions.Add(new CallAction(1));
            actions.Add(new RaiseAction(1,2));
            // execution
            List<PyAction> selectedActions = new List<PyAction>();
            for(int i =0; i<100; i++)
            {
                selectedActions.Add(PokerPlanner.SelectAction(decision, actions));
            }
            // parse result
            int calls = 0;
            int folds = 0;
            int raises = 0;
            foreach(PyAction action in selectedActions)
            {
                if(action is FoldAction)
                    folds++;
                if(action is CallAction)
                    calls++;
                if(action is RaiseAction)
                    raises++;
            }
            // assertion
            Assert.AreEqual(calls, 100);
            Assert.AreEqual(folds, 0);
            Assert.AreEqual(raises, 0);
        }

        [TestMethod]
        public void TestFoldDecision()
        {
            // setup
            (float Fold, float Call, float Raise) decision = (Fold: 1, Call: 0, Raise: 0);
            List<PyAction> actions = new List<PyAction>();
            actions.Add(new FoldAction());
            actions.Add(new CallAction(1));
            actions.Add(new RaiseAction(1, 2));
            // execution
            List<PyAction> selectedActions = new List<PyAction>();
            for (int i = 0; i < 100; i++)
            {
                selectedActions.Add(PokerPlanner.SelectAction(decision, actions));
            }
            // parse result
            int calls = 0;
            int folds = 0;
            int raises = 0;
            foreach (PyAction action in selectedActions)
            {
                if (action is FoldAction)
                    folds++;
                if (action is CallAction)
                    calls++;
                if (action is RaiseAction)
                    raises++;
            }
            // assertion
            Assert.AreEqual(calls, 0);
            Assert.AreEqual(folds, 100);
            Assert.AreEqual(raises, 0);
        }

        [TestMethod]
        public void TestRaiseDecision()
        {
            // setup
            (float Fold, float Call, float Raise) decision = (Fold: 0, Call: 0, Raise: 1);
            List<PyAction> actions = new List<PyAction>();
            actions.Add(new FoldAction());
            actions.Add(new CallAction(1));
            actions.Add(new RaiseAction(1, 2));
            // execution
            List<PyAction> selectedActions = new List<PyAction>();
            for (int i = 0; i < 100; i++)
            {
                selectedActions.Add(PokerPlanner.SelectAction(decision, actions));
            }
            // parse result
            int calls = 0;
            int folds = 0;
            int raises = 0;
            foreach (PyAction action in selectedActions)
            {
                if (action is FoldAction)
                    folds++;
                if (action is CallAction)
                    calls++;
                if (action is RaiseAction)
                    raises++;
            }
            // assertion
            Assert.AreEqual(calls, 0);
            Assert.AreEqual(folds, 0);
            Assert.AreEqual(raises, 100);
        }


        [TestMethod]
        public void TestCallRaiseDecision()
        {
            // setup
            (float Fold, float Call, float Raise) decision = (Fold: 0, Call: 0.5f, Raise: 0.5f);
            List<PyAction> actions = new List<PyAction>();
            actions.Add(new FoldAction());
            actions.Add(new CallAction(1));
            actions.Add(new RaiseAction(1, 2));
            // execution
            List<PyAction> selectedActions = new List<PyAction>();
            for (int i = 0; i < 100; i++)
            {
                selectedActions.Add(PokerPlanner.SelectAction(decision, actions));
            }
            // parse result
            int calls = 0;
            int folds = 0;
            int raises = 0;
            foreach (PyAction action in selectedActions)
            {
                if (action is FoldAction)
                    folds++;
                if (action is CallAction)
                    calls++;
                if (action is RaiseAction)
                    raises++;
            }
            // assertion
            Assert.AreEqual(0, folds);
            Assert.IsTrue(40 < calls && calls < 60);
            Assert.IsTrue(40 < raises && raises < 60);
        }

        [TestMethod]
        public void TestCallFoldDecision()
        {
            // setup
            (float Fold, float Call, float Raise) decision = (Fold: 0.25f, Call: 0.75f, Raise: 0);
            List<PyAction> actions = new List<PyAction>();
            actions.Add(new FoldAction());
            actions.Add(new CallAction(1));
            actions.Add(new RaiseAction(1, 2));
            // execution
            List<PyAction> selectedActions = new List<PyAction>();
            for (int i = 0; i < 100; i++)
            {
                selectedActions.Add(PokerPlanner.SelectAction(decision, actions));
            }
            // parse result
            int calls = 0;
            int folds = 0;
            int raises = 0;
            foreach (PyAction action in selectedActions)
            {
                if (action is FoldAction)
                    folds++;
                if (action is CallAction)
                    calls++;
                if (action is RaiseAction)
                    raises++;
            }
            // assertion
            Assert.AreEqual(0, raises);
            Assert.IsTrue(65 < calls && calls < 85);
            Assert.IsTrue(20 < folds && folds < 35);
        }

    }
}