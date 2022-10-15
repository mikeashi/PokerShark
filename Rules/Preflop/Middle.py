
# Setup

Group1 = ["AA","KK", "QQ", "JJ", "AKs"]
Group2 = ["TT", "AQs", "AJs", "KQs", "AK"]
Group3 = ["99", "JTs", "QJs", "KJs", "ATs", "AQ"]
Group4 = ["T9s", "KQ", "88", "QTs", "98s", "J9s", "AJ", "KTs"]
Group5 = ["77", "87s", "Q9s", "T8s", "KJ", "QJ", "JT", "67s", "97s", "A9s", "A8s", "A7s", "A6s", "A5s", "A4s", "A3s", "A2s"]
Group6 = ["66", "AT", "55", "86s", "KT", "QT", "54s", "K9s", "J8s", "75s"]
Group7 = ["44, J9", "64s", "T9", "53s", "33", "98", "43s", "22", "K8s", "K7s", "K6s", "K5s", "K4s", "K3s", "K2s", "T7s", "Q8s"]



def Fold():
    print("Fold")

def Call():
    print("Call")

def Raise(when, amount):
    print("Raise: ", when, ", Amount :", amount)

# init
Raises = 0
LooseGame = True
PassiveGame = True
AggressiveGame = True
TightGame = True
Hand = "AA"
Calls = 1
Occasionally = False
LooseRaiser = False

# logic
if Hand in [*Group1,*Group2]:
        # TODO set amount 4 or 5 BB
        Raise("Always", "unkown yet")

if Calls > 0:
    if Hand in ["AQ"]:
        # TODO set amount 4 or 5 BB
        Raise("Always", "unkown yet")
    
    if Hand in ["JTs"]:
        Call()

    if Hand in ["AJ", "KQ"]:
        # TODO set amount 4 or 5 BB
        Raise("60 percent", "unkown yet")

    if Hand in [*Group3]:
        # TODO set amount 4 or 5 BB
        Raise("50 Prc of the time", "unkown yet")
    pass

if Raises == 0:
    if Hand in [*Group3]:
        # TODO set amount 4 or 5 BB
        Raise("Always", "unkown yet")
    if LooseGame:
        if AggressiveGame:
            if Hand in ["KJ", "T8s"]:
                Fold()
            pass
        if Hand in [*Group4, *Group5, *Group6]:
            Raise("Almost Always", "unkown yet")
        pass
    if TightGame:
        # play groups 1,5
        if Hand in [*Group4, *Group5]:
            # 3 or 4 BB
            Raise("Usually", "unkown yet")
        pass
    pass


if Raises == 1:
    if LooseRaiser:
        if Hand in ["AQ", "99", "88"]:
            # TODO set amount
            Raise("Usually", "unkown yet")

if Raises > 1:
    if Hand in ["AA", "KK", "QQ", "AKs", "AK"]:
        # TODO set amount 4 or 5 BB
        Raise("Always", "unkown yet")
    if Occasionally and  Hand in ["T9s", "88"]:
        # TODO set amount 4 or 5 BB
        Raise("Always", "unkown yet")

Fold()