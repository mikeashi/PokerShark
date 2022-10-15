# Early Position

#--
# The number of hands that can be played from early position is 
# quite limited. Since you are out of position on all betting
# rounds, you need a superior starting hand to make it worth
# playing.
#--


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
LooseGame = True
TightGame = True
PassiveGame = True
AggressiveGame = True
Occasionally = True
LooseRaiser = True
Calls = 1
Raises = 1
Hand = "AA"



# if you are the first one in, or if there is only a call to your right
if Calls == 0:
    if Hand in ["AA", "KK","QQ", "AK", "AQ"]:
        # TODO set amount
        Raise("Always", "unkown yet")

if Raises == 0:
    if Hand in [*Group1,*Group2]:
        # TODO set amount
        Raise("Usually", "unkown yet")
    if Hand in ["AQ"]:
        # TODO set amount
        Raise("Usually", "unkown yet")
    if Hand in [*Group3]:
        # TODO set amount
        Raise("50 raise, 50 call", "unkown yet")
    if LooseGame:
        if AggressiveGame and Hand in ["AJ", "KTs"]:
            Fold()
        if Hand in [*Group4]:
            # TODO set amount
            Raise("50 raise, 50 call ", "unkown yet")
        if PassiveGame:
            if Hand in ["87s","76s", "65s"]:
                # TODO set amount
                Raise("50 raise, 50 call ", "unkown yet")
            if Hand in [*Group5]:
                # TODO set amount
                Raise("50 raise, 50 call ", "unkown yet")
    
    # Stop observant players from stealing your blinds.
    if Occasionally and Hand in ["87s","76s", "65s"]:
        # TODO set amount
        Raise("Occasionally", "unkown yet")

if Raises == 1:
    if Hand in [*Group1,*Group2]:
        if Hand in ["AJs", "KQs"]:
            Call()
        # TODO set amount
        Raise("Usually", "unkown yet")
    if LooseRaiser:
        if Hand in ["AQ", "99", "88"]:
            # TODO set amount
            Raise("Usually", "unkown yet")

if Raises > 1:
    if LooseGame:
        if Hand in "AQ":
            Fold()
        if Hand in [*Group1,*Group2, *Group3]:
            # TODO set amount
            Raise("Usually", "unkown yet")
    if TightGame:
        if Hand in ["AJs", "KQs"]:
            Fold()
        if Hand in [*Group1, *Group2]:
            # TODO set amount
            Raise("Usually", "unkown yet")
Fold()   
