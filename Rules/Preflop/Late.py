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
Calls= 0
Hand = "AA"


# logic

if Raises == 0:
    if Hand in [*Group1,*Group2, *Group3, *Group4, *Group5, *Group6, *Group7]:
        Raise("Always", "unkown yet")
        

if Calls > 0:
    if Hand in [*Group1,*Group2, *Group3]:
        Raise("Always", "unkown yet")

    if Hand in [*Group4]:
        if Hand in ["KQ", "AJ"]:
            Fold()
        Raise("Always", "unkown yet")
